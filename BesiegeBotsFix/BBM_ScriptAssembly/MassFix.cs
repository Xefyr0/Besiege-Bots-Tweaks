/*
MassFix.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    /*
    Some blocks are too heavy or light for their appearance or usage in a Besiege Bot.
    this class remedies that.
    */
    public class MassFix
    {
        /*
        This method loops through each block's prefab and alters their masses.
        The method is static because the blocks' mass can be accessed directly from the prefab
        and still achieve the desired effect.
        */
        internal static void FixMasses()
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
                    case BlockType.Swivel:
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
                    case BlockType.ArmorPlateLarge:
                        RB.mass = 0.5f;
                        break;
                }
            }
        }
    }
}