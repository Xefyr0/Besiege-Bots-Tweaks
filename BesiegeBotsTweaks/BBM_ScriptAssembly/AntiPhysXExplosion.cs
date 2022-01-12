/*
AntiPhysXExplosion.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    class AntiPhysXExplosion : MonoBehaviour
    {
        private static readonly int FRAMECOUNT = 15;  //The number of frames this component waits before making changes.
        //The entry point into this simple class is when it's attached to a GameObject. Awake is generally called once on blocks, when they're created in build mode.
        private void Awake()
        {
            Block block = Block.From(base.gameObject);

            if(block.InternalObject.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                //4. As client on the local instance if we're in local sim
                if (Player.GetHost() == null || (block.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !block.Machine.Player.InLocalSim)) StartCoroutine(ConfigureJointProjection());
                else Destroy(this);
            }
        }
        private System.Collections.IEnumerator ConfigureJointProjection()
        {
            //Wait FRAMECOUNT FixedUpdates into sim
            for (int i = 0; i < FRAMECOUNT; i++) yield return new WaitForFixedUpdate();

            //joinTrigger is the component that manages the grabber's grabby joint/trigger combo.
			JoinOnTriggerBlock joinTrigger = base.gameObject.GetComponentInChildren<JoinOnTriggerBlock>();

            //If the grabber has grabbed onto something within frameCount frames, configure projection for that joint.
            if(joinTrigger != null && joinTrigger.isJoined)
			{
                joinTrigger.currentJoint.projectionMode = JointProjectionMode.PositionAndRotation;
                joinTrigger.currentJoint.projectionDistance = 1.00f;
                joinTrigger.currentJoint.projectionAngle = 100f;
			}

            //The component instance is destroyed after the necessary changes are made.
            Destroy(this);
        }
    }
}