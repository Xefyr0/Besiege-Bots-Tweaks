using UnityEngine;

namespace BotFix
{
    public class Platefix : MonoBehaviour
    {
        private Collider[] colliders;
        private Rigidbody rigg;
        private ReduceBreakForceOnImpact RBF;

        void Start()
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
/*
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 200;
  */
                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.material.dynamicFriction = 0.1f;
                    collider.material.staticFriction = 0.1f;
                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }
            }
        }
    }
}

