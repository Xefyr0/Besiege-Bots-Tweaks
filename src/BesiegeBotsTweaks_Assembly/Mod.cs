/*
 * Mod.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 */

using BesiegeBotsTweaks;
using Modding;
using UnityEngine;

namespace BotFix
{
    public class Mod : ModEntryPoint
	{
        public static readonly float HIGHGRAV = -55f;
        private BBMUI UIObject;
        private BBinfo InfoObject;
        //public static bool UseModdedBlocks = true;
        //public static bool BetaMode = false;
        //public static bool ModbotMode = false;
        public override void OnLoad()
		{
            ModConsole.Log("[BesiegeBotsTweaks] Loading Mod");
            GameObject ModGameObject = GameObject.Find("ModControllerObject");	//If there isn't an existing mod GameObject,
			if(ModGameObject == null)
			{
				ModGameObject = new GameObject("ModControllerObject");	//Create a DontDestroyOnLoad Mod GameObject.
				UnityEngine.Object.DontDestroyOnLoad(ModGameObject);
			}
			//Initialize the BBMUI and BBinfo components and attach them to the Mod GameObject if it isn't already
			if(!(UIObject = ModGameObject.GetComponent<BBMUI>())) UIObject = ModGameObject.AddComponent<BBMUI>();
            if(!(InfoObject = ModGameObject.GetComponent<BBinfo>())) InfoObject = ModGameObject.AddComponent<BBinfo>();
			
			//If using SingleInstance, intialize the BBMUI component and make it the instance if it isn't already.
			#if useSingleInstance
			if(!SingleInstance<BBMUI>.hasInstance()) SingleInstance<BBMUI>.Initialize(UIObject);
            if(!SingleInstance<BBinfo>.hasInstance()) SingleInstance<BBinfo>.Initialize(InfoObject);
			#endif

            Modding.ModConsole.Log("[BBTweaks] Setting up Networking");
            FlameTFix.SetupNetworking();
            SpinnerSound.SetupNetworking();
            WoodDustToggle.SetupNetworking();
            WinchFix.SetupNetworking();
            Suspensionzcript.SetupNetworking();

            PrefabModder.ModAllPrefab();

            OptionsMaster.defaultSmoothness = 0f;
            Physics.gravity = new Vector3(Physics.gravity.x, HIGHGRAV, Physics.gravity.z);
            ModConsole.Log("[BBT] Set gravity to {0}", HIGHGRAV);

            OptionsMaster.BesiegeConfig.MorePrecisePhysics = false;
            //StatMaster.Rules.DisableFire = true;
        }
	}
}