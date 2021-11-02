/*
Mod.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/

using System;
using Modding;
using Modding.Blocks;
using UnityEngine;
using BesiegeBotsTweaks;

namespace BotFix
{
	public class Mod : ModEntryPoint
	{
        public static bool UseModdedBlocks = true;
        public static bool BetaMode = false;
        public static bool ModbotMode = false;
        public override void OnLoad()
		{
            GameObject gameObject = new GameObject("BBTUI");
            gameObject.AddComponent<BBMUI>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            Messages.SetupNetworking();

            //Events.OnBlockInit += new Action<Block>(AddFixScript);

            PrefabModder.ModAllPrefab();

            OptionsMaster.defaultSmoothness = 0f;
            Physics.gravity = new Vector3(Physics.gravity.x, -55f, Physics.gravity.z);
            OptionsMaster.BesiegeConfig.MorePrecisePhysics = false;
            //StatMaster.Rules.DisableFire = true;
        }
	}
}