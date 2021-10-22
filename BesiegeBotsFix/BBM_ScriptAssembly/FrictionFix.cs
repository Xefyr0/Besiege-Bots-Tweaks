/*
FrictionFix.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    /*
    In vanilla Besiege, Plows are the only viable wedging component, and there isn't really anything useful as a skidplate.
    Additionally, the high friction of blocks with an invincible connection render them more vulnerable to explosions.
    This class exists to modify the friction of some blocks, solving the above problems. It does so in a manner similar to Bluntener.cs.
    */
    public class FrictionFix
    {
        /*
        This method loops through each block's prefab and alters each of their colliders' friction.
        The method is static because the blocks' colliders and their members can be accessed directly from the prefab
        and still achieve the desired effect.
        */
        internal static void FixFriction()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);
                GameObject GO = BPI.InternalObject.gameObject;
                Collider[] colliders = GO.GetComponentsInChildren<Collider>();

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
                        case BlockType.Grabber: //Grabbers and spinblocks have even lower friction because of their inherent explosion risk
                        case BlockType.SpinningBlock:
                            collider.material.dynamicFriction = 0.05f;
                            collider.material.staticFriction = 0.05f;
                            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                            break;
                    }
                }
            }
        }
    }
}