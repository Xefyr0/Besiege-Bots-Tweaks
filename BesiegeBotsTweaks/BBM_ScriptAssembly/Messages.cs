﻿using Modding;

namespace BotFix
{
    public static class Messages
    {
        public static MessageType A1;
        public static MessageType LP;
        public static MessageType LS;
        public static MessageType SPEED;
        public static MessageType DS;
        public static MessageType ELS;
        public static void SetupNetworking()
        {
            //Suspension sound messages
            A1 = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.A1] += Suspensionzcript.PlaySoundClient;

            LP = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.LP] += Suspensionzcript.PlayLoopSoundClient;

            LS = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[Messages.LS] += Suspensionzcript.StopLoopSoundClient;


            //Spinnersound messages
            SPEED = ModNetworking.CreateMessageType(DataType.Block, DataType.Single);
            ModNetworking.Callbacks[Messages.SPEED] += SpinnerSound.AngularVelocityClient;


            //Springcode messages
            DS = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[Messages.DS] += WinchFix.DestroyClient;


            //Log messages
            ELS = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[Messages.ELS] += Logfix.SmokeClient;

        }
    }
}