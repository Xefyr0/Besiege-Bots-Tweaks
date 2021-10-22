/*
AxialDragToggle.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/

using UnityEngine;

namespace BotFix
{
    public class WFix : MonoBehaviour
    {
        public AxialDrag AD;
        private MToggle ADtoggle;
        void Awake()
        {
            AD = GetComponent<AxialDrag>();

            //Mapper definition
            ADtoggle = AD.AddToggle("Disable Drag", "Disable drag", false);
            ADtoggle.Toggled += (bool value) => {AD.velocityCap = value ? 0 : 30;};

            //DisplayInMapper config
            ADtoggle.DisplayInMapper = true;
        }
    }
}
