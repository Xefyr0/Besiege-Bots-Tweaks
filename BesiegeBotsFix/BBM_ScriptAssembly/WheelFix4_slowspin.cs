using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Collections;

namespace BotFix
{
    public class Wheelfix4 : MonoBehaviour
    {
        public BlockBehaviour BB;
        private Rigidbody rigg;
        private bool isFirstFrame = true;

        private MKey newforward;
        private MKey newbackward;
        private MSlider spinuptime;
        private MSlider speedslider;
        private float stime = 0;
        private float speed = 0;

        private HingeJoint HJ;
        private JointMotor JM;

        // //////////////////////////////////////////////////////////
        public bool allowControl = true;
        public float degreesPerSecond = 1f;
        public float maxAngularVel = 50f;
        public float minAcc = 0.1f;
        public float maxAcc = 50f;
        public float defaultAcc = float.PositiveInfinity;
        public float accInfinity = 600f;
        public float defaultSpeed = 1f;
        public float maxSpeed = 2f;
        public float speedLerpSmooth = 26f;
        public float Velocity;
        public bool startOn;
        public bool enableLimits;
        public float minSpeed;
        public JointMotor motor;
        protected MLimits limitsSlider;
        private float input;
        private MToggle toggleMode;
        private MToggle autoBreakMode;
        public MSlider speedSlider;
        private MSlider accSlider;
        private bool isUsingMotor;
        private float deltaMultiplier;
        private float lastVelocity;
        private JointSpring mySpring;
        private bool hasStarted;
        private bool forwardPressed;
        private bool backwardPressed;
        private bool forwardHeld;
        private bool backwardHeld;
        private bool emuForwardPressed;
        private bool emuBackwardPressed;
        private bool emuForwardHeld;
        private bool emuBackwardHeld;
        private bool forceReset;

        //////////////////////////////////////////////





        private void Awake()
        {
            //Get stuff
            BB = GetComponent<BlockBehaviour>();

            HJ = GetComponent<HingeJoint>();
            JM.force = Mathf.Infinity;
            JM.targetVelocity = speed;
            JM.freeSpin = true;
            HJ.motor = JM;
            HJ.useMotor = false;


            //Mapper definition
            newforward = BB.AddKey("Nforwards", "Nforwards", KeyCode.UpArrow);

            newbackward = BB.AddKey("Nbackwards", "backwards", KeyCode.DownArrow);

            spinuptime = BB.AddSlider("spinuptime", "stime", stime, 0f, 10f);
            spinuptime.ValueChanged += (float value) => { stime = value; };

            speedslider = BB.AddSlider("speed", "speed", speed, 0f, 10f);
            speedslider.ValueChanged += (float value) => { speed = value; };


            //DisplayInMapper config
            newforward.DisplayInMapper = true;
            newbackward.DisplayInMapper = true;
            spinuptime.DisplayInMapper = true;
            speedslider.DisplayInMapper = true;

            if (allowControl)
            {
                toggleMode = BB.AddToggle(2431, "toggle-mode", false);
                autoBreakMode = BB.AddToggle(2432, "auto-brake", true);

            }
            speedSlider = BB.AddSlider(2428, "speed", defaultSpeed, minSpeed, !StatMaster.UnlockSpeedSliders ? maxSpeed : 1000f, string.Empty, "x");
            accSlider = BB.AddSlider(2436, "acceleration", defaultAcc, minAcc, maxAcc, string.Empty, "x");
            accSlider.maxInfinity = true;
            speedSlider.ValueChanged += new ValueChangeHandler(checkForInfinityValue);



















            //Physics stuff
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 1000;
            }
        }

        void Start()
        {
            if (!BB.isSimulating || !BB.SimPhysics)
                return;
            motor = HJ.motor;
            motor.force = accSlider.Value != maxAcc ? accSlider.Value : float.PositiveInfinity;
            motor.targetVelocity = 600f;
            motor.freeSpin = autoBreakMode != null && !autoBreakMode.IsActive;
            HJ.motor = motor;
            HJ.useMotor = false;
            isUsingMotor = false;
            motor = HJ.motor;
            motor.targetVelocity = 0.0f;
            HJ.motor = motor;
        }

