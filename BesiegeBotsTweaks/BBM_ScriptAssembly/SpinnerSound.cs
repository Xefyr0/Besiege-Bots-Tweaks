/*
SpinnerSound.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/
using System;
using Modding;
using Modding.Common;
using Modding.Blocks;
using UnityEngine;
using System.Collections.Generic;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    public class SpinnerSound : MonoBehaviour
    {
        //Soundfiles to load. Cannot be static :/
        public readonly List<AudioClip> MSounds = new List<AudioClip>()
        {
            ModResource.GetAudioClip("Bar1"),
            ModResource.GetAudioClip("Bar2"),
            ModResource.GetAudioClip("Bar3"),
            ModResource.GetAudioClip("Bar4"),
            ModResource.GetAudioClip("Bar5"),
            ModResource.GetAudioClip("Bar6"),

            ModResource.GetAudioClip("Disc1"),
            ModResource.GetAudioClip("Disc2"),
            ModResource.GetAudioClip("Disc3"),
            ModResource.GetAudioClip("Disc4"),
            ModResource.GetAudioClip("Disc5"),
            ModResource.GetAudioClip("Disc6"),

            ModResource.GetAudioClip("Disc7"),
            ModResource.GetAudioClip("Disc8"),

            ModResource.GetAudioClip("Drum1"),
            ModResource.GetAudioClip("Drum2"),

            ModResource.GetAudioClip("Shell1"),
            ModResource.GetAudioClip("Shell2"),
            ModResource.GetAudioClip("Shell3")
        };

        //In-game names for sounds, corresponds to MSounds
        internal readonly List<string> Soundlist = new List<string>()
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

        //Information about this block
        private BlockBehaviour BB;
        private Block thisblock;
        private bool getPhysics = false;

        //Information about the motor sound
        private float startingVolume = 0.3f;
        private AudioSource SoundSource;
        private bool PlayingSound = false;

        //Key Mapper information
        public bool UseMotorSound;
        public int SelectedSound = 0;
        public float pitchMultiplier = 1;
        private MToggle MotorSoundToggle;
        private MMenu SoundMenu;
        private MSlider PitchSlider;

        //Information for calculating the average velocity over queueSize frames
        private Quaternion currentRotation;
        private Quaternion lastRotation;
        private float deltaAngle;
        private Queue<float> averageAngularVelocityQueue = new Queue<float>();
        private int queueSize = 20;
        private byte updateCounter = 0;
        private readonly byte updateRate = 5;

        private static MessageType mAvgSpeed = ModNetworking.CreateMessageType(DataType.Block, DataType.Single);

        internal static void SetupNetworking()
        {
            ModNetworking.Callbacks[mAvgSpeed] += (System.Action<Message>)delegate(Message m)
            {
                Block target = (Block)m.GetData(0);
                if(target == null) return;
                else target.InternalObject.GetComponent<SpinnerSound>().UpdateSound((float)m.GetData(1));
            };
        }
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            thisblock = Block.From(gameObject);

            //Key Mapper Init
            MotorSoundToggle = BB.AddToggle("SpinnerSound", "Sound", UseMotorSound);
            MotorSoundToggle.Toggled += (value) => UseMotorSound = SoundMenu.DisplayInMapper = PitchSlider.DisplayInMapper = value;

            SoundMenu = BB.AddMenu("SpinnerSound", SelectedSound, Soundlist, false);
            SoundMenu.ValueChanged += (int value) => SelectedSound = value;
            SoundMenu.DisplayInMapper = UseMotorSound;

            PitchSlider = BB.AddSlider("Pitch", "Pitcher", pitchMultiplier, 0f, 4f);
            PitchSlider.ValueChanged += (float value) => pitchMultiplier = value;
            PitchSlider.DisplayInMapper = UseMotorSound;
        }

        private void Start()
        {
            if(!BB.isSimulating) return;
            if(UseMotorSound)
            {
                
                //SoundSource is defined in Start because that's when UseMotorSound accurately reflects the toggle, unlike in Awake.
                SoundSource = gameObject.AddComponent<AudioSource>();
                SoundSource.playOnAwake = false;
                SoundSource.spatialBlend = 1;
                SoundSource.maxDistance = 150f;
                SoundSource.rolloffMode = AudioRolloffMode.Linear;
                SoundSource.loop = false;
                SoundSource.clip = MSounds[SelectedSound];

                //Mute the sound if time is frozen
                TimeSlider.Instance.onScaleChanged += (float percent) => {if(SoundSource != null) SoundSource.mute = percent <= Single.Epsilon;};

                //Remaining setup is host only
                if(!BB.SimPhysics) return;
                deltaAngle = 0;
                averageAngularVelocityQueue.Clear();

                //Initialize lastrotation and current rotation before the first difference is calculated
                currentRotation = BB.transform.rotation;
                lastRotation = currentRotation;
            }
            else Destroy(this);
        }

        private void FixedUpdate()
        {
            if(!BB.isSimulating || !BB.SimPhysics) return;
            //get current rotation and change from last frame
            currentRotation = BB.transform.rotation;
            deltaAngle = Quaternion.Angle(currentRotation, lastRotation);

            //Add delta angle to the angular velocity queue for averaging
            if (averageAngularVelocityQueue.Count == queueSize) averageAngularVelocityQueue.Dequeue();
            averageAngularVelocityQueue.Enqueue(deltaAngle);

            //find the mean angular velocity of the elements of the queue
            float averageAngularVelocity = 0;
            foreach (float angvel in averageAngularVelocityQueue) averageAngularVelocity += angvel;
            averageAngularVelocity /= averageAngularVelocityQueue.Count;

            lastRotation = currentRotation;

            //Updates only once per updateRate frames
            if (updateCounter == updateRate-1)
            {
                UpdateSound(averageAngularVelocity);
                ModNetworking.SendToAll(mAvgSpeed.CreateMessage(thisblock, averageAngularVelocity));
                updateCounter = 0;
            }
            updateCounter++;
        }

        private void UpdateSound(float avgAngVel)
        {
            try
            {
                if(PlayingSound)
                {
                    //update pitch and volume as the sound continues to play

                    SoundSource.pitch = (float)(Math.Sqrt(avgAngVel / 14) * TimeSlider.Instance.percentagey * pitchMultiplier);
                    SoundSource.volume = (avgAngVel / 14) * startingVolume;

                    //If the spinner slows down too much, stop the sound
                    if(avgAngVel <= 3)
                    {
                        SoundSource.loop = false;
                        SoundSource.Stop();
                        PlayingSound = false;
                    }
                }
                else
                {
                    //Once the spinner speeds up enough, start the sound
                    if(avgAngVel >= 4)
                    {
                        SoundSource.loop = true;
                        SoundSource.Play();
                        PlayingSound = true;
                    }
                }
            }
            catch(NullReferenceException)
            {
                if(SoundSource == null)Modding.ModConsole.Log("SoundSource null");
                if(TimeSlider.Instance == null)Modding.ModConsole.Log("timeSlider null");
            }
        }
    }
}