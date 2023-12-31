/*
 * FlameTFix.cs
 * Written by DokterDoyle for the Besiege Bots community
 * Amended by Xefyr
 * 
 * This class increases the ammunition a flamethrower starts with depending on the number present in the machine
 * and adds sound to the flamethrower when it is running.
 */
using Modding;
using Modding.Blocks;
using UnityEngine;


namespace BesiegeBotsTweaks
{
    class FlameTFix : MonoBehaviour
    {
        private Block block;
        private FlamethrowerController FC;
        private Transform FT;
        private AudioSource FireSound;
        private int baseAmmo = 240;

        private static MessageType mLoadFlamerAmmo, mPlayFireSound, mStopFireSound, mKillFire;
        private Message LFA, PFS, SFS, KF;

        internal static void SetupNetworking()
        {
            //Flamethrower message callbacks
            //TODO: Callbacks cause NRE's sometimes in multiverse? confirm again after 7/15/2022
            mLoadFlamerAmmo = ModNetworking.CreateMessageType(DataType.Block);
            mPlayFireSound = ModNetworking.CreateMessageType(DataType.Block);
            mStopFireSound = ModNetworking.CreateMessageType(DataType.Block);
            mKillFire = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[mLoadFlamerAmmo] += delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().LoadFireAmmo();};
            ModNetworking.Callbacks[mPlayFireSound] += delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().FireSound.Play();};
            ModNetworking.Callbacks[mStopFireSound] += delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().FireSound.Stop();};
            ModNetworking.Callbacks[mKillFire] += delegate (Message m) { ((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().KillFire(); };
        }

        private void Awake()
        {
            //Variable init
            FC = GetComponent<FlamethrowerController>();
            FT = transform.Find("FireTrigger");
            block = Block.From(FC);

            //FireSound init
            FireSound = FC.gameObject.AddComponent<AudioSource>();
            FireSound.rolloffMode = AudioRolloffMode.Linear;
            FireSound.spatialBlend = 1;
            FireSound.playOnAwake = false;
            FireSound.clip = ModResource.GetAudioClip("Flamerhrowerloop");
            FireSound.loop = true;
            FireSound.maxDistance = 100f;
            FireSound.volume = 0.05f;

            //Flamethrower Messages
            LFA = mLoadFlamerAmmo.CreateMessage(FC);
            PFS = mPlayFireSound.CreateMessage(FC);
            SFS = mStopFireSound.CreateMessage(FC);
            KF = mKillFire.CreateMessage(FC);
        }

        
        private void Start()
        {
            //If block isn't in sim then it shouldn't do anything.
            if (!FC.isSimulating || !FC.SimPhysics) return;

            //Load fire ammo when sim starts
            ModNetworking.SendToAll(LFA);
            LoadFireAmmo();
        }

        private void FixedUpdate()
        {
            //If block isn't in sim then it shouldn't do anything.
            if (!FC.isSimulating || !FC.SimPhysics) return;

            //Flamethrower sound toggle and pitch adjustment
            if (FT.gameObject.activeSelf)
            {
                if (!FireSound.isPlaying)
                {
                    FireSound.Play();
                    ModNetworking.SendToAll(PFS);
                }
                FireSound.pitch = Time.timeScale;
            }
            else
            {
                if (FireSound.isPlaying)
                {
                    FireSound.Stop();
                    ModNetworking.SendToAll(SFS);
                }
            }
        }
        //If there isn't a joint attaching this to another block, then shut off all functionality.
        private void OnJointBreak(float breakforce)
        {
            //Modding.ModConsole.Log("No joint. Killing Fire");
            ModNetworking.SendToAll(KF);
            KillFire();
        }

        private void LoadFireAmmo()
        {
            baseAmmo /= block.Machine.GetBlocksOfType(BlockType.Flamethrower).Count;
            //ModConsole.Log("Loading Flamethrower to {0} seconds", baseAmmo * 0.25f + 10f/*FC.baseAmmo*/);
            FC.OnReloadAmmo(ref baseAmmo, AmmoType.Fire, false, true);
        }

        private void KillFire()
        {
            //These variable changes are hacky, but the method I'd like to call instead is private so I've no other choice.
            FC.timeOut = true;
            FC.fireParticles.Stop();

            //FlameTFix does nothing after the the joint is broken, so it can be destroyed.
            Destroy(this);
        }
    }
}

