/*
grabberfix.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Modding;
using Modding.Common;
using Modding.Blocks;

namespace BotFix
{
    public class Grabberfix : MonoBehaviour
    {
        private static readonly int FRAMECOUNT = 20;  //The number of frames this component waits before making changes.
        private int selectedmode = 0;
        private const int MediumBF = 100000, UltraBF = 150000, DrumBF = 200000;
        private static readonly List<string> GrabMode = new List<string>()
        {
            "Vanilla",
            "Medium",
            "Ultra",
            "Drum"
        };
        void Awake()
        {
            Block block = Block.From(base.gameObject);

            //Mapper Menu definition. Must be defined for both simulation block and buildmode block.
            MMenu UgrabM = block.InternalObject.AddMenu("Grabmode", selectedmode, GrabMode, false);
            UgrabM.ValueChanged += (ValueHandler)(value => {selectedmode = value;});
            UgrabM.DisplayInMapper = true;

            if(block.InternalObject.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                //4. As client on the local instance if we're in local sim
                if (Player.GetHost() == null || (block.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !block.Machine.Player.InLocalSim)) StartCoroutine(GrabberSwitch());
                else Destroy(this);
            }
        }

        private IEnumerator GrabberSwitch()
        {
            //Wait FRAMECOUNT FixedUpdates into sim
            for (int i = 0; i < FRAMECOUNT; i++) yield return new WaitForFixedUpdate();

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

        /*Old betamode grabbers. Will not work if uncommented, it needs some obsolete class members.
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

