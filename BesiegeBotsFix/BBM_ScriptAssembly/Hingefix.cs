using UnityEngine;

namespace BotFix
{
    public class Hfix : MonoBehaviour
    {
        private ConfigurableJoint CJ;
        private HingeJoint HJ;
        public float BF = 30000f;
        private StrengthenTreads ST;
        private Rigidbody rigg;

        void Awake()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {

/*
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
            */
                if (GetComponent<ConfigurableJoint>())
                {
                    CJ = GetComponent<ConfigurableJoint>();
                    CJ.breakForce = BF;
                    CJ.breakTorque = BF;
                }

                if (GetComponent<HingeJoint>())
                {
                    HJ = GetComponent<HingeJoint>();
                    HJ.breakForce = BF;
                    HJ.breakTorque = BF;
                }

                if (GetComponent<StrengthenTreads>())
                {
                    ST = GetComponent<StrengthenTreads>();
                    ST.breakForce = 50000;
                    ST.breakTorque = 50000;
                }
            }
        }
    }
}

