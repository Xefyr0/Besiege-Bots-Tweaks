/*
 * AntiPhysXExplosion.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * This class is for use on Grabbers to drastically reduce the chance of physics explosions with their "grabby" connection.
 */

using Modding.Blocks;
using Modding.Common;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    //Not necessary because Joints are generated after the component is applied
    //[RequireComponent(typeof(Joint))]
    class AntiPhysXExplosion : FrameDelayAction
    {
        protected override int FRAMECOUNT { get; } = 15;
        protected override void DelayedAction()
        {
            //joinTrigger is the component that manages the grabber's grabby joint/trigger combo.
            JoinOnTriggerBlock joinTrigger = gameObject.GetComponentInChildren<JoinOnTriggerBlock>();

            //If the grabber has grabbed onto something within frameCount frames, configure projection for that joint.
            if (joinTrigger != null && joinTrigger.isJoined)
            {
                joinTrigger.currentJoint.projectionMode = JointProjectionMode.PositionAndRotation;
                joinTrigger.currentJoint.projectionDistance = 1.25f;
                joinTrigger.currentJoint.projectionAngle = 100f;
            }
        }
    }
}