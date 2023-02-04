/*
 * FrictionSlider.cs
 * Written by Xefyr for the Besiege Bots community
 * Adapted from the work of DokterDoyle
 * 
 * This class adds to the block mapper
 * a slider that modifies the friction and menu that modifies the friction combine
 */

using System.Collections.Generic;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    class FrictionSlider : FrameDelayAction
    {
        private MSlider GS;
        private MMenu PCMenu;
        private int PCselect = 0;
        private BlockBehaviour BB;
        public float grip = 0.7f;
        public PhysicMaterialCombine PC = PhysicMaterialCombine.Average;
        internal static List<string> PCmenul = new List<string>()
        {
            "Average",
            "Multiply",
            "Minimum",
            "Maximum",
        };

        protected override int FRAMECOUNT { get; } = 3;

        new void Awake()
        {
            base.Awake();

            //BB is initialized in Awake instead of Update because otherwise GetComponent would be called multiple times.
            BB = GetComponent<BlockBehaviour>();
            if(BB == null) Object.Destroy(this);

            //Create friction slider and define its ValueChanged event handler to modify an internal variable.
            GS = BB.AddSlider("Friction", "Friction", grip, 0.1f, 4f);
            GS.ValueChanged += (float value) => { grip = value; };

            //Create friction menu and define its ValueChanged event handler to modify an internal variable.
            PCMenu = BB.AddMenu("Combine", PCselect, PCmenul, false);
            PCMenu.ValueChanged += (ValueHandler)(value => 
            {
                switch (value)
                {
                    case 0:
                        PC = PhysicMaterialCombine.Average;
                        break;
                    case 1:
                        PC = PhysicMaterialCombine.Multiply;
                        break;
                    case 2:
                        PC = PhysicMaterialCombine.Minimum;
                        break;
                    case 3:
                        PC = PhysicMaterialCombine.Maximum;
                        break;
                }
            });

            //Forces the new menu elements to display in the mapper tool.
            GS.DisplayInMapper = true;
            PCMenu.DisplayInMapper = true;
        }

        protected override void DelayedAction()
        {
            //If the block isn't actually part of a simulation (i.e. on a client computer in multiverse with local sim turned off) then the component instance is destroyed since it won't do anything either way
            if (StatMaster.isClient && !StatMaster.isLocalSim) Object.Destroy(this);

            //Gets the colliders from each block, and modifies those colliders' friction and frictionCombine to the slider and menu values.
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.material.dynamicFriction = grip;
                collider.material.staticFriction = grip;
                collider.material.frictionCombine = PC;
            }
        }
    }
}