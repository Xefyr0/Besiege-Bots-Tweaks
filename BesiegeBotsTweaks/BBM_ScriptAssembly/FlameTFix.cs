/*
FlameTFix.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/
using Modding;
using UnityEngine;
using Modding.Common;
using Modding.Blocks;


namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    public class FlameTFix : MonoBehaviour
    {
        private Block block;
        private FlamethrowerController FC;
        private ConfigurableJoint CJ;
        private Transform FT;
        private AudioSource FireSound;
        
        private bool isFirstFrame = true;
        private int baseAmmo = 240;

        private static MessageType mLoadFlamerAmmo = ModNetworking.CreateMessageType(DataType.Block);
        private static MessageType mPlayFireSound = ModNetworking.CreateMessageType(DataType.Block);
        private static MessageType mStopFireSound = ModNetworking.CreateMessageType(DataType.Block);
        private static MessageType mKillFire = ModNetworking.CreateMessageType(DataType.Block);
        private Message LFA, PFS, SFS, KF;

        /*          Old Flamethrower color vars
        private MColourSlider colslid;
        public Color32 fcolor = new Color32(255,240,0,255);
        public Color32 startcolor = new Color32(255, 240, 0, 255);
        */

        void Awake()
        {
            //Variable init
            FC = GetComponent<FlamethrowerController>();
            CJ = GetComponent<ConfigurableJoint>();
            FT = transform.Find("FireTrigger");
            Block block = Block.From(FC);

            /*      Old Flamethrower color init
            colslid = FC.AddColourSlider("Firecolor", "Firecolor", fcolor, false);
            colslid.ValueChanged += (ColourChangeHandler)(value => { fcolor = value; });
            colslid.DisplayInMapper = true;

            var cl = fire.colorOverLifetime;
            cl.enabled = true;
            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(startcolor, 0.0f), new GradientColorKey(fcolor, 0.3f), new GradientColorKey(Color.black, 0.7f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) });
            cl.color = grad;
            */

            //FireSound init
            FireSound = FC.gameObject.AddComponent<AudioSource>();
            FireSound.rolloffMode = AudioRolloffMode.Linear;
            FireSound.spatialBlend = 1;
            FireSound.playOnAwake = false;
            FireSound.clip = ModResource.GetAudioClip("Flamerhrowerloop");
            FireSound.loop = true;
            FireSound.maxDistance = 100f;
            FireSound.volume = 0.05f;

            //Flamethrower message callbacks
            ModNetworking.Callbacks[mLoadFlamerAmmo] += (System.Action<Message>)delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().LoadFireAmmo();};
            ModNetworking.Callbacks[mPlayFireSound] += (System.Action<Message>)delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().FireSound.Play();};
            ModNetworking.Callbacks[mStopFireSound] += (System.Action<Message>)delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().FireSound.Stop();};
            ModNetworking.Callbacks[mKillFire] += (System.Action<Message>)delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<FlameTFix>().KillFire();};

            //Flamethrower Messages
            LFA = mLoadFlamerAmmo.CreateMessage(FC);
            PFS = mPlayFireSound.CreateMessage(FC);
            SFS = mStopFireSound.CreateMessage(FC);
            KF = mKillFire.CreateMessage(FC);

            if (FC.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                //4. As client on the local instance if we're in local sim
                if (Player.GetHost() == null || (block.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !block.Machine.Player.InLocalSim)) StartCoroutine(FirstFrame());
                else Destroy(this);    //Maybe not a good idea? There's multiplayer sound to consider
            }
        }

        private void FixedUpdate()
        {
            //If block isn't in sim then it shouldn't do anything.
            if (!FC.SimPhysics) return;

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

            //If there isn't a joint attaching this to another block, then shut off all functionality.
            if (!CJ)
            {
                ModNetworking.SendToAll(KF);
                KillFire();
            }
        }
        public void LoadFireAmmo()
        {
            //ModConsole.Log("Loading Flamethrower to {0} seconds", baseAmmo * 0.25f);
            FC.OnReloadAmmo(ref baseAmmo, AmmoType.Fire, true, true);
        }
        public void KillFire()
        {
            //These variable changes are hacky, but the method I'd like to call instead is private so I've no other choice.
            FC.timeOut = true;
            FC.fireParticles.Stop();
            FT.gameObject.SetActive(false); //This last one may be unnecessary

            //FlameTFix does nothing after the the joint is broken, so it can be destroyed.
            Destroy(this);
        }
        private System.Collections.IEnumerator FirstFrame()
        {
            yield return new WaitForFixedUpdate();
            ModNetworking.SendToAll(LFA);
            LoadFireAmmo();
        }
    }
}

