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
