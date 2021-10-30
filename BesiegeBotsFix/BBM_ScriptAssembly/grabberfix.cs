/*
AxialDragToggle.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/

using UnityEngine;
using System.Collections.Generic;

namespace BotFix
{
    public class Grabberfix : MonoBehaviour
    {
        private const byte FRAMECOUNT = 20;  //The number of frames this component waits before making changes
        private byte frameCounter = 0;  //frameCounter Variable to keep track of how many frames have elapsed
        public int selectedmode = 0;
        private const int MediumBF = 100000;
        private const int UltraBF = 150000;
        private const int DrumBF = 250000;
        internal static List<string> GrabMode = new List<string>()
        {
            "Vanilla",
            "Medium",
            "Ultra",
            "Drum"
        };
        void Awake()
        {
            //If this component is on a client, then destroy it as it won't do anything anyways.
            if (StatMaster.isClient && !Modding.Game.IsSetToLocalSim) Destroy(this);

            //Mapper Menu definition
            MMenu UgrabM = GetComponent<GrabberBlock>().AddMenu("Grabmode", selectedmode, GrabMode, false);
            UgrabM.ValueChanged += (ValueHandler)(value => {selectedmode = value;});
            UgrabM.DisplayInMapper = true;
        }

        void FixedUpdate()
        {
            if (!Modding.Game.IsSimulating) return;
            if (frameCounter < FRAMECOUNT)
            {
                ConfigurableJoint[] CJ = GetComponents<ConfigurableJoint>();
                foreach (ConfigurableJoint joint in CJ)
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
                frameCounter++;
            } else Destroy(this);

            /*                  Old betamode grabbers. Will not work if uncommented, it needs some obsolete class members.
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
            */      
        }
    }
}

