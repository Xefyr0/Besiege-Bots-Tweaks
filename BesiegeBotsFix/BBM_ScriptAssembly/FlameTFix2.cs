using Modding;
using UnityEngine;
using Modding.Blocks;


namespace BotFix
{
    public class FlameTFix : MonoBehaviour
    {
        private FlamethrowerController FC;
        private ConfigurableJoint CJ;
        private MKey Fkey;
        private BoxCollider BBC;
        private Transform FT;
        private bool broke = false;
        private ParticleSystem fire;
        private bool isFirstFrame = true;

        public int Ammo = 60;
        public ServerMachine SM;

        private AudioSource Firesound;

        //private MColourSlider colslid;
        //public Color32 fcolor = new Color32(255,240,0,255);
        //public Color32 startcolor = new Color32(255, 240, 0, 255);

        void Awake()
        {            
            FC = GetComponent<FlamethrowerController>();
            fire = GetComponentInChildren<ParticleSystem>();
            SM = GetComponentInParent<ServerMachine>();

            //colslid = FC.AddColourSlider("Firecolor", "Firecolor", fcolor, false);
            //colslid.ValueChanged += (ColourChangeHandler)(value => { fcolor = value; });
            //colslid.DisplayInMapper = true;

            fire.transform.Translate(new Vector3(0, 0, 0.4f));
            var col = fire.collision;
            col.enabled = true;
            col.type = ParticleSystemCollisionType.World;
            col.mode = ParticleSystemCollisionMode.Collision3D;
            col.bounce = 0f;
            col.radiusScale = 0.1f;
            col.maxCollisionShapes = 200;
            col.dampen = 0f;
            fire.startSpeed = 50f;

            //var cl = fire.colorOverLifetime;
            //cl.enabled = true;
            //Gradient grad = new Gradient();
            //grad.SetKeys(new GradientColorKey[] { new GradientColorKey(startcolor, 0.0f), new GradientColorKey(fcolor, 0.3f), new GradientColorKey(Color.black, 0.7f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 0.1f), new GradientAlphaKey(0.0f, 1.0f) });
            //cl.color = grad;
            
            FC.Prefab.myDamageType = DamageType.Blunt;
            //Fkey = FC.IgniteKey;

            FT = transform.Find("FireTrigger");
            FireController fireC = FT.GetComponent<FireController>();
            fireC.enabled = false;

            Firesound = FC.gameObject.AddComponent<AudioSource>();
            Firesound.rolloffMode = AudioRolloffMode.Linear;
            Firesound.spatialBlend = 1;
            Firesound.playOnAwake = false;
            Firesound.clip = Soundfiles.Flameloop;
            Firesound.loop = true;
            Firesound.maxDistance = 100f;
            Firesound.volume = 0.05f;
            
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponent<ConfigurableJoint>();
                CJ.breakForce = 40000;
                CJ.breakTorque = 40000;
            }
        }

        private void Update()
        {
            if (StatMaster.isClient || StatMaster.isLocalSim)
                return;

            if (FC.SimPhysics)
            {                      
                if (isFirstFrame)
                {
                    isFirstFrame = false;
                    Reloadfire();
                }

                if (FT.gameObject.activeSelf)
                {
                    if (!Firesound.isPlaying)
                    {
                        PlayFireSound();
                    }
                    Firesound.pitch = Time.timeScale;
                }
                else
                {
                    if (Firesound.isPlaying)
                    StopFireSound();
                }

                if (!CJ && !broke)
                {
                    ModNetworking.SendToAll(Messages.SF.CreateMessage(FC));
                    Stopfire();
                    broke = true;
                }
            }         
        }

        public void PlayFireSound()
        {
            Firesound.Play();

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                //Debug.Log("Send play message");
                ModNetworking.SendToAll(Messages.PFS.CreateMessage(FC));
            }
        }

        public void StopFireSound()
        {
            Firesound.Stop();

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                //Debug.Log("Send stop message");
                ModNetworking.SendToAll(Messages.SFS.CreateMessage(FC));
            }
        }

        public static void PlayFireSoundClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            BL.InternalObject.GetComponent<FlameTFix>().PlayFireSound();
        }

        public static void StopFireSoundClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            BL.InternalObject.GetComponent<FlameTFix>().StopFireSound();
        }

        public static void StopFireClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            BL.InternalObject.GetComponent<FlameTFix>().Stopfire();
        }

        public static void LoadFireClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            BL.InternalObject.GetComponent<FlameTFix>().Reloadfire();
        }

        public void Stopfire()
        {
            FC.timeOut = true;
            //FC.keyHeld = false;
            FC.fireParticles.Stop();
            FT.gameObject.SetActive(false);
        }

        public void Reloadfire()
        {
            SM.ReloadAmmo(240, AmmoType.Fire, true, true);
            Debug.Log("Loaded Flamethrower to 240 seconds");

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                ModNetworking.SendToAll(Messages.LF.CreateMessage(FC));
            }
        }
    }
}

