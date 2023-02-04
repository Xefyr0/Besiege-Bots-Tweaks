/*
 * grabberfix.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 */

using System.Collections.Generic;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    class Grabberfix : FrameDelayAction
    {
        private int selectedmode = 0;
        private const int MediumBF = 100000, UltraBF = 150000, DrumBF = 200000;
        private static readonly List<string> GrabMode = new List<string>()
        {
            "Vanilla",
            "Medium",
            "Ultra",
            "Drum"
        };

        protected override int FRAMECOUNT { get; } = 20;

        new void Awake()
        {
            base.Awake();

            //Mapper Menu definition. Must be defined for both simulation block and buildmode block.
            MMenu UgrabM = thisBlock.InternalObject.AddMenu("Grabmode", selectedmode, GrabMode, false);
            UgrabM.ValueChanged += (ValueHandler)(value => {selectedmode = value;});
            UgrabM.DisplayInMapper = true;
        }

        protected override void DelayedAction()
        {
            //uGrabbers, mGrabbers and dGrabbers change the strength value of *all* joints, not just the grabby one
            Joint[] joints = GetComponents<Joint>();
            foreach (Joint joint in joints)
            {
                switch(selectedmode)
                {
                    case 0:
                        Destroy(this);
                        break;
                    case 1:
                        joint.breakForce = joint.breakTorque = MediumBF;
                        break;
                    case 2:
                        joint.breakForce = joint.breakTorque = UltraBF;
                        break;
                    case 3:
                        joint.breakForce = joint.breakTorque = DrumBF;
                        break;
                }
            }
        }

        /*Old betamode grabbers legacy code. Will not work if uncommented, it needs some obsolete class members.
        private void FixedUpdate()
        {
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
                    }
                    Rigidbody prigg = GetComponentInParent<Rigidbody>();

                    prigg.mass = 10f;
                    prigg.ResetInertiaTensor();

                    FC++;
                }
            }
        }
        */      
    }
}

