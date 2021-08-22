using UnityEngine;

namespace BotFix
{
    public class Shfix : MonoBehaviour
    {
        private ConfigurableJoint CJ;
        private Rigidbody rigg;
        
        private SteeringWheel BB;
        public float Speed = 1;
        public MSlider Sslider;

        private void Awake()
        {
            BB = GetComponent<SteeringWheel>();
            /*
            SoundMenu = block.AddMenu("Firesound", selectedsound, FireSound, false);
            SoundMenu.ValueChanged += (ValueHandler)(value => { selectedsound = value; });
            SoundMenu.DisplayInMapper = true;
            */           
            //Sslider = BB.AddSlider("Modded Speed", "MSpeed", Speed, 0f, 3f);
            //Sslider.ValueChanged += (float value) => { BB.SpeedSlider.Value = value; };
            //Sslider.DisplayInMapper = true;          
        }
    
        private void Start()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponent<ConfigurableJoint>();
                rigg = GetComponent<Rigidbody>();
                CJ.breakForce = 30000;
                CJ.breakTorque = 30000;
                //rigg.mass = 0.4f;
                /*
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
                */
            }
        }
    }
}



