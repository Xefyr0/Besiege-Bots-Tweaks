using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class InvincibleJointToggle : MonoBehaviour
    {       
        private BlockBehaviour BB;
        private Joint joint;
        private bool Invincible = false;
        private MToggle toggle;
        private byte frameCounter = 0;
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            toggle = BB.AddToggle("Make Invincible", "MVI", Invincible);
            toggle.Toggled += (bool value) => Invincible = value;
            joint = BB.GetComponent<Joint>();
        }
        private void Update()
        {
            if(!Modding.Game.IsSimulatingGlobal) return;
            frameCounter++;
            if(frameCounter < 4) return;
            if(Invincible && joint != null)
            {
                joint.breakForce = Mathf.Infinity;
                joint.breakTorque = Mathf.Infinity;
            }
            Object.Destroy(this);
        }
    }
}

