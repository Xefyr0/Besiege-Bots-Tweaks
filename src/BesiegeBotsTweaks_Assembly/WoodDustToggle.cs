/*
 * WoodDustToggle.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 */

using Modding;
using Modding.Blocks;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    [RequireComponent(typeof(SoundOnCollide))]
    class WoodDustToggle : FrameDelayAction
    {
        private BlockBehaviour BB;
        private SoundOnCollide SOC;

        private MToggle SmokeToggle;

        private bool dustActive = true;

        protected override int FRAMECOUNT { get; } = 1;
        protected override bool DESTROY_AT_END { get; } = false;
        public bool DustActive
        {
            get
            {
                return dustActive;
            }

            set
            {
                if (SOC.particles != null) SOC.particles.gameObject.SetActive(value);
                if (SOC.randSoundController != null) SOC.randSoundController.audioSource.enabled = value;
                dustActive = value;
                TD = mToggleDust.CreateMessage(BB, value);
                ModNetworking.SendToAll(TD);
            }
        }

        private static MessageType mToggleDust;
        Message TD;

        internal static void SetupNetworking()
        {
            mToggleDust = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[mToggleDust] += (System.Action<Message>)delegate(Message m)
            {
                Block target = (Block)m.GetData(0);
                if(target == null) return;
                else target.InternalObject.GetComponent<WoodDustToggle>().DustActive = (bool)m.GetData(1);
            };
        }
        new void Awake()
        {
            base.Awake();
            BB = GetComponent<BlockBehaviour>();
            SOC = GetComponent<SoundOnCollide>();

            SmokeToggle = BB.AddToggle("Enable Smoke", "Enable Smoke", dustActive);
            SmokeToggle.Toggled += (bool value) => DustActive = value;
        }

        protected override void DelayedAction()
        {
            if(!BB.SimPhysics) return;
            DustActive = true;
        }
    }
}

