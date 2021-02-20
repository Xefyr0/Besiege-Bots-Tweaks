using UnityEngine;

namespace BotFix
{
    public class AngVelFix : MonoBehaviour
    {
        private Rigidbody rigg;

        void Awake()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;

                rigg.maxAngularVelocity = 1000;
            }
        }
    }
}

