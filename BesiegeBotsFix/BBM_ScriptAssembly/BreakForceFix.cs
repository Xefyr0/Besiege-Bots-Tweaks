/*
BreakForceFix.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    /*
    The very first and most common change BBM makes is to change the durability of certain blocks.
    This is done by changing the breakForce and breakTorque thresholds of Joints.
    Unfortunately, since Joints are formed as late as a few frames after sim is started,
    an Update method must be used for a few frames to wait until the Joints are formed before making the necessary changes,
    similar to Dragfix.cs.
    */
    public class BreakForceFix : MonoBehaviour
    {
        private const byte FRAMECOUNT = 3;  //The number of frames this component waits before making changes
        private byte frameCounter = 0;  //frameCounter Variable to keep track of how many frames have elapsed
        private BlockBehaviour BB;

        /*
        A static, parameterless void method that is used as the "entry point" of this class.
        This method loops through each block prefab and attaches an instance of this component to that prefab.
        */
        internal static void FixBreakForces()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
                GameObject GO = BPI.InternalObject.gameObject;
                GO.AddComponent<BreakForceFix>();
            }
        }
        private void Awake()
        {
            //BB is initialized in Awake instead of Update because otherwise GetComponent would be called multiple times.
            BB = GetComponent<BlockBehaviour>();
            if(BB == null) Object.Destroy(this);
        }
        private void Update()
        {
            //Wait until sim starts, then starts counting frames until FRAMECOUNT frames are reached
            if(!BB.SimPhysics) return;
            frameCounter++;
            if(frameCounter < FRAMECOUNT) return;

            //If the block isn't actually part of a simulation (i.e. on a client computer in multiverse with local sim turned off) then the component instance is destroyed since it won't do anything either way
            if(StatMaster.isClient && !StatMaster.isLocalSim) Object.Destroy(this);
            
            //Gets the joint array from each block, and if there is at least one joint then it modifies the joint strength based on a switch statement of the block type.
            Joint[] joints = GetComponents<Joint>();
            if(joints.Length > 0)
            {
                switch(BB.Prefab.Type)
                {
                    case BlockType.FlyingBlock:
                        joints[0].breakForce = 20000;
                        joints[0].breakTorque = 20000;
                        break;
                    case BlockType.Ballast:
                    case BlockType.Decoupler:
                    case BlockType.SteeringHinge:
                        joints[0].breakForce = 30000;
                        joints[0].breakTorque = 30000;
                        break;
                    case BlockType.Hinge:
                    case BlockType.BallJoint:
                    case BlockType.Swivel:
                        joints[0].breakForce = 30000;
                        joints[0].breakTorque = 30000;
                        //Hinges have a specialized component made to strengthen hinge-based treads.
                        //This changes the break threshold values of that component to strengthen treads further.
                        StrengthenTreads ST = null;
                        if(ST = GetComponent<StrengthenTreads>())
                        {
                            ST.breakForce = 50000;
                            ST.breakTorque = 50000;
                        }
                        break;
                    case BlockType.Suspension:
                    case BlockType.Piston:
                        joints[0].breakForce = 35000;
                        joints[0].breakTorque = 35000;
                        break;
                    case BlockType.Flamethrower:
                    case BlockType.Wing:
                    case BlockType.WingPanel:
                    case BlockType.BuildSurface:
                        joints[0].breakForce = 40000;
                        joints[0].breakTorque = 40000;
                        break;
                    case BlockType.SteeringBlock:
                    case BlockType.SmallWheel:
                    case BlockType.Wheel:
                    case BlockType.WheelUnpowered:
                    case BlockType.LargeWheel:
                    case BlockType.LargeWheelUnpowered:
                        joints[0].breakForce = 60000;
                        joints[0].breakTorque = 60000;
                        break;
                    case BlockType.MetalBlade:
                    case BlockType.Spike:
                        joints[0].breakForce = 80000;
                        joints[0].breakTorque = 80000;
                        break;
                    case BlockType.SpinningBlock:
                    case BlockType.CogMediumPowered:
                    case BlockType.CircularSaw:
                        joints[0].breakForce = 90000;
                        joints[0].breakTorque = 90000;
                        break;
                    case BlockType.WoodenPanel:
                        joints[0].breakForce = Mathf.Infinity;
                        joints[0].breakTorque = Mathf.Infinity;
                        break;
                    //The following blocks have 2 joints, all of which are modified. Hence the foreach.
                    case BlockType.SingleWoodenBlock:
                        foreach(Joint joint in joints)
                        {
                            if(joint == null) continue;
                            joint.breakForce = 60000;
                            joint.breakTorque = 60000;
                        }
                        break;
                    case BlockType.DoubleWoodenBlock:
                        foreach(Joint joint in joints)
                        {
                            if(joint == null) continue;
                            joint.breakForce = 50000;
                            joint.breakTorque = 50000;
                        }
                        break;
                    case BlockType.WoodenPole:
                        foreach(Joint joint in joints)
                        {
                            if(joint == null) continue;
                            joint.breakForce = 40000;
                            joint.breakTorque = 40000;
                        }
                        break;
                }
            }
            Object.Destroy(this);   //The component instance is destroyed after the necessary changes are made.
        }
    }
}