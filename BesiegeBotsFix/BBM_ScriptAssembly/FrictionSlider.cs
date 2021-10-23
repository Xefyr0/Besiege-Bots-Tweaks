/*
FrictionSlider.cs
Written by Xefyr for the Besiege Bots community
Adapted from the work of DokterDoyle
*/

using UnityEngine;
using System.Collections.Generic;

namespace BesiegeBotsTweaks
{
    public class FrictionSlider : MonoBehaviour
    {
        private const byte FRAMECOUNT = 3;  //The number of frames this component waits before making changes
        private byte frameCounter = 0;  //frameCounter Variable to keep track of how many frames have elapsed
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
        
        void Awake()
        {
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

        private void Update()
        {
            //Wait until sim starts, then starts counting frames until FRAMECOUNT frames are reached
            if(!BB.SimPhysics) return;
            frameCounter++;
            if(frameCounter < FRAMECOUNT) return;

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
            Object.Destroy(this);   //The component instance is destroyed after the necessary changes are made.
        }
    }
}