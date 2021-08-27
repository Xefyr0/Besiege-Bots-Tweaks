using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class InvincibleJointToggle : MonoBehaviour
    {       
        private BlockBehaviour BB;
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;
        private float normalBreakForce;
        private float normalBreakTorque;

        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            toggle = BB.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => ToggleInvincibility(value);

            joint = BB.GetComponentInChildren<Joint>();
            normalBreakForce = joint.breakForce;
            normalBreakTorque = joint.breakTorque;
        }
        internal void ToggleInvincibility(bool value)
        {
            if(value)
            {
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
            }
            else
            {
                joint.breakForce = normalBreakForce;
                joint.breakTorque = normalBreakTorque;
            }
        }
    }
}

