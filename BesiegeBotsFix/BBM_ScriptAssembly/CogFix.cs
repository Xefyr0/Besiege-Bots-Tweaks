using UnityEngine;

namespace BotFix
{
    public class Cogfix : MonoBehaviour
    {
        private HingeJoint HJ;
        private BlockBehaviour BB;
        private bool firstframe = false;
        private int fcounter;

        public CogMotorControllerHinge CCH;
        private float Lerpo = 0f;
        private MSlider Lerpomode;

        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            CCH = GetComponent<CogMotorControllerHinge>();
            Lerpomode = BB.AddSlider("Spin up time", "Lerp", Lerpo, 0f, 10f);
            //Lerpomode.ValueChanged += (float value) => { Lerpo = value; CCH.speedLerpSmooth = value; };
            Lerpomode.ValueChanged += (float value) =>
                {
                    Lerpo = -0.1f + (26.24517f + 0.4575599f) / Mathf.Pow(1f + (value / 2.265289f), 3.291725f);
                    CCH.speedLerpSmooth = Lerpo;
                    //Debug.Log(Lerpo);
                };

            Lerpomode.DisplayInMapper = true;


            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (!GetComponent<HingeJoint>())
                    return;
                HJ = GetComponent<HingeJoint>();

            }
        }

        void Update()
        {
            if (BB.SimPhysics)
            {
                if (!StatMaster.isClient || StatMaster.isLocalSim)
                {
                    if (!Mod.ModbotMode)
                    {
                        if (!firstframe)
                        {
                            if (!HJ)
                            {
                                firstframe = true;
                                return;
                            }
                            fcounter++;
                            //HJ.breakForce = 90000;
                            //HJ.breakTorque = 90000;
                            if (fcounter == 4)
                                firstframe = true;
                        }
                    }
                }
            }           
        }
    }
}


