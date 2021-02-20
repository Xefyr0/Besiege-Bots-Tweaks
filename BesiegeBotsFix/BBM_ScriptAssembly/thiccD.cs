
using Modding;
using UnityEngine;


namespace BotFix
{
    public class ThiccD : MonoBehaviour
    {
        public CogMotorDamage CD;
        public BlockHealthBar bh;
        private BlockBehaviour BB;

        private HingeJoint HJ;
        private CogMotorControllerHinge CH;
        private bool firstframe = false;
        private int fcounter;
        private Rigidbody rigg;

        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            
            BB.Prefab.myDamageType = DamageType.Blunt;

            if (!GetComponent<HingeJoint>())
                return;
            HJ = GetComponent<HingeJoint>();

            
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {              
                CD = GetComponent<CogMotorDamage>();
                CD.damageToBlock = 0f;
                CD.jointDamageScale = 0f;
                //CD.UseCollisionStay = false;
                //CD.enabled = false;

                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 5;
            }
            
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.SimPhysics)
                {
                    if (BB.Prefab.ID == 48)
                        return;

                    if (!firstframe)
                    {
                        if (!HJ)
                        {
                            firstframe = true;
                            return;
                        }
                        fcounter++;
                        HJ.breakForce = 90000;
                        HJ.breakTorque = 90000;
                        //BB.jointBreakForce = 5;
                        if (fcounter == 4)
                            firstframe = true;
                    }
                }
            }
        }
    }
}

