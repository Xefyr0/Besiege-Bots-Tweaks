/*
InvincibleJointToggle.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BesiegeBotsTweaks
{
    /*
    For a long while in Besiege Bots, it's been hard to make a spinner that does not break.
    Eventually the strategy converged onto making a spinner entirely out of invincible blocks.
    Over time, people wanted to use blocks like Blades in spinners,
    and continue using blocks with nerfed joint strengths like Spinning Blocks.
    This class provides whatever it's attached to with a joint strength invincibility toggle
    so that they can function as a part of a spinner.
    */
    [RequireComponent(typeof(BlockBehaviour))]
    public class InvincibleJointToggle : MonoBehaviour
    {
        private static readonly int FRAMECOUNT = 3;  //The number of frames this component waits before making changes.
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;
        private void Awake()
        {
            Block block = Block.From(base.gameObject);
            joint = GetComponent<Joint>();  //Primary connections are reliably created before simblocks Awake I guess? weird but noted.

            toggle = block.InternalObject.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => Invincible = value;

            if (block.InternalObject.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                //4. As client on the local instance if we're in local sim
                if (Player.GetHost() == null || (block.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !block.Machine.Player.InLocalSim)) StartCoroutine(MakeInvincible());
                else Destroy(this);    //Maybe not a good idea? Toggles like to be declared in buildmode AND sim
            }
        }
        private System.Collections.IEnumerator MakeInvincible()
        {
            //Wait FRAMECOUNT FixedUpdates into sim
            for (int i = 0; i < FRAMECOUNT; i++) yield return new WaitForFixedUpdate();

            //In the event that the invincible button is toggled on, make the block invincible.
            if (Invincible && joint != null)
            {
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
            }

            //The component instance is destroyed after the necessary changes are made.
            Destroy(this);
        }
    }
}

