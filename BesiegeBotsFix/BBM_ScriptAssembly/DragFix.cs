/*
DragFix.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    /*
    Before the existence of BBMod, Spinners and other high-speed weapons had low speed limits
    and needed many braces to achieve passable speeds.
    Thus, Doyle decreased the drag of blocks' Rigidbodies and increased their maxAngularVelocitys.
    This class does the same thing, but consolidated into one location. It functions similarly to BreakForceFix.cs,
    because for some reason blocks' drag are also modified a few frames into the simulation.
    */
    public class DragFix : MonoBehaviour
    {
        private const byte FRAMECOUNT = 3;  //The number of frames this component waits before making changes
        private byte frameCounter = 0;  //frameCounter Variable to keep track of how many frames have elapsed
        private BlockBehaviour BB;
        
        /*
        A static, parameterless void method that is used as the "entry point" of this class.
        This method loops through each block prefab and attaches an instance of this component to that prefab.
        */
        internal static void FixDrags()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
                GameObject GO = BPI.InternalObject.gameObject;
                //Rigidbody RB = GO.GetComponent<Rigidbody>();
                GO.AddComponent<DragFix>();
            }
        }
        private void Awake()
        {
            //BB is initialized in Awake instead of Update because otherwise GetComponent would be called multiple times.
            BB = GetComponent<BlockBehaviour>();
            if(BB == null) Object.Destroy(this);
        }
        void Update()
        {
            //Wait until sim starts, then starts counting frames until FRAMECOUNT frames are reached
            if(!BB.SimPhysics) return;
            frameCounter++;
            if(frameCounter < FRAMECOUNT) return;

            //If the block isn't actually part of a simulation (i.e. on a client computer in multiverse with local sim turned off) then the component instance is destroyed since it won't do anything either way
            if(StatMaster.isClient && !StatMaster.isLocalSim) Object.Destroy(this);
            
            //Gets the Rigidbody of each block, and if it isn't null then modifiy the drags & maxAngularVelocity based on a switch statement of the block type.
            Rigidbody RB = GetComponent<Rigidbody>();
            if(RB != null)
            {
                switch(BB.Prefab.Type)
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
            Object.Destroy(this);   //The component instance is destroyed after the necessary changes are made.
        }
    }
}