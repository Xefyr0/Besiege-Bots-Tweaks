/*
 * RealisticMotorTorque.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * In Besiege, most blocks that spin can easily supply lots of torque.
 * For simulating robot combat however, this resulted in bots
 * that had spinners with much more torque than their real-life counterparts.
 * I created this class to provide a limit on torque as an alternative to the spin up time slider.
 * It never got adopted as part of the ruleset, but remains as an option to builders.
 */

using Modding.Blocks;
using Modding.Common;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(CogMotorControllerHinge))]
    class RealisticMotorTorque : FrameDelayAction
    {
        private CogMotorControllerHinge CMCH;
        private HingeJoint myJoint;
        private JointMotor motor;

        //The below vars are used in the code ripped from CogMotorControllerHinge.
        private int FlipInvert => CMCH.Flipped ? 1 : (-1);
        private float DeltaMultiplier => CMCH.degreesPerSecond * 80f * (float)(-FlipInvert);
        private float lastVelocity;

        protected override int FRAMECOUNT { get; } = 1;
        new void Awake()
        {
            base.Awake();

            //Basic gets
            CMCH = GetComponent<CogMotorControllerHinge>();
            myJoint = GetComponent<HingeJoint>();
            Block block = Block.From(CMCH);
        }
        private void FixedUpdate()
        {
            //Wait until the block in question is simulating.
            if (!CMCH.SimPhysics) return;
            
            //If the joint was destroyed or never existed in the first place, then destroy this component.
            if(myJoint == null)
            {
                Destroy(this);
                return;
            }

            //Method variables for computing motor's targetVelocity each frame
            float Velocity = CMCH.Input * CMCH.SpeedSlider.Value;
            float maxVelocity = Velocity * DeltaMultiplier;
		    float targetVelocity = lastVelocity + (maxVelocity - lastVelocity) * Time.fixedDeltaTime * CMCH.speedLerpSmooth;
            
            //This block of code is magic, right down to the 0.01 number. No clue why it works, 
            //but it stops motors from braking in either direction when autobrake is turned off and acceleration is set to infinity
            if (CMCH.allowControl && !CMCH.AutoBreakToggle.IsActive && System.Math.Abs(CMCH.Input) < 0.05f)
		    {
                float num = 0.01f;
			    if (lastVelocity > 0f) targetVelocity = Mathf.Min(targetVelocity, num);
			    else if (lastVelocity < 0f) targetVelocity = Mathf.Max(targetVelocity, -num);
		    }
            
            //If the wheel is accelerating or decelerating, wake up the rigidbodies and update lastVelocity and the joint motor.
		    if (System.Math.Abs(targetVelocity-lastVelocity) > Mathf.Epsilon)
		    {
			    if (!CMCH.noRigidbody && CMCH.Rigidbody.IsSleeping()) CMCH.Rigidbody.WakeUp();
			    if (myJoint.connectedBody != null && myJoint.connectedBody.IsSleeping()) myJoint.connectedBody.WakeUp();
			    motor.targetVelocity = lastVelocity = targetVelocity;
			    myJoint.motor = motor;
		    }
        }
        protected override void DelayedAction()
        {
            //CMCH.motor doesn't exist and UnregisterFixedUpdate doesn't work until a frame into sim.

            //Motor init
            myJoint.useMotor = true;
            motor = CMCH.motor;
            motor.force = CMCH.AccelerationSlider.Value;
            myJoint.motor = motor;

            //All of the jank vanilla behaviour relies on BlockFixedUpdate to take effect.
            //This line "unregisters" the block this Component is attached to, so the method isn't called.
            CMCH._parentMachine.UnregisterFixedUpdate(CMCH, isBuild: false);
        }
    }
}