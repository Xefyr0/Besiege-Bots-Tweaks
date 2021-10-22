using System;
using Modding;
using Modding.Blocks;
using UnityEngine;

namespace BotFix
{
	public class Mod : ModEntryPoint
	{
        public static bool UseModdedBlocks = true;
        public static bool BetaMode = false;
        public static bool ModbotMode = false;
        public GameObject gameObject;

        public override void OnLoad()
		{
            gameObject = new GameObject("BBTUI");
            gameObject.AddComponent<BBMUI>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            SetupNetworking();

            //Events.OnBlockInit += new Action<Block>(AddFixScript);

            PrefabModder.ModAllPrefab();

            OptionsMaster.defaultSmoothness = 0f;
            Physics.gravity = new Vector3(Physics.gravity.x, -55f, Physics.gravity.z);
            OptionsMaster.BesiegeConfig.MorePrecisePhysics = false;
            //StatMaster.Rules.DisableFire = true;
        }

        public void SetupNetworking()
        {
            //Suspension sound messages
            Messages.A1 = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.A1] += Suspensionzcript.PlaySoundClient;

            Messages.LP = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.LP] += Suspensionzcript.PlayLoopSoundClient;

            Messages.LS = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[Messages.LS] += Suspensionzcript.StopLoopSoundClient;


            //Flame thrower messages
            Messages.SF = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.SF] += FlameTFix.StopFireClient;

            Messages.LF = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.LF] += FlameTFix.LoadFireClient;

            Messages.PFS = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.PFS] += FlameTFix.PlayFireSoundClient;

            Messages.SFS = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.SFS] += FlameTFix.StopFireSoundClient;


            //Wheelfix messages
            //Messages.SO = ModNetworking.CreateMessageType(DataType.Block, DataType.Integer);
            //ModNetworking.Callbacks[Messages.SO] += Wheelfix2_smoke.SOClient;


            //Spinnersound messages
            Messages.SPEED = ModNetworking.CreateMessageType(DataType.Block, DataType.Single);
            ModNetworking.Callbacks[Messages.SPEED] += SpinnerSound.AngularVelocityClient;


            //Springcode messages
            Messages.DS = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.DS] += WinchFix.DestroyClient;


            //Log messages
            Messages.ELS = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[Messages.ELS] += Logfix.SmokeClient;

        }
	}
}