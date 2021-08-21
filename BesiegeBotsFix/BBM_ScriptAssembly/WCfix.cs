using UnityEngine;

namespace BotFix
{
    public class WCfix : MonoBehaviour
    {    
        private Rigidbody rigg;
        private ReduceBreakForceOnImpact RBF;

        void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                /*
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                */

                RBF = GetComponent<ReduceBreakForceOnImpact>();
                RBF.reduceMultiplier = 0f;
                RBF.firstBreakForce = Mathf.Infinity;
                RBF.impactThreshold = Mathf.Infinity;
/*
                if (!Mod.BetaMode)
                {
                    rigg.maxAngularVelocity = 5;
                }
                else
                {
                    rigg.maxAngularVelocity = 1000;
                }
                */
            }
        }
    }
}

