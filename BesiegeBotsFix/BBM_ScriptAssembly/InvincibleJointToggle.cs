using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class InvincibleJointToggle : MonoBehaviour
    {       
        private BlockBehaviour BB;
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;
        private byte frameCount;

        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            toggle = BB.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => Invincible = value;

            joint = BB.GetComponentInChildren<Joint>();
        }
        internal void Update()
        {
            if(BB.SimPhysics)
            {
                if(frameCount == 5)
                {
                    if(Invincible)
                    {
                        joint.breakForce = Mathf.Infinity;
                        joint.breakTorque = Mathf.Infinity;
                    }
                    Object.Destroy(this);
                }
                frameCount++;
            }
        }
    }
}

