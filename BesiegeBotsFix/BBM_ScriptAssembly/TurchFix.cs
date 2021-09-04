using UnityEngine;
using Modding.Blocks;
using System.Collections.Generic;

namespace BotFix
{
    public class TurchFix : MonoBehaviour
    {
        private BlockBehaviour BB;
        private FireController FC;

        private MToggle Ftoggle;

        private bool FireEnabled = true;
        private int FrameCounter = 0;

        private void Awake()
        {
            //Get Stuff
            BB = GetComponent<BlockBehaviour>();
            FC = GetComponent<FireController>();

            //Mapper definition
            Ftoggle = BB.AddToggle("Enable Fire", "Enable Fire", FireEnabled);
            Ftoggle.Toggled += (bool value) => { FireEnabled = value; };

            //DisplayInMapper config
            Ftoggle.DisplayInMapper = true;
        }

        public void Update()
        {
            if(StatMaster.isClient && !StatMaster.isLocalSim || BB == null || FC == null) Object.Destroy(this);
            if (FrameCounter <= 100)
            {              
                FC.enabled = FireEnabled;
                FrameCounter++;
                BB.transform.FindChild("FireParticle").gameObject.SetActive(FireEnabled);
            }
        }
    }
}
