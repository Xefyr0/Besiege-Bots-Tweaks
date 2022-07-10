/*
LogDustToggle.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/
using UnityEngine;
using Modding;
using Modding.Blocks;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    [RequireComponent(typeof(SoundOnCollide))]
    public class LogDustToggle : MonoBehaviour
    {
        private BlockBehaviour BB;
        private SoundOnCollide SOC;

        private MToggle SmokeToggle;

        private bool dustActive = true;

        private static MessageType mToggleDust;
        Message TD;

        internal static void SetupNetworking()
        {
            mToggleDust = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[mToggleDust] += (System.Action<Message>)delegate(Message m)
            {
                Block target = (Block)m.GetData(0);
                if(target == null) return;
                else target.InternalObject.GetComponent<LogDustToggle>().SetDust((bool)m.GetData(1));
            };
        }
        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            SOC = GetComponent<SoundOnCollide>();

            SmokeToggle = BB.AddToggle("Enable Smoke", "Enable Smoke", dustActive);
            SmokeToggle.Toggled += (bool value) => dustActive = value;
        }

        private void Start()
        {
            if(!BB.SimPhysics) return;
            TD = mToggleDust.CreateMessage(BB, dustActive);
            ModNetworking.SendToAll(TD);
            SetDust(dustActive);
        }

        private void SetDust(bool active)
        {
            SOC.particles.gameObject.SetActive(active);
        }
    }
}

