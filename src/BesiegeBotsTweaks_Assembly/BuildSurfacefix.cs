/*
 * BuildSurfacefix.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 * 
 * This class adds a few hidden settings into the Build Surface block mapper and vastly increases the break threshold.
 */

using Modding.Blocks;
using Modding.Common;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BuildSurface))]
    class BuildSurfaceFix : FrameDelayAction
    {
        protected override int FRAMECOUNT { get; } = 1;
        protected override void DelayedAction()
        {
            BuildSurface.SurfaceMaterialType BS = ((BuildSurface)thisBlock.InternalObject).currentType;

            BS.breakImpactThreshold = 500000f;
            BS.jointBaseBreakForce = 50000f;
            BS.jointBreakForceScaler = Mathf.Infinity;
            BS.jointBreakTorqueScaler = Mathf.Infinity;
        }
    }
}
