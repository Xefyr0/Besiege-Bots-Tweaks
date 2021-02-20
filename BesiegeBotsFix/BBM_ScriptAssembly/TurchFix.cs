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

        private bool Fired = true;
        private int FrameCounter = 0;

        private void Awake()
        {
            //Get Stuff
            BB = GetComponent<BlockBehaviour>();
            FC = GetComponent<FireController>();

            //Mapper definition
            Ftoggle = BB.AddToggle("Enable Fire", "Enable Fire", Fired);
            Ftoggle.Toggled += (bool value) => { Fired = value; };

            //DisplayInMapper config
            Ftoggle.DisplayInMapper = true;
        }

        public void Update()
        {
            if (FrameCounter <= 100)
            {              
                FC.enabled = Fired;
                FrameCounter++;
                GameObject FP = BB.transform.FindChild("FireParticle").gameObject;
                FP.SetActive(Fired);
            }
        }
    }
}
