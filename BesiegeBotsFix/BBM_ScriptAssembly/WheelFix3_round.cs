using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace BotFix
{
    public class Wheelfix3_round : MonoBehaviour
    {
        public BlockBehaviour BB;
        public CogMotorControllerHinge CCH;
        private Rigidbody rigg;
        private HingeJoint CJ;
        static GameObject WheelColliderOrgin;
        private Collider[] Colliders;
        private MeshFilter mFilter;
        private MeshRenderer mRenderer;
        private MeshCollider mCollider;
        public GameObject WheelCollider;

        private MSlider GS;
        private MMenu PCMenu;
        public MToggle Roundwheelz;
        private MSlider Lerpomode;

        private int PCselect = 3;
        private int ID = 0;
        private float Friction = 0.8f;
        private float Lerpo = 0f;
           
        public bool MakeRound = false;       
        private bool isFirstFrame = true;
        private bool Collider = false;
        private bool ShowCollider = false;
        private bool IsPowered = false;

        public PhysicMaterialCombine PC = PhysicMaterialCombine.Maximum;

        internal static List<string> PCmenul = new List<string>()
        {
            "Average",
            "Multiply",
            "Minimum",
            "Maximum",
        };
     
        private void Awake()
        {
            //Get stuff
            BB = GetComponent<BlockBehaviour>();

            ID = BB.BlockID;
            Friction = PSaF.GetPositionScaleAndFriction(ID).Friction;

            if (ID == (int)BlockType.Wheel || ID == (int)BlockType.LargeWheel)
            {
                IsPowered = true;
                CCH = GetComponent<CogMotorControllerHinge>();
                Lerpomode = BB.AddSlider("Spin up time", "Lerp", Lerpo, 0f, 10f);
                //Lerpomode.ValueChanged += (float value) => { Lerpo = value; CCH.speedLerpSmooth = value; };
                Lerpomode.ValueChanged += (float value) => {
                    Lerpo = -0.1f + (26.24517f + 0.4575599f) / Mathf.Pow(1f + (value / 2.265289f), 3.291725f);
                    CCH.speedLerpSmooth = Lerpo;
                    //Debug.Log(Lerpo);
                };
                Lerpomode.DisplayInMapper = true;
            }

            if (WheelColliderOrgin == null)
            {
                StartCoroutine(ReadWheelMesh());
            }

            //Mapper definition
            Roundwheelz = BB.AddToggle("ROUNDWHEELZ!", "ROUNDWHEELZ!", MakeRound);
            Roundwheelz.Toggled += (bool value) => { MakeRound = Collider = value; };

            GS = BB.AddSlider("Friction", "Friction", Friction, 0.1f, 10f);
            GS.ValueChanged += (float value) => { Friction = value; };

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

            //DisplayInMapper config
            Roundwheelz.DisplayInMapper = true;
            GS.DisplayInMapper = true;
            PCMenu.DisplayInMapper = true;

            //Physics stuff
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                
                CJ = GetComponent<HingeJoint>();
                CJ.breakForce = 60000;
                CJ.breakTorque = 60000;
               
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
            }    
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.isSimulating)
                {
                    if (isFirstFrame)
                    {
                        isFirstFrame = false;
                        if (!rigg)
                            return;

                        Colliders = GetComponentsInChildren<Collider>();
                        if (MakeRound)
                        {
                            if (WheelCollider != null) return;

                            foreach (Collider c in Colliders) { if (c.name == "CubeColliders") c.isTrigger = true; }

                            WheelCollider = (GameObject)Instantiate(WheelColliderOrgin, transform.position, transform.rotation, transform);
                            WheelCollider.SetActive(true);
                            WheelCollider.name = "Wheel Collider";
                            WheelCollider.transform.SetParent(transform);

                            mFilter = WheelCollider.AddComponent<MeshFilter>();
                            mFilter.sharedMesh = WheelCollider.GetComponent<MeshCollider>().sharedMesh;

                            mCollider = WheelCollider.GetComponent<MeshCollider>();
                            mCollider.convex = true;
                            mCollider.material.staticFriction = Friction;
                            mCollider.material.dynamicFriction = Friction;
                            mCollider.material.frictionCombine = PC;

                            if (ShowCollider)
                            {
                                mRenderer = WheelCollider.AddComponent<MeshRenderer>();
                                mRenderer.material.color = Color.red;
                            }

                            PSaF pas = PSaF.GetPositionScaleAndFriction(ID);

                            WheelCollider.transform.parent = transform;
                            WheelCollider.transform.rotation = transform.rotation;
                            WheelCollider.transform.position = transform.TransformPoint(transform.InverseTransformPoint(transform.position) + pas.Position);
                            WheelCollider.transform.localScale = pas.Scale;
                        }
                        else
                        {
                            foreach (Collider c in Colliders)
                            {
                                if (c.name == "CubeColliders")
                                {
                                    c.enabled = true;
                                    BoxCollider bc = c.GetComponent<BoxCollider>();
                                    bc.material.dynamicFriction = Friction;
                                    bc.material.staticFriction = Friction;
                                    bc.material.frictionCombine = PC;
                                }
                            }
                            Destroy(WheelCollider);
                        }
                    }
                }
            }
        }

        private struct PSaF
        {
            public Vector3 Position;
            public Vector3 Scale;
            public float Friction;
            public static PSaF one = new PSaF { Position = Vector3.one, Scale = Vector3.one, Friction = 1 };

            public static PSaF GetPositionScaleAndFriction(int id)
            {
                PSaF psaf = new PSaF();

                if (id == (int)BlockType.Wheel)
                {
                    psaf.Position = new Vector3(0, 0, 0.175f);
                    psaf.Scale = new Vector3(0.98f, 0.98f, 1.75f);
                    psaf.Friction = 0.8f;
                    return psaf;
                }
                if (id == (int)BlockType.LargeWheel)
                {
                    psaf.Position = new Vector3(0, 0, 0.45f);
                    psaf.Scale = new Vector3(1.38f, 1.38f, 3.75f);
                    psaf.Friction = 0.8f;
                    return psaf;
                }
                if (id == (int)BlockType.WheelUnpowered)
                {
                    psaf.Position = new Vector3(0, 0, 0.175f);
                    psaf.Scale = new Vector3(0.98f, 0.98f, 1.75f);
                    psaf.Friction = 0.8f;
                    return psaf;
                }
                if (id == (int)BlockType.LargeWheelUnpowered)
                {
                    psaf.Position = new Vector3(0, 0, 0.45f);
                    psaf.Scale = new Vector3(1.38f, 1.38f, 1.75f);
                    psaf.Friction = 0.8f;
                    return psaf;
                }
                return PSaF.one;
            }
        }

        static IEnumerator ReadWheelMesh()
        {
            //Debug.Log("Readmesh!");
            WheelColliderOrgin = new GameObject("Wheel Collider Orgin");
            UnityEngine.Object.DontDestroyOnLoad(WheelColliderOrgin);
            //WheelColliderOrgin.transform.SetParent(Mod.gameObject.transform);
            
            if (!WheelColliderOrgin.GetComponent<MeshCollider>())
            {
                ModMesh modMesh = ModResource.CreateMeshResource("Wheel Mesh", "Resources" + @"/" + "Wheel.obj");
                MeshCollider meshCollider = WheelColliderOrgin.AddComponent<MeshCollider>();

                yield return new WaitUntil(() => modMesh.Available);

                meshCollider.sharedMesh = ModResource.GetMesh("Wheel Mesh");
                meshCollider.convex = true;
                WheelColliderOrgin.SetActive(false);
            }
        }
    }
}


