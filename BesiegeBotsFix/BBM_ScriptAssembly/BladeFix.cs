using Modding;
using UnityEngine;

namespace BotFix
{
    public class Bladefix : MonoBehaviour
    {
        private Collider[] colliders;
        private ConfigurableJoint CJ;
        private BlockDamageType BD;
        private Rigidbody rigg;
        private BlockBehaviour BB;

        void Start()
        {
            BB = GetComponent<BlockBehaviour>();
            BB.Prefab.myDamageType = DamageType.Blunt;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                /*
                CJ = GetComponent<ConfigurableJoint>();
                CJ.breakForce = 80000;
                CJ.breakTorque = 80000;

                //rigg = GetComponent<Rigidbody>();
                
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
                */
                //rigg.mass = 0.6f;
        
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

