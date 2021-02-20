using System.Collections.Generic;
using UnityEngine;
using Modding;
using Modding.Blocks;

namespace BotFix
{
    public class WinchFix : MonoBehaviour
    {
        private SpringCode SC;
        private MKey DKey;
        private bool brok = false;

        private void Awake()
        {
            SC = GetComponent<SpringCode>();

            //Mapper definition
            DKey = SC.AddKey("Destroy", "Destroy", KeyCode.X);

            //DisplayInMapper config
            DKey.DisplayInMapper = true;
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (SC.SimPhysics)
                {
                    if (brok)
                        return;

                    if (DKey.IsHeld || DKey.EmulationHeld())
                    {
                        DestroySpringCode();
                    }
                }
            }
        }

        public void DestroySpringCode()
        {
            if (SC.Prefab.Type == BlockType.RopeWinch)
            {
                SC.Snap();
                //Debug.Log("oh snap!");
                brok = true;
            }

            if (SC.Prefab.Type == BlockType.Spring)
            {
                SC.gameObject.SetActive(false);
                brok = true;
            }

            if (StatMaster.isClient || StatMaster.isLocalSim)
                return;
            //Debug.Log("Send play message");
            ModNetworking.SendToAll(Messages.DS.CreateMessage(SC));
            //Debug.Log("NETWORKED oh snap!");
        }

        public static void DestroyClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            BL.InternalObject.GetComponent<WinchFix>().DestroySpringCodeClient();
            //Debug.Log("oh snapped!");
        }

        public void DestroySpringCodeClient()
        {
            if (SC.Prefab.Type == BlockType.RopeWinch)
            {
                SC.Snap();
                brok = true;
            }

            if (SC.Prefab.Type == BlockType.Spring)
            {
                SC.gameObject.SetActive(false);
                brok = true;
            }
        }
    }
}

        


         

        
 
         



           
           
   