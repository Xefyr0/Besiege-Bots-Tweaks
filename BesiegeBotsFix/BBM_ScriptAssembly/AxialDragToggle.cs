/*
AxialDragToggle.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class AxialDragToggle : MonoBehaviour
    {
        void Awake()
        {
            AxialDrag AD = GetComponent<AxialDrag>();

            //Mapper definition
            MToggle ADtoggle = AD.AddToggle("Disable Drag", "Disable drag", false);
            ADtoggle.Toggled += (bool value) => {AD.velocityCap = value ? 0 : 30;};

            //DisplayInMapper config
            ADtoggle.DisplayInMapper = true;
        }
    }
}
