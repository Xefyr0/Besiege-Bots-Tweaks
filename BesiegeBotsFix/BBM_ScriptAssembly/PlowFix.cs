using UnityEngine;

namespace BotFix
{
    public class Plowfix : MonoBehaviour
    {
        private Rigidbody rigg;
        private ReduceBreakForceOnImpact RBF;

        void Awake()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (GetComponent<ReduceBreakForceOnImpact>())
                {
                    RBF = GetComponent<ReduceBreakForceOnImpact>();
                    RBF.reduceMultiplier = 0f;
                    RBF.firstBreakForce = Mathf.Infinity;
                    RBF.impactThreshold = Mathf.Infinity;
                }

                if (Mod.ModbotMode)
                    return;

                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
            }
        }
    }
}

