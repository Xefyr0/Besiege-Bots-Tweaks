/*
 * InvincibleJointToggle.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * For a long while in Besiege Bots, it's been hard to make a spinner that does not break.
 * Eventually the strategy converged onto making a spinner entirely out of invincible blocks.
 * Over time, people wanted to use blocks like Blades in spinners,
 * and continue using blocks with nerfed joint strengths like Spinning Blocks.
 * This class provides whatever it's attached to with a joint strength invincibility toggle
 */

using UnityEngine;

namespace BesiegeBotsTweaks
{
    class InvincibleJointToggle : FrameDelayAction
    {
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;

        protected override int FRAMECOUNT { get; } = 3;
        new void Awake()
        {
            joint = GetComponent<Joint>();  //Primary connections are reliably created before simblocks Awake I guess? weird but noted.

            toggle = thisBlock.InternalObject.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => Invincible = value;
        }
        protected override void DelayedAction()
        {
            //In the event that the invincible button is toggled on, make the block joint invincible.
            if (Invincible && joint != null)
            {
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
            }
        }
    }
}

