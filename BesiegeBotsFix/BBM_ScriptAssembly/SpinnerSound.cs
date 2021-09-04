using System;
using Modding;
using Modding.Blocks;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace BotFix
{
    public class SpinnerSound : MonoBehaviour
    {
        //List<AudioClip> MSounds = new List<AudioClip>();
 
        public List<AudioClip> MSounds = new List<AudioClip>()
        {
            Soundfiles.Bar1,
            Soundfiles.Bar2,
            Soundfiles.Bar3,
            Soundfiles.Bar4,
            Soundfiles.Bar5,
            Soundfiles.Bar6,


            Soundfiles.Disc1,
            Soundfiles.Disc2,
            Soundfiles.Disc3,
            Soundfiles.Disc4,
            Soundfiles.Disc5,
            Soundfiles.Disc6,


            Soundfiles.Toothy_Disc,
            Soundfiles.DualDisc,

            Soundfiles.Drum1,
            Soundfiles.Drum2,


            Soundfiles.Shell1,
            Soundfiles.Shell2,
            Soundfiles.Shell3,
        };

        internal static List<string> Soundlist = new List<string>()
        {
            "Big Bar",
            "High Rpm Bar",
            "Brushless Bar",
            "Big Asym. Bar",
            "Small Bar",
            "Big F*** Bar",

            "Sym. Disc",
            "Asym. Disc",
            "Sym. Disc 2",
            "Big Asym. Disc",
            "Big Sym. Disc",
            "High Rpm Disc",

            "Saw Disc",
            "Dual Disc",

            "High Rpm Drum",
            "Med. Rpm Drum",

            "Resonant Shell",
            "Very Big Bar",
            "Noisy Shell"

        };

        private bool Tstate = false;
        private bool UseMotorSound;
        public BlockBehaviour BB;

        public float Pitch = 1;
        public float startingVolume = 0.3f;
        public float timeToDecrease = 1;
        public AudioSource CurrentSound;
        public int SelectedSound = 0;
        private MMenu SoundMenu;
        private MToggle MotorSoundToggle;
        private MSlider PitchSlider;
        public Block thisblock;
        bool firstframe = true;

        public Quaternion Angle;
        public float angval;
        public float angvel;
        private Quaternion lastAngle;
        private Queue<float> averageAngularVelocityQueue = new Queue<float>();
        private int queueSize = 20;
        private int Sendrate = 0;


        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            MotorSoundToggle = BB.AddToggle("SpinnerSound", "Sound", UseMotorSound);
            MotorSoundToggle.Toggled += (value) =>
            {
                UseMotorSound = SoundMenu.DisplayInMapper = PitchSlider.DisplayInMapper = value;
            };

            PitchSlider = BB.AddSlider("Pitch", "Pitcher", Pitch, 0f, 4f);
            PitchSlider.ValueChanged += (float value) => { Pitch = value; };

            SoundMenu = BB.AddMenu("SpinnerSound", SelectedSound, Soundlist, false);
            SoundMenu.ValueChanged += (value => { SelectedSound = value; });

            PitchSlider.DisplayInMapper = UseMotorSound;
            SoundMenu.DisplayInMapper = UseMotorSound;
            MotorSoundToggle.DisplayInMapper = true;
            thisblock = Block.From(gameObject);


        }

        private void FixedUpdate()
        {
            if (BB.SimPhysics || StatMaster.isClient)
            {
                if (UseMotorSound)
                { 
                    if (firstframe)
                    {
                        CurrentSound = gameObject.AddComponent<AudioSource>();
                        CurrentSound.playOnAwake = false;
                        CurrentSound.spatialBlend = 1;
                        CurrentSound.maxDistance = 150f;
                        CurrentSound.rolloffMode = AudioRolloffMode.Linear;
                        CurrentSound.volume = startingVolume;
                        CurrentSound.loop = false;                     
                        CurrentSound.clip = MSounds[SelectedSound];
                        firstframe = false;
                        angval = 0;
                        angvel = 0;
                        averageAngularVelocityQueue.Clear();
                    }

                    if (!StatMaster.isClient)
                    {
                        AngularVelocity();
                    }

                    CurrentSound.pitch = Pitch * Time.timeScale;
                    if (angval >= 50)
                    {
                        if (!Tstate)
                        {
                            CurrentSound.volume = startingVolume;
                            CurrentSound.loop = true;
                            CurrentSound.Play();
                            Tstate = true;
                        }

                        CurrentSound.pitch = (angval / 700) * Time.timeScale * Pitch;
                        CurrentSound.volume = (angval / 700) * startingVolume;
                    }
                    else
                    {
                        CurrentSound.loop = false;
                        CurrentSound.Stop();
                        Tstate = false;
                    }
                }
            }
        }

        public void AngularVelocity()
        {           
            Angle = BB.transform.rotation;
            
            angval = Quaternion.Angle(Angle, lastAngle);

            if (averageAngularVelocityQueue.Count == queueSize)
                averageAngularVelocityQueue.Dequeue();
            averageAngularVelocityQueue.Enqueue(angval);
            float zero = 0;
            foreach (float averageAng in averageAngularVelocityQueue)
                zero += averageAng;
            angval = zero / (float)averageAngularVelocityQueue.Count;

            angval = angval * 16.5f;
            lastAngle = Angle;

            if (Sendrate == 0)
            {
                ModNetworking.SendToAll(Messages.SPEED.CreateMessage(thisblock, Convert.ToSingle(angval)));
            }

            Sendrate++;
            if (Sendrate == 2)
            {
                Sendrate = 0;
            }
        }

        public static void AngularVelocityClient(Message m)
        {
            //NRE here on callbacks.
            //Debug.Log("Gotmessage:");
            Block BL = (Block)m.GetData(0);
            //Debug.Log(BL);
            if(BL == null) return;
            BL.InternalObject.GetComponent<SpinnerSound>().angval = (float)m.GetData(1);
        }
    }
}









        

        

        
    

