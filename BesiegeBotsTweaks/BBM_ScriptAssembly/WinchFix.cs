using System.Collections.Generic;
using UnityEngine;
using Modding;
using Modding.Blocks;

namespace BotFix
{
    public class WinchFix : MonoBehaviour
    {
        private SpringCode SC;
        private MKey DestroyKey;
        private bool brok = false;

        private MessageType mDestroySpring = ModNetworking.CreateMessageType(DataType.Block);
        private Message DS;

        private void Awake()
        {
            SC = GetComponent<SpringCode>();

            //Mapper definition
            DestroyKey = SC.AddKey("Destroy", "Destroy", KeyCode.X);
            DestroyKey.DisplayInMapper = true;

            ModNetworking.Callbacks[mDestroySpring] += (System.Action<Message>)delegate(Message m) {((Block)m.GetData(0)).InternalObject.GetComponent<WinchFix>().DestroySpring();};
            DS = mDestroySpring.CreateMessage(SC);
        }

        void Update()
        {
            //If block isn't in sim then it shouldn't do anything.
            if (!SC.SimPhysics) return;

            if (DestroyKey.IsHeld || DestroyKey.EmulationHeld())
            {
                ModNetworking.SendToAll(DS);
                DestroySpring();
            }
        }

        public void DestroySpring()
        {
            //If the block this Component is attached to is a rope, snap it.
            if (SC.Prefab.Type == BlockType.RopeWinch) SC.Snap();
            //If the block this Component is attached to is a spring, disable it.
            if (SC.Prefab.Type == BlockType.Spring) SC.gameObject.SetActive(false);
            //WinchFix does nothing after the the snap, so it can be destroyed.
            Destroy(this);
        }
    }
}

        


         

        
 
         



           
           
   