/*
Bluntener.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    /*
    In vanilla Besiege, Blocks with certain DamageTypes cut Rope & Winches.
    This is undesirable, as ropes are often used to power flippers in a manner similar to pneumatics.
    Thus, this class exists to modify the myDamageType of each of these blocks to Blunt,
    thereby rendering those blocks unable to cut ropes.
    */
    public class Bluntener
    {
        /*
        This method loops through each block's prefab and alters their myDamageType to Blunt.
        The method is static because the myDamageType member can be accessed directly from the prefab
        and still achieve the desired effect.
        */
        internal static void Blunten()
        {
            foreach(BlockType type in System.Enum.GetValues(typeof(BlockType)))
            {
                Modding.Blocks.BlockPrefabInfo BPI = Modding.Blocks.BlockPrefabInfo.GetOfficial(type);

                switch(type)
                {
                    case BlockType.MetalBlade:
                    case BlockType.CircularSaw:
                    case BlockType.Spike:
                    case BlockType.Flamethrower:
                    case BlockType.Wing:
                    case BlockType.Propeller:
                    case BlockType.SmallPropeller:
                    case BlockType.Drill:
                        BPI.InternalObject.myDamageType = DamageType.Blunt;
                        break;
                }
            }
        }
    }
}

