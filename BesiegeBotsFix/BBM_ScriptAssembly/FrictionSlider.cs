using UnityEngine;
using System.Collections.Generic;

namespace BesiegeBotsTweaks
{
    public class FrictionSlider : MonoBehaviour
    {
        private Collider[] colliders;
        private MSlider GS;
        private MMenu PCMenu;
        private int PCselect = 0;
        public BlockBehaviour BB { get; internal set; }

        public float grip = 0.3f;
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
            BB = GetComponent<BlockBehaviour>();

            GS = BB.AddSlider("Friction", "Friction", grip, 0.1f, 4f);
            GS.ValueChanged += (float value) => { grip = value; };

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

            GS.DisplayInMapper = true;
            PCMenu.DisplayInMapper = true;
          
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.material.dynamicFriction = grip;
                    collider.material.staticFriction = grip;
                    collider.material.frictionCombine = PC;
                }
            }
        }
    }
}