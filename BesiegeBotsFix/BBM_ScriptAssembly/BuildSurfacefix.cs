using UnityEngine;

namespace BotFix
{
    public class BuildSurfaceFix : MonoBehaviour
    {
        private Rigidbody rigg;
        private BuildSurface BS;
        private ConfigurableJoint[] CJ;
        private Collider[] colliders;
        private bool firstframe = true;

        void Awake()
        {
            BS = GetComponent<BuildSurface>();

            var CT = BS.currentType;
            CT.breakImpactThreshold = 20000f;
            CT.jointBaseBreakForce = 50000f;
            CT.jointBreakForceScaler = Mathf.Infinity;
            CT.jointBreakTorqueScaler = Mathf.Infinity;


            BuildSurface.AllowThicknessChange = true;
            BS.thickSlider.DisplayInMapper = true;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;

                CJ = GetComponents<ConfigurableJoint>();
                foreach (ConfigurableJoint joint in CJ)
                {
                    joint.breakForce = 40000;
                    joint.breakTorque = 40000;
                }

                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {

                    collider.material.dynamicFriction = 0.1f;
                    collider.material.staticFriction = 0.1f;
                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }
            }
        }

        void FixedUpdate()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BS.SimPhysics)
                {
                    if (firstframe)
                    {
                        BS.currentType.breakImpactThreshold = 500000f;
                    }

                    //Debug.Log(BS.currentType.breakImpactThreshold);
                }
            }
        }
    }
}
