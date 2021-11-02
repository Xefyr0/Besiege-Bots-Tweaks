/*
RealisticMotorTorque.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;

namespace BotFix
{
    [RequireComponent(typeof(CogMotorControllerHinge))]
    class RealisticMotorTorque : MonoBehaviour
    {
        private CogMotorControllerHinge CMCH;
        private HingeJoint myJoint;
        private JointMotor motor;

        //The below vars are used in the code ripped from CogMotorControllerHinge.
        private int FlipInvert => CMCH.Flipped ? 1 : (-1);
        private float deltaMultiplier => CMCH.degreesPerSecond * 80f * (float)(-FlipInvert);
        private float lastVelocity;
        private void Awake()
        {
            //For some reason, this init is called twice. Once for the building block, once for the simblock.
            //Other Awakes are normally called only for the building block.
            if(Modding.Game.IsSimulating)
            {
                CMCH = GetComponent<CogMotorControllerHinge>();
                myJoint = GetComponent<HingeJoint>();
                StartCoroutine(Init());
            }
        }
        private void FixedUpdate()
        {
            //If the block isn't in sim, wait until it is.
            if (!Modding.Game.IsSimulating) return;
            
            //If the block is in sim but there's no joint or it's a server client, then it is of no use.
            if(myJoint == null || (StatMaster.isClient && !Modding.Game.IsSetToLocalSim))
            {
                Destroy(this);
                return;
            }
            //Method variables for computing motor's targetVelocity each frame
            float Velocity = CMCH.Input * CMCH.SpeedSlider.Value;
            float maxVelocity = Velocity * deltaMultiplier;
		    float targetVelocity = lastVelocity + (maxVelocity - lastVelocity) * Time.fixedDeltaTime * CMCH.speedLerpSmooth;
            
            //This code is magic, right down to the 0.01 number. No clue why it works, 
            //but it stops motors from braking in either direction when autobrake is turned off and acceleration is set to infinity
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
			    if (myJoint.connectedBody != null && myJoint.connectedBody.IsSleeping()) myJoint.connectedBody.WakeUp();
			    motor.targetVelocity = lastVelocity = targetVelocity;
			    myJoint.motor = motor;
		    }
        }
        private System.Collections.IEnumerator Init()
        {
            //Things like myJoint and CMCH.motor don't exist until a frame into sim, so this waits until that happens.
            while(!Modding.Game.IsSimulating) yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            myJoint.useMotor = true;
            motor = CMCH.motor;
            motor.force = CMCH.AccelerationSlider.Value;
            myJoint.motor = motor;
            CMCH._parentMachine.UnregisterFixedUpdate(CMCH, isBuild: false);
        }
    }
}