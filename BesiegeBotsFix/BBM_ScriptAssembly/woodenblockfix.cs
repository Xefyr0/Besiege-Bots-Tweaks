using UnityEngine;

namespace BotFix
{
    public class WoodBlockFix : MonoBehaviour
    {
        public ConfigurableJoint[] CJ;
        private Rigidbody rigg;

        void Start()
        { 
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {/*
                CJ = GetComponents<ConfigurableJoint>();
                foreach (ConfigurableJoint joint in CJ)
                {
                    joint.breakForce = 60000;
                    joint.breakTorque = 60000;
    
                }
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100; 
                */
            }
        }
    }
}
