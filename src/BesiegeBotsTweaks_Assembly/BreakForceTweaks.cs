/*
 * BreakForcetweaks.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * This class finds what type of block it's attached to and may modify the strengths of its Joints
 * FRAMECOUNT FixedUpdates after sim starts depending on the type of the block.
 * 
 * This does not include modifications created by blocks with "modes" such as Grabbers and Suspensions.
 */

using UnityEngine;

namespace BesiegeBotsTweaks
{
    //Not necessary because Joints are generated after the component is applied
    //[RequireComponent(typeof(Joint))]
    class BreakForceTweak : FrameDelayAction
    {
        protected override int FRAMECOUNT { get; } = 2;
        protected override void DelayedAction()
        {
            //Gets the joint array from each block, and if there is at least one joint then it modifies the joint strength based on a switch statement of the block type.
            Joint[] joints = thisBlock.GameObject.GetComponents<Joint>();
            StrengthenTreads ST = thisBlock.GameObject.GetComponent<StrengthenTreads>();

            if (joints.Length > 0)
            {
                switch (thisBlock.InternalObject.Prefab.Type)
                {
                    case BlockType.FlyingBlock:
                        joints[0].breakForce = 20000;
                        joints[0].breakTorque = 20000;
                        break;
                    case BlockType.Ballast:
                    case BlockType.Decoupler:
                    case BlockType.SteeringHinge:
                    case BlockType.Hinge:
                    case BlockType.BallJoint:
                    case BlockType.Swivel:
                        joints[0].breakForce = 30000;
                        joints[0].breakTorque = 30000;
                        //Hinges have a specialized component made to strengthen hinge-based treads.
                        //This changes the break threshold values of that component to strengthen treads further.
                        if (ST != null)
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
                        joints[0].breakForce = 60000;
                        joints[0].breakTorque = 60000;
                        break;
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
                        joints[0].breakForce = 100000;
                        joints[0].breakTorque = 100000;
                        break;
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
                        foreach (Joint joint in joints)
                        {
                            if (joint == null) continue;
                            joint.breakForce = 60000;
                            joint.breakTorque = 60000;
                        }
                        break;
                    case BlockType.DoubleWoodenBlock:
                        foreach (Joint joint in joints)
                        {
                            if (joint == null) continue;
                            joint.breakForce = 50000;
                            joint.breakTorque = 50000;
                        }
                        break;
                    case BlockType.WoodenPole:
                        foreach (Joint joint in joints)
                        {
                            if (joint == null) continue;
                            joint.breakForce = 40000;
                            joint.breakTorque = 40000;
                        }
                        break;
                }
            }
        }
    }
}