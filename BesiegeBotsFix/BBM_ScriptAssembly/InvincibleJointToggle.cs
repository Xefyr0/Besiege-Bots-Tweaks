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
    public class InvincibleJointToggle : MonoBehaviour
    {
        private const byte FRAMECOUNT = 3;  //The number of frames this component waits before making changes
        private byte frameCounter = 0;  //frameCounter Variable to keep track of how many frames have elapsed
        private BlockBehaviour BB;
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;
        private void Awake()    //Init some vars & the toggle button on Awake
        {
            BB = GetComponent<BlockBehaviour>();
            joint = BB.GetComponent<Joint>();

            if(BB == null) Object.Destroy(this);
            
            toggle = BB.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => Invincible = value;
        }
        private void Update()
        {
            //Wait until sim starts, then starts counting frames until FRAMECOUNT frames are reached
            if(!BB.SimPhysics) return;
            frameCounter++;
            if(frameCounter < FRAMECOUNT) return;

            //If the block isn't actually part of a simulation (i.e. on a client computer in multiverse with local sim turned off) then the component instance is destroyed since it won't do anything either way
            if(StatMaster.isClient && !StatMaster.isLocalSim) Object.Destroy(this);

            //In the event that the invincible button is toggled on, make the block invincible.
            if(Invincible && joint != null)
            {
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
            }

            Object.Destroy(this);   //The component instance is destroyed after the necessary changes are made.
        }
    }
}

