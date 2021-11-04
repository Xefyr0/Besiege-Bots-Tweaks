/*
DragTweaks.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    [RequireComponent(typeof(Rigidbody))]
    public class DragTweak : MonoBehaviour
    {
        private const byte FRAMECOUNT = 10;  //The number of frames this component waits before making changes
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
                if (Player.GetHost() == null || (Player.GetLocalPlayer().IsHost && block.Machine.Player == Player.GetLocalPlayer() ? true : !block.Machine.Player.InLocalSim)) StartCoroutine(TweakDrags());
                else Destroy(this);
            }
        }
        private System.Collections.IEnumerator TweakDrags()
        {
            //Wait FRAMECOUNT FixedUpdates into sim
            for (int i = 0; i < FRAMECOUNT; i++) yield return new WaitForFixedUpdate();

            //Gets the Rigidbody of each block, and if it isn't null then the drags & maxAngularVelocity are modified based on a switch statement of the block type.
            GameObject GO = block.GameObject;
            Rigidbody RB = GO.GetComponent<Rigidbody>();
            if (RB != null)
            {
                switch (block.InternalObject.Prefab.Type)
                {
                    case BlockType.CogMediumUnpowered:
                    case BlockType.GripPad:
                    case BlockType.Log:
                    case BlockType.StartingBlock:
                    case BlockType.BuildSurface:
                    case BlockType.Ballast:
                    case BlockType.Swivel:
                    case BlockType.BallJoint:
                    case BlockType.Spike:
                    case BlockType.MetalBlade:
                    case BlockType.Cannon:
                    case BlockType.Torch:
                    case BlockType.MetalBall:
                    case BlockType.ShrapnelCannon:
                    case BlockType.Decoupler:
                    case BlockType.DoubleWoodenBlock:
                    case BlockType.Hinge:
                    case BlockType.Plow:
                    case BlockType.HalfPipe:
                    case BlockType.SteeringBlock:
                    case BlockType.SteeringHinge:
                    case BlockType.Suspension:
                    case BlockType.WheelUnpowered:
                    case BlockType.LargeWheelUnpowered:
                    case BlockType.CogLargeUnpowered:
                    case BlockType.WoodenPole:
                    case BlockType.SingleWoodenBlock:
                    case BlockType.WoodenPanel:
                    case BlockType.Wheel:
                    case BlockType.LargeWheel:
                    case BlockType.WaterCannon:
                    case BlockType.ArmorPlateSmall:
                    case BlockType.ArmorPlateLarge:
                    case BlockType.ArmorPlateRound:
                        RB.drag = 0f;
                        RB.angularDrag = 0f;
                        RB.maxAngularVelocity = 100;
                        break;
                    case BlockType.FlyingBlock:
                    case BlockType.SpinningBlock:
                    case BlockType.CircularSaw:
                    case BlockType.Drill:
                        RB.drag = 0f;
                        RB.angularDrag = 0f;
                        break;
                    //Grabbers have nonzero drag because they're used so often in spinners.
                    case BlockType.Grabber:
                        RB.drag = 0.01f;
                        RB.angularDrag = 0f;
                        RB.maxAngularVelocity = 100;
                        break;
                }
            }

            //The component instance is destroyed after the necessary changes are made.
            Destroy(this);
        }   
    }
}