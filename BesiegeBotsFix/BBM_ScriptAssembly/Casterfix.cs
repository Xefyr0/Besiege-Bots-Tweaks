using Modding;
using UnityEngine;

namespace BotFix
{
    public class Casterfix : MonoBehaviour
    {
        private ConfigurableJoint CJ;

        void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponent<ConfigurableJoint>();
                CJ.breakForce = 60000;
                CJ.breakTorque = 60000;
            }
        }
    }
}

