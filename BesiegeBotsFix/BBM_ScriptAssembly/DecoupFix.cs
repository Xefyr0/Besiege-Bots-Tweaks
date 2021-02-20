using UnityEngine;

namespace BotFix
{
    public class DecoupFix : MonoBehaviour
    {
        public ConfigurableJoint myJoint;
        private Rigidbody rigg;

        void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            { 
                myJoint = GetComponent<ConfigurableJoint>();
                myJoint.breakForce = 30000;
                myJoint.breakTorque = 30000;

                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
    
            }
        }
    }
}
