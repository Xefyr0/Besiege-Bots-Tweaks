using Modding;
using UnityEngine;
using Modding.Blocks;


namespace BotFix
{
    public class FlameTFix : MonoBehaviour
    {
        private FlamethrowerController FC;
        private ConfigurableJoint CJ;
        private Transform FT;
        private AudioSource FireSound;
        
        private bool isFirstFrame = true;
        private int baseAmmo = 240;

        private MessageType mLoadFlamerAmmo = ModNetworking.CreateMessageType(DataType.Block);
        private MessageType mPlayFireSound = ModNetworking.CreateMessageType(DataType.Block);
        private MessageType mStopFireSound = ModNetworking.CreateMessageType(DataType.Block);
        private MessageType mKillFire = ModNetworking.CreateMessageType(DataType.Block);
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
            FireSound.clip = Soundfiles.Flameloop;
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
        }

        private void Update()
        {
            //If the local instance isn't a host, SP or local simmer in sim then return to save frames
            if ((StatMaster.isClient && !Game.IsSetToLocalSim) || !FC.SimPhysics) return;    

            //On the first frame, load the flamethrower.      
            if (isFirstFrame)
            {
                isFirstFrame = false;
                ModNetworking.SendToAll(LFA);
                LoadFireAmmo();
            }

            //Flamethrower sound toggle and pitch adjustment
            if (FT.gameObject.activeSelf)
            {
                if (!FireSound.isPlaying)
                {
                    ModNetworking.SendToAll(PFS);
                    FireSound.Play();
                }
                FireSound.pitch = Time.timeScale;
            }
            else
            {
                if (FireSound.isPlaying)
                {
                    ModNetworking.SendToAll(SFS);
                    FireSound.Stop();
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
            FC.OnReloadAmmo(ref baseAmmo, AmmoType.Fire, true, true);
            ModConsole.Log("Loaded Flamethrower to {0} seconds", baseAmmo * 0.25f);
        }
        public void KillFire()
        {
            //These variable changes are hacky, but the method I'd like to call instead is private so I've no other choice.
            FC.timeOut = true;
            FC.fireParticles.Stop();
            FT.gameObject.SetActive(false);

            //FlameTFix does nothing after the the joint is broken, so it can be destroyed.
            Object.Destroy(this);
        }
    }
}

