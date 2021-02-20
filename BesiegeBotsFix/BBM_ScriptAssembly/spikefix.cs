using UnityEngine;

namespace BotFix
{
    public class Spikefix : MonoBehaviour
    {
        private Collider[] colliders;
        private ConfigurableJoint CJ;
        private BlockBehaviour BB;

        void Start()
        {
            BB = GetComponent<BlockBehaviour>();
            BB.Prefab.myDamageType = DamageType.Blunt;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponent<ConfigurableJoint>();
                CJ.breakForce = 80000;
                CJ.breakTorque = 80000;

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

