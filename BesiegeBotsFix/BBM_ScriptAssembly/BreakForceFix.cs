/*
BreakForceFix.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    public class BreakForceFix : MonoBehaviour
    {
        private const byte FRAMECOUNT = 2;  //The number of frames this component waits before making changes
        //The entry point into this simple class is when it's attached to a GameObject. Awake is generally called once on blocks, when they're created in build mode.
        private Block block;
        private void Awake()
        {
            block = Block.From(base.gameObject);
            
            if (block.InternalObject.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                if (Player.GetHost() == null || (Player.GetLocalPlayer().IsHost && block.Machine.Player == Player.GetLocalPlayer() ? true : !block.Machine.Player.InLocalSim)) StartCoroutine(TweakBreakForces());
                else Destroy(this);
            }
        }
        private System.Collections.IEnumerator TweakBreakForces()
        {
            //Wait FRAMECOUNT FixedUpdates into sim
            for (int i = 0; i < FRAMECOUNT; i++) yield return new WaitForFixedUpdate();

            //Gets the joint array from each block, and if there is at least one joint then it modifies the joint strength based on a switch statement of the block type.
            BlockBehaviour BB = block.InternalObject;
            Joint[] joints = BB.GetComponents<Joint>();
            StrengthenTreads ST = BB.GetComponent<StrengthenTreads>();

            if (joints.Length > 0)
            {
                switch (BB.Prefab.Type)
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

            //The component instance is destroyed after the necessary changes are made.
            Destroy(this);
        }
    }
}