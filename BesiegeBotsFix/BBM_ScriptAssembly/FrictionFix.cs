#define DoyleNumbers
using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class FrictionFix
    {
        internal static void FixFriction()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
                GameObject GO = BPI.InternalObject.gameObject;
                Collider[] colliders = GO.GetComponentsInChildren<Collider>();

                #if DoyleNumbers
                foreach(Collider collider in colliders)
                {
                    switch(type)
                    {
                        case BlockType.MetalBlade:
                        case BlockType.Spike:
                        case BlockType.ArmorPlateLarge:
                        //case BlockType.ArmorPlateSmall:
                        case BlockType.ArmorPlateRound:
                        case BlockType.WoodenPanel:
                        case BlockType.BuildSurface:
                            collider.material.dynamicFriction = 0.1f;
                            collider.material.staticFriction = 0.1f;
                            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                            break;
                        case BlockType.Grabber: //Grabbers and spinblocks have half the low friction because of explosion risk
                        case BlockType.SpinningBlock:
                            collider.material.dynamicFriction = 0.05f;
                            collider.material.staticFriction = 0.05f;
                            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                            break;
                    }
                }
                #else
                #endif
            }
        }
    }
}