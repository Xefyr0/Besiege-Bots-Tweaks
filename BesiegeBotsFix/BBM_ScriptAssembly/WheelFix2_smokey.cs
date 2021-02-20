
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BotFix
{
    public class Wheelfix2_smoke : MonoBehaviour
    {
        private HingeJoint HJ;
        private CogMotorControllerHinge CM;
        private JointMotor JM;
        private Collider[] colliders;
        private MSlider GS;
        private MToggle EM;
        private MMenu PCMenu;
        private int PCselect = 3;
        public BlockBehaviour BB { get; internal set; }
        public float grip = 1;

        public PhysicMaterialCombine PC = PhysicMaterialCombine.Maximum;
        
        internal static List<string> PCmenul = new List<string>()
        {
            "Average",
            "Multiply",
            "Minimum",
            "Maximum",
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private Queue<float> averageAccelerationQueue = new Queue<float>();
        private Queue<Vector3> averageVelocityQueue = new Queue<Vector3>();
        private Queue<float> ATQ = new Queue<float>();
        private int queueSize = 60;
        private int TqueueSize = 150;
        private bool validBlock = true;
        private Vector3 lastPosition;
        private Vector3 lastVelocity;
        private bool isFirstFrame = false;

        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }
        public float Distance { get; private set; }
        public float Overload { get; private set; }
        public float Acceleration { get; private set; }

        public bool useEM = false;
        public float temp = 0;
        public int flipper = 0;

        public static ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        public ModTexture Smoketex = ModResource.GetTexture("Smoke3");
        public ParticleSystem SSmoke;
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (GetComponent<HingeJoint>())
                {
                    HJ = GetComponent<HingeJoint>();
                }
            }

            EM = BB.AddToggle("SMOKEY", "SMOKEY", useEM);
            EM.Toggled += (value) =>
            {
                useEM = value;
            };

            GS = BB.AddSlider("Friction", "Friction", grip, 0.1f, 10f);
            GS.ValueChanged += (float value) => { grip = value; };

            PCMenu = BB.AddMenu("Combine", PCselect, PCmenul, false);
            PCMenu.ValueChanged += (ValueHandler)(value => 
            {
                switch (value)
                {
                    case 0:
                        PC = PhysicMaterialCombine.Average;
                        break;
                    case 1:
                        PC = PhysicMaterialCombine.Multiply;
                        break;
                    case 2:
                        PC = PhysicMaterialCombine.Minimum;
                        break;
                    case 3:
                        PC = PhysicMaterialCombine.Maximum;
                        break;
                }
            });

            EM.DisplayInMapper = true;
            GS.DisplayInMapper = true;
            PCMenu.DisplayInMapper = true;

            if (!Game.IsSimulating)
                initSmokeFX();

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {

                    collider.material.dynamicFriction = grip;
                    collider.material.staticFriction = grip;
                    collider.material.frictionCombine = PC;
                }
            }
        }

        private void FixedUpdate()
        {
            
            if (useEM)
            {
                if (BB.SimPhysics)
                {
                    if (!isFirstFrame)
                    {
                        InitPropertise();
                        averageAccelerationQueue.Clear();
                        ATQ.Clear();
                        isFirstFrame = true;
                    }
                    if (!StatMaster.isClient || StatMaster.isLocalSim)
                    {
                        FuncPosition();
                        FuncVelocity();
                        FuncDistance();
                        FuncOverload();
                        FuncAcceleration();
                        Functemp();
                    }
                }
                else
                {
                    if (!isFirstFrame)
                        return;
                    isFirstFrame = false;
                    InitPropertise();
                }
            }
            
        }

        public void InitPropertise()
        {
            Position = lastPosition = Vector3.zero;
            Velocity = lastVelocity = Vector3.zero;
            Distance = 0.0f;
            Overload = 0.0f;
            Acceleration = 0.0f;
            temp = 20f;

        }

        public void ChangedVelocityUnit()
        {
            Velocity = Vector3.zero;  
        }

        private void FuncPosition()
        {
            if (validBlock)
                Position = BB.transform.position;
            else
                Position = new Vector3(0.0f, 0.0f, 0.0f);
        }

        private void FuncVelocity()
        {
            if (validBlock)
            {
                Vector3 velocity = (Position - lastPosition) / Time.deltaTime;
                if (StatMaster.isClient)
                {
                    if (averageVelocityQueue.Count == queueSize)
                        averageVelocityQueue.Dequeue();
                    averageVelocityQueue.Enqueue(velocity);
                    Vector3 zero = Vector3.zero;
                    foreach (Vector3 averageVelocity in averageVelocityQueue)
                        zero += averageVelocity;
                    velocity = zero / (float)averageVelocityQueue.Count;
                }
                Velocity = velocity;
            }
            else
                Velocity = Vector3.zero;
        }

        private void FuncDistance()
        {
            if (validBlock)
            {
                Vector3 position = Position;
                Distance += (position - lastPosition).magnitude;
                lastPosition = position;
            }
            else
                Distance = 0.0f;
        }

        private void FuncOverload()
        {
            if (validBlock)
            {
                float acceleration = Acceleration;
         
                if (averageAccelerationQueue.Count == queueSize)
                {
                    double num = (double)averageAccelerationQueue.Dequeue();
                }
                averageAccelerationQueue.Enqueue(acceleration);
                Overload = averageAccelerationQueue.Average() / Physics.gravity.magnitude;
            }
            else
                Overload = 0.0f;
        }

        private void FuncAcceleration()
        {
            if (validBlock)
            {
                Vector3 velocity = Velocity;
                Vector3 zero = Vector3.zero;
                Vector3 lhs = (velocity - lastVelocity) / Time.fixedDeltaTime;
                Acceleration = ((double)Vector3.Dot(lhs, velocity) > 0.0 ? 1f : -1f) * lhs.magnitude;
                lastVelocity = velocity;
            }
            else
                Acceleration = 0.0f;
        }

        private void Functemp()
        {
            if (flipper >= 10)
            {
                if (ATQ.Count == TqueueSize)
                    ATQ.Dequeue();
                ATQ.Enqueue(Overload);
                float zero = 0;
                foreach (float aTemp in ATQ)
                    zero += aTemp;
                temp = zero / (float)ATQ.Count;
                temp = (temp * 1.5f) + 20;
                flipper = 0;
            }

            if (flipper == 0 || flipper == 5)
                if (temp >= 60 && temp <= 74)
                {
                    ModNetworking.SendToAll(Messages.SO.CreateMessage(BB, 1));
                    emitParams.startColor = new Color32(200, 200, 200, 10);
                    SSmoke.Emit(emitParams, 1);
                }
                else if (temp >= 75 && temp <= 89)
                {
                    ModNetworking.SendToAll(Messages.SO.CreateMessage(BB, 2));
                    emitParams.startColor = new Color32(255, 255, 255, 15);
                    emitParams.angularVelocity = 2;
                    SSmoke.Emit(emitParams, 1);
                }
                else if (temp >= 90)
                {
                    ModNetworking.SendToAll(Messages.SO.CreateMessage(BB, 3));
                    emitParams.startColor = new Color32(150, 150, 150, 20);
                    emitParams.angularVelocity = 4;
                    SSmoke.Emit(emitParams, 2);
                }
                else
                {
                    ModNetworking.SendToAll(Messages.SO.CreateMessage(BB, 0));
                }

            flipper++;
        }

        public void initSmokeFX()
        {
            GameObject Smokesystem = new GameObject("Smokesystem");
            SSmoke = Smokesystem.AddComponent<ParticleSystem>();
            Smokesystem.transform.SetParent(BB.transform);
            Smokesystem.transform.position = SSmoke.transform.position = BB.transform.position;

            Material particleMaterial = new Material(Shader.Find("Particles/Alpha Blended"));
            particleMaterial.mainTexture = Smoketex;

            var render = Smokesystem.GetComponent<ParticleSystemRenderer>();
            render.material = particleMaterial;
            render.renderMode = ParticleSystemRenderMode.Stretch;
            render.velocityScale = 0.03f;
            render.lengthScale = 1;
            var main = SSmoke;
            var em = SSmoke.emission;
            var lvol = SSmoke.limitVelocityOverLifetime;
            var col = SSmoke.collision;
            var colt = SSmoke.colorOverLifetime;
            var shp = SSmoke.shape;
            var sol = SSmoke.sizeOverLifetime;
            var rol = SSmoke.rotationOverLifetime;
            main.playOnAwake = false;
            main.loop = false;
            main.maxParticles = 1000;
            main.startLifetime = 3;
            main.startSize = 5;
            main.gravityModifier = 0.3f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startSpeed = 0;
            em.rate = 20;
            em.enabled = false;
           
            lvol.enabled = true;
            lvol.dampen = 1f;
            shp.shapeType = ParticleSystemShapeType.Sphere;
            shp.radius = 0.5f;
            sol.enabled = true;
            AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0.0f, 0.5f);
                curve.AddKey(1.0f, 5f);
            sol.size = new ParticleSystem.MinMaxCurve(1.0f, curve);
            rol.enabled = true;
            rol.y = new ParticleSystem.MinMaxCurve(1.0f, curve);
            colt.enabled = true;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color32(255, 255, 255, 255), 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 0.5f), new GradientAlphaKey(0.0f, 1.0f) });

            colt.color = grad;
        }

        public static void SOClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            int SmokeCounter = (int)m.GetData(1);
            BL.InternalObject.GetComponent<Wheelfix2_smoke>().SmokeOut(SmokeCounter);
        }

        public void SmokeOut(int SmokeCounter)
        {
            switch (SmokeCounter)
            {
                case 0: //idle
                    break;
                case 1: //light smoke
                    emitParams.startColor = new Color32(200, 200, 200, 10);
                    SSmoke.Emit(emitParams, 1);
                    break;
                case 2: //middle smoke
                    emitParams.startColor = new Color32(255, 255, 255, 15);
                    emitParams.angularVelocity = 2;
                    SSmoke.Emit(emitParams, 1);
                    break;
                case 3: //heavy smoke
                    emitParams.startColor = new Color32(150, 150, 150, 20);
                    emitParams.angularVelocity = 4;
                    SSmoke.Emit(emitParams, 2);
                    break;
            }
        }
    }
}
