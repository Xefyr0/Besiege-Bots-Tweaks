
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BotFix
{
    internal class BBinfo : MonoBehaviour
    {
        private ModKey nutkey = ModKeys.GetKey("nutnut");
        private Queue<float> averageAccelerationQueue = new Queue<float>();
        private Queue<Vector3> averageVelocityQueue = new Queue<Vector3>();
        private Queue<float> averageAngularVelocityQueue = new Queue<float>();
        private int queueSize = 60;
        // 0 = kmh, 1 = mph, 2 = ms, 3 = Mach
        public byte velocityUnit;
        public BlockBehaviour targetBlock;
        private bool validBlock;
        private Vector3 lastPosition;
        private Vector3 lastVelocity;
        private Quaternion lastAngle;
        private bool isFirstFrame;
        private string strBlock;
        public string yeet;
        

        public Vector3 Position { get; private set; }

        public Vector3 Velocity { get; private set; }

        public Quaternion Angle { get; private set; }

        public Vector3 AngularVelocity { get; private set; }

        public float angval { get; private set; }

        public float Distance { get; private set; }

        public float Overload { get; private set; }

        public float Acceleration { get; private set; }


        private void Awake()
        {
            isFirstFrame = false;
            velocityUnit = 2;
        }

        private void FixedUpdate()
        {   
            if (Game.IsSimulating)
            {
                if (!isFirstFrame)
                {
                    isFirstFrame = true;
                    InitPropertise();
                    averageAccelerationQueue.Clear();
                    averageAngularVelocityQueue.Clear();
                    LoadBlock();
                    yeet = targetBlock.ToString();

                }
                FuncPosition();
                FuncAngle();
                FuncVelocity();
                FuncAngVel();
                FuncDistance();
                FuncOverload();
                FuncAcceleration();
                BlockSelector();
               
            }
            else
            {
                if (!isFirstFrame)
                    return;
                isFirstFrame = false;
                InitPropertise();
            }
        }

        public void InitPropertise()
        {
            Position = lastPosition = Vector3.zero;
            Velocity = lastVelocity = Vector3.zero;
            Angle = lastAngle = new Quaternion(0,0,0,0);
            Distance = 0.0f;
            Overload = 0.0f;
            Acceleration = 0.0f;
            angval = 0.0f;
            yeet = null;
        }

        public void ChangedVelocityUnit()
        {
            if(velocityUnit < 3) velocityUnit ++;
            else velocityUnit = 0;
            Velocity = Vector3.zero;
            ConsoleController.ShowMessage(velocityUnit.ToString());
        }

        private void FuncPosition()
        {
            if (validBlock)
                Position = targetBlock.transform.position;
            else
                Position = new Vector3(0.0f, 0.0f, 0.0f);
        }

        private void FuncAngle()
        {
            if (validBlock)
                Angle = targetBlock.transform.rotation;         
        }

        private void FuncAngVel()
        {
            if (validBlock)
            {
                angval = Quaternion.Angle(Angle, lastAngle);

                if (StatMaster.isClient)
                {
                    if (averageAngularVelocityQueue.Count == queueSize)
                        averageAngularVelocityQueue.Dequeue();
                    averageAngularVelocityQueue.Enqueue(angval);
                    float zero = 0;
                    foreach (float averageAng in averageAngularVelocityQueue)
                        zero += averageAng;
                    angval = zero / (float)averageAngularVelocityQueue.Count;
                }

                angval = angval * 16.5f; //16.5
                lastAngle = Angle;
            } else
            {
                angval = 0;
            }
            
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
                Velocity = GetVelocity(velocity, velocityUnit);
            }
            else
                Velocity = Vector3.zero;
        }

        private Vector3 GetVelocity(Vector3 velocity, byte velocityUnit)
        {
            switch (velocityUnit)
            {
                case 0: //kph
                    velocity = Vector3.Scale(velocity, Vector3.one * 3.6f);
                    break;
                case 1: //mach
                    velocity = Vector3.Scale(velocity, Vector3.one / 340f);
                    break;
                case 3: //mph
                    velocity = Vector3.Scale(velocity, Vector3.one * 2.237f);
                    break;
            }
            return velocity;
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
                if (velocityUnit == 0)  //kmh
                    acceleration /= 3.6f;
                else if (velocityUnit == 1) //mph
                    acceleration /= 2.237f;
                else if (velocityUnit == 3) //mach
                    acceleration *= 340f;

                    if (averageAccelerationQueue.Count == queueSize)
                {
                    double num = (double)averageAccelerationQueue.Dequeue();
                }
                averageAccelerationQueue.Enqueue(acceleration);
                Overload = averageAccelerationQueue.Average() / Physics.gravity.magnitude;

                /*
                if (Overload > +Overloadmax)
                    Overloadmax = Overload;
                */
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

        private void LoadBlock()
        {
            try
            {
                if (StatMaster.isMP && StatMaster.isClient)
                {
                    foreach (PlayerData player in Playerlist.Players)
                    {
                        if (!player.isSpectator && (int)player.machine.PlayerID == (int)Machine.Active().PlayerID)
                            targetBlock = player.machine.FirstBlock;
                    }
                }
                else
                    targetBlock = Machine.Active().FirstBlock;
            }
            catch
            {
            }
            validBlock = !((Object)targetBlock == (Object)null);
        }

        private void BlockSelector()
        {
            RaycastHit hitInfo;

            if (nutkey != null && nutkey.IsPressed && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f))
            {
               
                GameObject index = hitInfo.transform.gameObject;
                if (index.GetComponent<BlockBehaviour>())
                    targetBlock = index.GetComponent<BlockBehaviour>();
                else
                    return;

                yeet = targetBlock.Prefab.name.ToString();
                //yeet = strBlock.Replace("UnityEngine", "");
                //yeet = yeet.Replace("GameObject", "");
            }
        }
    }
}
