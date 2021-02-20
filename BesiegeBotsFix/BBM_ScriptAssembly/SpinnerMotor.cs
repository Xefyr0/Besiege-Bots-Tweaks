using UnityEngine;
using Modding.Blocks;

namespace BotFix
{
    public class SMotor : MonoBehaviour
    {
        private MKey FK;
        private MKey BK;
        private MSlider SpeedSlider;
        private MSlider TorqueSliderx;
        private MSlider TorqueSlidery;
        private MSlider TorqueSliderz;
        private MToggle SpinnerToggle;

        public bool SpinnerMode = false;
        public float Speed = 100;
        public float Torquex = 0;
        public float Torquey = 0;
        public float Torquez = 0;
        private JointMotor motor;

        private Rigidbody rigg;
        private Rigidbody target;
        private Block blocc;
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
                SpinnerMode = FK.DisplayInMapper = BK.DisplayInMapper = SpeedSlider.DisplayInMapper = TorqueSliderx.DisplayInMapper = TorqueSlidery.DisplayInMapper = TorqueSliderz.DisplayInMapper = value;
            };

            SpeedSlider = BB.AddSlider("Speed", "Speedslider", Speed, 0f, 100f);
            SpeedSlider.ValueChanged += (float value) => { Speed = value*100; };

            TorqueSliderx = BB.AddSlider("Torquex", "Torquesliderx", Torquex, 0f, 100f);
            TorqueSliderx.ValueChanged += (float value) => { Torquex = value*10; };

            TorqueSlidery = BB.AddSlider("Torquey", "Torqueslidery", Torquey, 0f, 100f);
            TorqueSlidery.ValueChanged += (float value) => { Torquey = value * 10; };

            TorqueSliderz = BB.AddSlider("Torquez", "Torquesliderz", Torquez, 0f, 100f);
            TorqueSliderz.ValueChanged += (float value) => { Torquez = value * 10; };

            FK.DisplayInMapper = SpinnerMode;
            BK.DisplayInMapper = SpinnerMode;
            SpeedSlider.DisplayInMapper = SpinnerMode;
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
            if (SpinnerMode)
            {
                if (BB.isSimulating)
                {
                    if (FK.IsPressed || FK.EmulationPressed())
                    {
                        if (isFirstFrame)
                        {
                            isFirstFrame = false;
                            target = HJ.connectedBody;
                        }
                        
                        target.AddRelativeTorque(Torquex, Torquey, Torquez, ForceMode.Acceleration);
                    }
                    
                    if (BK.IsPressed || BK.EmulationPressed())
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

