using UnityEngine;

namespace BotFix
{
    public class FBfix : MonoBehaviour
    {
        private Rigidbody rigg;
        private ConfigurableJoint CJ;

        void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                /*
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                
                CJ = GetComponent<ConfigurableJoint>();
                CJ.breakForce = 20000f;
                CJ.breakTorque = 20000f;
                */
            }
        }
    }
}

