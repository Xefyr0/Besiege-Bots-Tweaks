#define DoyleNumbers
using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class Bluntener
    {
        internal static void Blunten()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);

                #if DoyleNumbers
                switch(type)
                {
                    case BlockType.MetalBlade:
                    case BlockType.CircularSaw:
                    case BlockType.Spike:
                    case BlockType.Wing:
                    case BlockType.Propeller:
                    case BlockType.SmallPropeller:
                    case BlockType.Drill:
                        BPI.InternalObject.myDamageType = DamageType.Blunt;
                        break;
                }
                #endif
            }
        }
    }
}

