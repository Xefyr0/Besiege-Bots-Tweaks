/*
 * DragTweaks.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * This class finds the type of block it is attached to and may modify the air drag of that block
 * depending on the type.
 */

using UnityEngine;

namespace BesiegeBotsTweaks
{
    //Cannot have because parenting requires the removal of a rigidbody.
    //[RequireComponent(typeof(Rigidbody))]
    class DragTweak : FrameDelayAction
    {
        protected override int FRAMECOUNT { get; } = 10;
        protected override void DelayedAction()
        {
            //Gets the Rigidbody of each block, and if it isn't null then the drags & maxAngularVelocity are modified based on a switch statement of the block type.
            GameObject GO = thisBlock.GameObject;
            Rigidbody RB = GO.GetComponent<Rigidbody>();
            if (RB != null)
            {
                switch (thisBlock.InternalObject.Prefab.Type)
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
        }   
    }
}