        private void checkForInfinityValue(float newValue)
        {
            if (newValue != maxAcc)
                return;
            accSlider.Value = float.PositiveInfinity;
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.isSimulating)
                {
                    if (isFirstFrame)
                    {
                        isFirstFrame = false;
                        JM.force = Mathf.Infinity;
                        JM.targetVelocity = speed;
                        JM.freeSpin = true;
                        HJ.motor = JM;
                    }


                    if (allowControl)
                    {
                        forwardPressed = newforward.IsPressed;
                        backwardPressed = newbackward.IsPressed;
                        forwardHeld = newforward.IsHeld;
                        backwardHeld = newbackward.IsHeld;
                        CheckKeys(forwardPressed, backwardPressed, forwardHeld, backwardHeld, newforward.Value, -newbackward.Value, emuForwardHeld, emuBackwardHeld);
                    }
                    else if (HJ == null || rigg && rigg.isKinematic && ((HJ.connectedBody) && HJ.connectedBody.isKinematic))
                        input = 0.0f;
                    else
                        input = 1f;
                }
            }
        }

        void FixedUpdate()
        {
            if (!hasStarted)
            {
                if (rigg)
                    rigg.maxAngularVelocity = maxAngularVel;
                deltaMultiplier = degreesPerSecond * 80f;
                hasStarted = true;
            }
            if (HJ == null || HJ.connectedBody == null)
                return;
            Velocity = input * speedSlider.Value;
            if (input == 0.0)
            {
                motor.force = float.PositiveInfinity;
                forceReset = true;
            }
            else
            {
                if (motor.force == double.PositiveInfinity && forceReset)
                {
                    motor.force = accSlider.Value;
                    forceReset = false;
                }
                if (motor.force != double.PositiveInfinity && !forceReset)
                {
                    motor.force += (float)(accSlider.Value * Time.deltaTime * 10.0);
                    if (motor.force > accInfinity)
                        motor.force = float.PositiveInfinity;
                    HJ.motor = motor;
                }
            }
            if (!isUsingMotor)
            {
                HJ.useMotor = true;
                isUsingMotor = true;
            }
            float num1 = lastVelocity + (Velocity * deltaMultiplier - lastVelocity) * Time.deltaTime * speedLerpSmooth;
            if (allowControl && !autoBreakMode.IsActive && (input >= 0.0 ? input : -input) < 0.0500000007450581)
            {
                float num2 = 0.01f;
                if (lastVelocity > 0.0)
                    num1 = num1 <= num2 ? num1 : num2;
                else if (lastVelocity < 0.0)
                    num1 = num1 <= -num2 ? -num2 : num1;
            }
            float num3 = num1 - lastVelocity;
            if ((num3 >= 0.0 ? num3 : -num3) <= Mathf.Epsilon)
                return;
            if (rigg && rigg.IsSleeping())
                rigg.WakeUp();
            if (HJ.connectedBody.IsSleeping())
                HJ.connectedBody.WakeUp();
            motor.targetVelocity = num1;
            lastVelocity = num1;
            HJ.motor = motor;
        }

        protected void CheckKeys(bool forwardPress, bool backwardPress, bool forwardHeld, bool backwardHeld, float forwardVal, float backwardVal, bool altForwardHeld, bool altBackwardHeld)
        {
            if (HJ == null || rigg && rigg.isKinematic && ((HJ.connectedBody) && HJ.connectedBody.isKinematic))
                input = 0.0f;
            else if (toggleMode.IsActive)
            {
                if (forwardPress)
                    input = input <= 0.899999976158142 ? 1f : 0.0f;
                if (!backwardPress)
                    return;
                if (input < -0.899999976158142)
                    input = 0.0f;
                else
                    input = -1f;
            }
            else if (forwardHeld)
            {
                input = forwardVal;
            }
            else
            {
                if (altForwardHeld)
                    return;
                if (backwardHeld)
                {
                    input = backwardVal;
                }
                else
                {
                    if (altBackwardHeld)
                        return;
                    input = 0.0f;
                }
            }
        }

    }
}

