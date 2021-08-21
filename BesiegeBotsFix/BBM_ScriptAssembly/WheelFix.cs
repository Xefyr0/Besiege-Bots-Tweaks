using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BotFix
{
    public class Wheelfix : MonoBehaviour
    {
        public BlockBehaviour BB;
        private bool isFirstFrame = true;
        private Rigidbody rigg;
        private MSlider GS;
        private MMenu PCMenu;
        private int PCselect = 3;
        private int ID = 0;
        private float Friction = 0.8f;

        public PhysicMaterialCombine PC = PhysicMaterialCombine.Maximum;

        internal static List<string> PCmenul = new List<string>()
        {
            "Average",
            "Multiply",
            "Minimum",
            "Maximum",
        };

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.isSimulating)
                {
                    if (isFirstFrame)
                    {
                        isFirstFrame = false;
                        foreach (Collider c in GetComponentsInChildren<Collider>())
                        {
                            c.material.dynamicFriction = Friction;
                            c.material.staticFriction = Friction;
                            c.material.frictionCombine = PC;
                        }
                    }
                }
            }
        }
     
        private void Awake()
        {
            //Get stuff
            BB = GetComponent<BlockBehaviour>();

            ID = BB.BlockID;

            //Mapper definition
            GS = BB.AddSlider("Friction", "Friction", Friction, 0.1f, 10f);
            GS.ValueChanged += (float value) => { Friction = value; };

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

            //DisplayInMapper config
            GS.DisplayInMapper = true;
            PCMenu.DisplayInMapper = true;
            /*
            //Physics stuff
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                rigg = GetComponent<Rigidbody>();
                rigg.drag = 0f;
                rigg.angularDrag = 0f;
                rigg.maxAngularVelocity = 100;
            }    
            */
        }
    }
}
