/*
Doyle's betamode motor code. The realistic acceleration and torque this provided was an inspiration to
create RealisticMotorTorque.cs, which is more compatible with vanilla behaviour.
The class, however, remains unused as legacy code.
*/
using UnityEngine;

namespace BotFix
{
    public class SMotor2 : MonoBehaviour
    {
        private MKey FK;
        private MKey BK;
        //private MSlider SpeedSlider;
        private MSlider TorqueSliderx;
        private MSlider TorqueSlidery;
        private MSlider TorqueSliderz;
        private MToggle SpinnerToggle;

        public bool SpinnerMode = false;
        //public float Speed = 100;
        public float Torquex = 0;
        public float Torquey = 0;
        public float Torquez = 0;
        private Rigidbody rigg;
        private Rigidbody target;
        public BlockBehaviour BB { get; internal set; }
        private HingeJoint HJ;
        private bool isFirstFrame = true;


        void Awake()
        {
            //Init sliders/keys/etc.
            BB = GetComponent<BlockBehaviour>();

            FK = BB.AddKey("Forward", "Forward", KeyCode.UpArrow);
            BK = BB.AddKey("Backward", "Backward", KeyCode.DownArrow);

            SpinnerToggle = BB.AddToggle("SpinnerMode", "SpinnerMode", SpinnerMode);
            SpinnerToggle.Toggled += (bool value) =>
            {
                SpinnerMode = FK.DisplayInMapper = BK.DisplayInMapper = /*SpeedSlider.DisplayInMapper  =*/ TorqueSliderx.DisplayInMapper = TorqueSlidery.DisplayInMapper = TorqueSliderz.DisplayInMapper = value;
            };

            //SpeedSlider = BB.AddSlider("Speed", "Speedslider", Speed, 0f, 100f);
            //SpeedSlider.ValueChanged += (float value) => { Speed = value*100; };

            TorqueSliderx = BB.AddSlider("Torquex", "Torquesliderx", Torquex, -10f, 10f);
            TorqueSliderx.ValueChanged += (float value) => { Torquex = value*1000; };

            TorqueSlidery = BB.AddSlider("Torquey", "Torqueslidery", Torquey, -10f, 10f);
            TorqueSlidery.ValueChanged += (float value) => { Torquey = value*1000; };

            TorqueSliderz = BB.AddSlider("Torquez", "Torquesliderz", Torquez, -10f, 10f);
            TorqueSliderz.ValueChanged += (float value) => { Torquez = value*1000; };


            FK.DisplayInMapper = SpinnerMode;
            BK.DisplayInMapper = SpinnerMode;
            //SpeedSlider.DisplayInMapper = SpinnerMode;
            TorqueSliderx.DisplayInMapper = SpinnerMode;
            TorqueSlidery.DisplayInMapper = SpinnerMode;
            TorqueSliderz.DisplayInMapper = SpinnerMode;
            SpinnerToggle.DisplayInMapper = true;

            //init hingejoint
            HJ = GetComponent<HingeJoint>();      

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 500;
            }
        }

        void FixedUpdate()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (SpinnerMode)
                {
                    if (BB.isSimulating)
                    {
                        if (FK.IsHeld || FK.EmulationHeld())
                        {
                            if (isFirstFrame)
                            {
                                isFirstFrame = false;
                                target = HJ.connectedBody;
                            }

                            target.AddRelativeTorque(Torquex, Torquey, Torquez, ForceMode.Acceleration);
                        }

                        if (BK.IsHeld || BK.EmulationHeld())
                        {
                            if (isFirstFrame)
                            {
                                isFirstFrame = false;
                                target = HJ.connectedBody;
                            }
                            target.AddRelativeTorque(-Torquex, -Torquey, -Torquez, ForceMode.Acceleration);
                        }
                    }
                }
            }
        }
    }
}

