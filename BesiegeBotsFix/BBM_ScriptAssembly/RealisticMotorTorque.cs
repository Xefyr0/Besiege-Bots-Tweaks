/*
RealisticMotorTorque.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BotFix
{
    class RealisticMotorTorque : MonoBehaviour
    {
        private CogMotorControllerHinge CMCH;
        private HingeJoint myJoint;
        private JointMotor motor;
        private bool firstFrame = true;


        //The below vars are used in the code ripped from CogMotorControllerHinge.
        private int FlipInvert => CMCH.Flipped ? 1 : (-1);
        private float deltaMultiplier => CMCH.degreesPerSecond * 80f * (float)(-FlipInvert);
        private float lastVelocity;
        private void FixedUpdate()
        {
            //If the block isn't host or SP or local sim, return to save frames.
            if (StatMaster.isClient && !Modding.Game.IsSetToLocalSim) return;

            //Init stuff. The joint and CMCH.motor don't exist until vanilla besiege does init stuff so it goes here instead of Awake.
            if(firstFrame)
            {
                CMCH = GetComponent<CogMotorControllerHinge>();
                myJoint = GetComponent<HingeJoint>();
                myJoint.useMotor = true;
                motor = CMCH.motor;
                motor.force = CMCH.AccelerationSlider.Value;
                myJoint.motor = motor;
                firstFrame = false;
                CMCH._parentMachine.UnregisterFixedUpdate(CMCH, isBuild: false);
            }

            //If not simulating, return to save frames.
            if(!CMCH.isSimulating) return;

            //Method variables for computing motor's targetVelocity each frame
            float Velocity = CMCH.Input * CMCH.SpeedSlider.Value;
            float maxVelocity = Velocity * deltaMultiplier;
		    float targetVelocity = lastVelocity + (maxVelocity - lastVelocity) * Time.fixedDeltaTime * CMCH.speedLerpSmooth;
		    
            //This code is magic, right down to the 0.01 number. No clue why it works, 
            //but it stops motors from braking in either direction when autobrake is turned off.
            if (CMCH.allowControl && !CMCH.AutoBreakToggle.IsActive && System.Math.Abs(CMCH.Input) < 0.05f)
		    {
                float num = 0.01f;
			    if (lastVelocity > 0f) targetVelocity = Mathf.Min(targetVelocity, num);
			    else if (lastVelocity < 0f) targetVelocity = Mathf.Max(targetVelocity, -num);
		    }

            //If the wheel is accelerating or decelerating, wake up the rigidbodies and update lastVelocity and the motor.
		    if (System.Math.Abs(targetVelocity-lastVelocity) > Mathf.Epsilon)
		    {
			    if (!CMCH.noRigidbody && CMCH.Rigidbody.IsSleeping()) CMCH.Rigidbody.WakeUp();
			    if (myJoint.connectedBody.IsSleeping()) myJoint.connectedBody.WakeUp();
			    motor.targetVelocity = lastVelocity = targetVelocity;
			    myJoint.motor = motor;
		    }
        }
    }
}