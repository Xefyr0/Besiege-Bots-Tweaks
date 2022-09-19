/*
BuildSurfacefix.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr

This class adds a few hidden settings into the Build Surface block mapper and vastly increases the break threshold.
*/

using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BotFix
{
    [RequireComponent(typeof(BuildSurface))]
    public class BuildSurfaceFix : MonoBehaviour
    {
        private BuildSurface BS;

        void Awake()
        {
            //Basic gets
            BS = GetComponent<BuildSurface>();
            Block block = Block.From(BS);

            /*
            var CT = BS.currentType;
            CT.breakImpactThreshold = 20000f;
            CT.jointBaseBreakForce = 50000f;
            CT.jointBreakForceScaler = Mathf.Infinity;
            CT.jointBreakTorqueScaler = Mathf.Infinity;
            */

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
