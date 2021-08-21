using UnityEngine;

namespace BotFix
{
    public class Sbfix : MonoBehaviour
    {
        private ConfigurableJoint CJ;
        private Rigidbody rigg;

        void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponent<ConfigurableJoint>();
                rigg = GetComponent<Rigidbody>();

                CJ.breakForce = 60000;
                CJ.breakTorque = 60000;
                /*
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
                */
            }
        }
    }
}

