/*
RealisticMotorTorque.cs
Written by Xefyr for the Besiege Bots community
*/

using UnityEngine;
using Modding;
using Modding.Common;
using Modding.Blocks;

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
            //Basic gets
            CMCH = GetComponent<CogMotorControllerHinge>();
            myJoint = GetComponent<HingeJoint>();
            Block block = Block.From(CMCH);

            if(CMCH.SimPhysics)
            {
                //The Enumerator is only meant to be executed:
                //1. On the local instance (all instances) in Singleplayer
                //2. As host on the local instance
                //3. As host on the non-local instances if they're not in local sim
                //4. As client on the local instance if we're in local sim
                if (Player.GetHost() == null || (block.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !block.Machine.Player.InLocalSim)) StartCoroutine(Init());
                else Destroy(this);
            }
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
            //CMCH.motor doesn't exist and UnregisterFixedUpdate doesn't work until a frame into sim, so this waits for that to happen.
            yield return new WaitForFixedUpdate();

            //Motor init
            myJoint.useMotor = true;
            motor = CMCH.motor;
            motor.force = CMCH.AccelerationSlider.Value;
            myJoint.motor = motor;

            //All of the acceleration jank vanilla behaviour has relies on BlockFixedUpdate.
            //This line "unregisters" the block this Component is attached to, so the method isn't called.
            CMCH._parentMachine.UnregisterFixedUpdate(CMCH, isBuild: false);
        }
    }
}