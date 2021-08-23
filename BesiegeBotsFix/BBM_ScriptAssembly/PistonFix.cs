using UnityEngine;

namespace BotFix
{
    public class PistonFix : MonoBehaviour
    {
        public ConfigurableJoint myJoint;
  
        void Start()
        {/*
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            { 
                myJoint = GetComponent<ConfigurableJoint>();
                myJoint.breakForce = 35000;
                myJoint.breakTorque = 30000;
            }*/
        }
    }
}
