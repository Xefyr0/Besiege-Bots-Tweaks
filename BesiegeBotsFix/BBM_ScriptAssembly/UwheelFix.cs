using UnityEngine;

namespace BotFix
{
    public class Uwheelfix : MonoBehaviour
    {
        private Rigidbody rigg;

        void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
              
            }
        }
    }
}

