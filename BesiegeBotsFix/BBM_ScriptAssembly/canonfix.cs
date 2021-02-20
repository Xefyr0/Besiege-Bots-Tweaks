using UnityEngine;

namespace BotFix
{
    public class CanonFix : MonoBehaviour
    {
        private CanonBlock CB;
        private Rigidbody rigg;
        private ReduceBreakForceOnImpact RBF;

        void Awake()
        {
            CB = GetComponent<CanonBlock>();
            if (CB != null)
            {
                CB.randomDelay = 0f;
            }

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (GetComponent<ReduceBreakForceOnImpact>())
                {
                    RBF = GetComponent<ReduceBreakForceOnImpact>();
                    RBF.reduceMultiplier = 0f;
                    RBF.firstBreakForce = Mathf.Infinity;
                    RBF.impactThreshold = Mathf.Infinity;
                }

                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
            }
        }
    }
}

