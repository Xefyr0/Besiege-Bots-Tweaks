using UnityEngine;
using Modding.Blocks;
using System.Collections.Generic;

namespace BotFix
{
    public class Grabberfix : MonoBehaviour
    {
        public BBMUI BBMToolbox;
        private Collider[] colliders;
        private Rigidbody rigg;
        private Block blocc;
        private ConfigurableJoint[] CJ;
        private GrabberBlock GB;
        private Rigidbody target;

        private MToggle MI;
        private MMenu UgrabM;

        private int FC = 0;
        private int fcounter;
        public int selectedmode;
        private int BFUgr = 100000;
        private int BFSpinnerUgr = 150000;

        private bool firstframe = true;

        internal static List<string> GrabMode = new List<string>()
        {
            "Vanilla",
            "Medium",
            "Ultra",
        };

        void Awake()
        {
            //Get Stuff
            GB = GetComponent<GrabberBlock>();
            rigg = GetComponent<Rigidbody>();

            //Mapper definition
            UgrabM = GB.AddMenu("Grabmode", selectedmode, GrabMode, false);
            UgrabM.ValueChanged += (ValueHandler)(value => { selectedmode = value; });

            //DisplayInMapper config
            UgrabM.DisplayInMapper = true;

            //Physics Stuff
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                //rigg.mass = 0.7f;
                /*
                rigg.drag = 0.01f;
                rigg.angularDrag = 0f;
                */
                rigg = GetComponent<Rigidbody>();
                //rigg.maxAngularVelocity = 100;
                /*
                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.material.dynamicFriction = 0.05f;
                    collider.material.staticFriction = 0.05f;
                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }*/
            }
        }

        void FixedUpdate()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (GB.SimPhysics)
                {
                    if (selectedmode == 0)
                        return;

                    if (firstframe)
                    {
                        fcounter++;

                        CJ = GetComponents<ConfigurableJoint>();
                        foreach (ConfigurableJoint joint in CJ)
                        {
                            if (selectedmode == 1)
                            {
                                joint.breakForce = BFUgr;
                                joint.breakTorque = BFUgr;
                            }
                            else if (selectedmode == 2)
                            {
                                joint.breakForce = BFSpinnerUgr;
                                joint.breakTorque = BFSpinnerUgr;
                            }
                        }
                        if (fcounter == 20)
                            firstframe = false;
                    }
                }
            }

            if (Mod.BetaMode)
            {
                if (!StatMaster.isClient || StatMaster.isLocalSim)
                {
                    if (FC >= 4)
                        return;

                    CJ = GetComponents<ConfigurableJoint>();

                    foreach (ConfigurableJoint joint in CJ)
                    {
                        if (joint.connectedBody == null)
                            return;
                        //if (joint.breakForce != Mathf.Infinity)
                        //   return;

                        target = joint.connectedBody;
                        transform.parent = target.gameObject.transform;

                        Destroy(joint);

                        Rigidbody[] aim = GetComponents<Rigidbody>();
                        foreach (Rigidbody rigbod in aim)
                        {
                            Destroy(rigbod);
                        }

                        Rigidbody[] aim2 = GetComponentsInChildren<Rigidbody>();
                        foreach (Rigidbody rigbod in aim2)
                        {
                            Destroy(rigbod);
                        }


                        //{
                        //    transform.parent = collider.transform;
                        //}

                        //foreach (Collider collider in target.GetComponents<Collider>())
                        //{
                        //    transform.parent = collider.transform;
                        //}
                    }
                    Rigidbody prigg = GetComponentInParent<Rigidbody>();

                    prigg.mass = 10f;
                    prigg.ResetInertiaTensor();

                    Rigidbody priggg = GetComponentInParent<Rigidbody>();

                    priggg.mass = 10f;
                    priggg.ResetInertiaTensor();


                    FC++;
                }
            }          
        }
    }
}

