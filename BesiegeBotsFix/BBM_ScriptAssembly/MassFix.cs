using UnityEngine;

namespace BesiegeBotsMod
{
    public class MassFix
    {
        internal static void fixMasses()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
                GameObject GO = BPI.InternalObject.gameObject;
                Rigidbody RB = GO.GetComponent<Rigidbody>();

                switch(type)
                {
                    case BlockType.MetalBlade:
                    case BlockType.Spike:
                        RB.mass = 0.6f;
                        break;
                    case BlockType.Grabber:
                        RB.mass = 0.7f;
                        break;
                    case BlockType.SteeringHinge:
                        RB.mass = 0.4f;
                        break;
                    case BlockType.WoodenPanel:
                        RB.mass = 0.5f;
                        break;
                }
            }
        }
    }
}