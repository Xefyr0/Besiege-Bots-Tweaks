using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BotFix
{
    [RequireComponent(typeof(BuildSurface))]
    public class BuildSurfaceFix : MonoBehaviour
    {
        private Rigidbody rigg;
        private BuildSurface BS;
        private ConfigurableJoint[] CJ;
        private Collider[] colliders;

        void Awake()
        {
            BS = GetComponent<BuildSurface>();
            Block block = Block.From(BS);

            var CT = BS.currentType;
            CT.breakImpactThreshold = 20000f;
            CT.jointBaseBreakForce = 50000f;
            CT.jointBreakForceScaler = Mathf.Infinity;
            CT.jointBreakTorqueScaler = Mathf.Infinity;

            BuildSurface.AllowThicknessChange = true;
            BS.thickSlider.DisplayInMapper = true;

            if(BS.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                //4. As client on the local instance if we're in local sim
                if (Player.GetHost() == null || (block.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !block.Machine.Player.InLocalSim)) StartCoroutine(Init());
                else Destroy(this);
            }
        }

        System.Collections.IEnumerator Init()
        {
            //Wait one frame into sim.
            yield return new WaitForFixedUpdate();
            BS.currentType.breakImpactThreshold = 500000f;
        }
    }
}
