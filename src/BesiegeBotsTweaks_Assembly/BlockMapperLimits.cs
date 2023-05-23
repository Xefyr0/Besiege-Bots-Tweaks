/*
 * BlockMapperLimits.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * This class changes the sliders/menus/toggles of a block mapper.
 * The changes depend on the block this is attached to.
 * For the most part, this changes the limits of sliders to eliminate the need for the NoBounds mod
 * and eliminate rules that would limit its use in turn.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(BlockBehaviour))]
    class BlockMapperLimits : MonoBehaviour
    {
        private void Start()
        {
            BlockBehaviour BB = GetComponent<BlockBehaviour>();
            //Add Steering Block angle limits
            if(BB is SteeringWheel)
            {
                ((SteeringWheel)BB).allowLimits = true;
                if (BB.BlockID == ((int)BlockType.SteeringBlock) && !Modding.Game.IsSimulating && ((SteeringWheel)BB).LimitsSlider != null)
                {
                    FauxTransform fauxTransform = new FauxTransform(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f), Vector3.one * 0.35f);
                    (BB as SteeringWheel).LimitsSlider.UseLimitsToggle.IsActive = false;
                    (BB as SteeringWheel).LimitsSlider.UseLimitsToggle.ApplyValue();
                    (BB as SteeringWheel).LimitsSlider.iconInfo = fauxTransform;
                }
            }
            //Increase the max speed of flying blocks to 2.0
            if(BB is FlyingController)
            {
                (BB as FlyingController).SpeedSlider.SetRange((BB as FlyingController).SpeedSlider.Min, 2.0f);
            }
            //Increase max speed of Rope Winches to 10.0
            if(BB is SpringCode)
            {
                (BB as SpringCode).SpeedSlider.SetRange((BB as SpringCode).SpeedSlider.Min, 10.0f);
            }
            //Reduce max power of water cannons to 0.5f
            if(BB is WaterCannonController)
            {
                (BB as WaterCannonController).StrengthSlider.SetRange((BB as WaterCannonController).StrengthSlider.Min, 0.5f);
                (BB as WaterCannonController).StrengthSlider.SetValue(0.5f);
            }
            //Motor tweaks
            if(BB is CogMotorControllerHinge)
            {
                //Reduce minimum acceleration for motors to 0.0f
                (BB as CogMotorControllerHinge).AccelerationSlider.SetRange(0.0f, (BB as CogMotorControllerHinge).AccelerationSlider.Max);
                //Increase maximum speed for motors to 12.0f for spinner motor speeds
                //(BB as CogMotorControllerHinge).SpeedSlider.SetRange((BB as CogMotorControllerHinge).SpeedSlider.Min, 12.0f);
            }
            Destroy(this);
        }
    }
}