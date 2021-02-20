using UnityEngine;

namespace BotFix
{
    public class SpinBfix : MonoBehaviour
    {       
        private Rigidbody rigg;
        private Collider[] colliders;
        private HingeJoint HJ;
        private BlockBehaviour BB;
        private bool firstframe = false;
        private int fcounter;

        private bool MakeInvinc = false;
        private MToggle MI;

        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            if (Mod.ModbotMode)
                MakeInvinc = true;

            MI = BB.AddToggle("Make Invincible", "MVI", MakeInvinc);
            MI.Toggled += (bool value) => { MakeInvinc = value; };

            MI.DisplayInMapper = true;
        
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;

                colliders = GetComponentsInChildren<Collider>();

                foreach (Collider collider in colliders)
                {
                    collider.material.dynamicFriction = 0.05f;
                    collider.material.staticFriction = 0.05f;
                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }

                if (!GetComponent<HingeJoint>())
                    return;
                HJ = GetComponent<HingeJoint>();
            }
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.SimPhysics)
                {
                    if (!firstframe)
                    {
                        if (!HJ)
                        {
                            firstframe = true;
                            return;
                        }
                        fcounter++;

                        if (MakeInvinc)
                            return;
                        else
                        {
                            HJ.breakForce = 90000;
                            HJ.breakTorque = 90000;
                            //rigg.maxAngularVelocity = 5;
                        }
                       
                        if (fcounter == 4)
                            firstframe = true;
                    }
                }
            }
        }
    }
}

