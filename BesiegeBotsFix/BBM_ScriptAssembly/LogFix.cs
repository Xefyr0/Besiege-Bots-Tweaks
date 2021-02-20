using UnityEngine;
using Modding;
using Modding.Blocks;

namespace BotFix
{
    public class Logfix : MonoBehaviour
    {
        private BlockBehaviour BB;
        public ConfigurableJoint[] CJ;
        private Rigidbody rigg;
        private SoundOnCollide SOC;

        private MToggle SmokeToggle;

        private bool smokey = true;
        private bool firstframe = true;


        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            SOC = GetComponent<SoundOnCollide>();

            SmokeToggle = BB.AddToggle("Enable Smoke", "Enable Smoke", smokey);
            SmokeToggle.Toggled += (bool value) => { smokey = value;};

            SmokeToggle.DisplayInMapper = true;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponents<ConfigurableJoint>();
                foreach (ConfigurableJoint joint in CJ)
                {
                    joint.breakForce = 60000;
                    joint.breakTorque = 60000;

                }
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 1000;
            }
        }

        private void Update()
        {
            if (BB.SimPhysics)
            {
                if (!StatMaster.isClient || StatMaster.isLocalSim)
                {
                    if (firstframe)
                    {
                        ToggleSmoke();
                        firstframe = false;
                    }
                }
            }
        }

        public void ToggleSmoke()
        {
            SOC.particles.gameObject.SetActive(smokey);

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB != null)
                {
                    ModNetworking.SendToAll(Messages.ELS.CreateMessage(BB, smokey));
                }
            }
        }

        public static void SmokeClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            //Debug.Log(BL);
            bool tog = (bool)m.GetData(1);
            //Debug.Log(tog);
            BL.InternalObject.GetComponent<Logfix>().ToggleSmoke();
        }
    }
}

