/*
AntiPhysXExplosion.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    class AntiPhysXExplosion : MonoBehaviour
    {
        private const int FRAMECOUNT = 15;  //The number of frames this component waits before making changes
        private void Awake()
        {
            if(StatMaster.isClient && !Modding.Game.IsSetToLocalSim) Destroy(this);
            if(Modding.Game.IsSimulating) StartCoroutine(FixJointProjection(FRAMECOUNT));
        }
        private System.Collections.IEnumerator FixJointProjection(int frameCount)
        {
            //Wait frameCount FixedUpdates into sim
            for (int i = 0; i < frameCount; i++)
		    {
		    	yield return new WaitForFixedUpdate();
		    }

            //joinTrigger is the component that manages the grabber's grabby joint/trigger combo.
			JoinOnTriggerBlock joinTrigger = gameObject.GetComponentInChildren<JoinOnTriggerBlock>();
            //If the grabber has grabbed onto something within frameCount frames, configure projection for that joint.
            if(joinTrigger != null && joinTrigger.isJoined)
			{
                joinTrigger.currentJoint.projectionMode = JointProjectionMode.PositionAndRotation;
                joinTrigger.currentJoint.projectionDistance = 0.55f;
                joinTrigger.currentJoint.projectionAngle = 0.45f;
			}
            Destroy(this);
        }
    }
}