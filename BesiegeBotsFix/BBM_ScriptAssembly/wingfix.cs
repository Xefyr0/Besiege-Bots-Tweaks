using UnityEngine;

namespace BotFix
{
    public class WFix : MonoBehaviour
    {
        public ConfigurableJoint myJoint;
        public AxialDrag AD;

        private MToggle Wtoggle;

        private bool wingDragEnabled = false;

        void Awake()
        {
            AD = GetComponent<AxialDrag>();

            //Mapper definition
            Wtoggle = AD.AddToggle("Disable Drag", "Disable drag", wingDragEnabled);
            Wtoggle.Toggled += (bool value) => {
                wingDragEnabled = value;
                if (wingDragEnabled)
                {
                    AD.velocityCap = 0;
                }
                else
                {
                    AD.velocityCap = 30;
                }
            };

            //DisplayInMapper config
            Wtoggle.DisplayInMapper = true;
/*
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                myJoint = GetComponent<ConfigurableJoint>();
                myJoint.breakForce = 40000;
                myJoint.breakTorque = 40000;
            }
            */
        }
    }
}
