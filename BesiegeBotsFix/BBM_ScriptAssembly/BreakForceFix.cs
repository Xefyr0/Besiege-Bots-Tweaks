using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class BreakForceFix : MonoBehaviour
    {
        byte frameCount = 0;
        BlockBehaviour BB;
        internal static void FixBreakForces()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
                GameObject GO = BPI.InternalObject.gameObject;
                //Joint[] joints = GO.GetComponents<Joint>();
                GO.AddComponent<BreakForceFix>();
            }
        }
        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
        }
        void Update()
        {
            if(!BB.SimPhysics) return;
            if(frameCount < 5)
            {
                Joint[] joints = GetComponents<Joint>();
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
                    case BlockType.SingleWoodenBlock:
                        foreach(Joint joint in joints)
                        {
                            joint.breakForce = 60000;
                            joint.breakTorque = 60000;
                        }
                        break;
                    case BlockType.DoubleWoodenBlock:
                        foreach(Joint joint in joints)
                        {
                            joint.breakForce = 50000;
                            joint.breakTorque = 50000;
                        }
                        break;
                    case BlockType.WoodenPole:
                        foreach(Joint joint in joints)
                        {
                            joint.breakForce = 40000;
                            joint.breakTorque = 40000;
                        }
                        break;
                }
                frameCount++;
            }
            else Object.Destroy(this);
        }
    }
}