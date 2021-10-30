/*
InvincibleJointToggle.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

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
        private const byte FRAMECOUNT = 3;  //The number of frames this component waits before making changes
        private byte frameCounter = 0;  //frameCounter Variable to keep track of how many frames have elapsed
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;
        private void Awake()    //Init some vars & the toggle button on Awake
        {
            BlockBehaviour BB = GetComponent<BlockBehaviour>();
            joint = BB.GetComponent<Joint>();  //Primary connections are reliably created before simblocks Awake I guess? weird but noted.
            
            toggle = BB.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => Invincible = value;
        }
        private void Update()
        {
            //Wait until sim starts, then starts counting frames until FRAMECOUNT frames are reached
            if(!Modding.Game.IsSimulating) return;
            frameCounter++;
            if(frameCounter < FRAMECOUNT) return;

            //If the block isn't actually part of a simulation (i.e. on a client computer in multiverse with local sim turned off) then the component instance is destroyed since it won't do anything either way
            if(StatMaster.isClient && !Modding.Game.IsSetToLocalSim) Destroy(this);

            //In the event that the invincible button is toggled on, make the block invincible.
            if(Invincible && joint != null)
            {
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
            }

            Destroy(this);   //The component instance is destroyed after the necessary changes are made.
        }
    }
}

