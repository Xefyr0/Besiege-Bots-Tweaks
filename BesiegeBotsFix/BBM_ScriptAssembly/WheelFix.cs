using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BotFix
{
    public class Wheelfix : MonoBehaviour
    {
        private HingeJoint HJ;
        private HingeJoint CJ;
        private JointMotor JM;
        private Collider[] colliders;
        private MSlider GS;
        private MMenu PCMenu;
        private int PCselect = 3;
        public BlockBehaviour BB { get; internal set; }
        public float grip = 1;
        private Rigidbody rigg;
        
        public PhysicMaterialCombine PC = PhysicMaterialCombine.Maximum;
        
        internal static List<string> PCmenul = new List<string>()
        {
            "Average",
            "Multiply",
            "Minimum",
            "Maximum",
        };
     
        private void Awake()
        {          
            BB = GetComponent<BlockBehaviour>();
            

            GS = BB.AddSlider("Friction", "Friction", grip, 0.1f, 10f);
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
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;

                CJ = GetComponent<HingeJoint>();
                CJ.breakForce = 60000;
                CJ.breakTorque = 60000;

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
