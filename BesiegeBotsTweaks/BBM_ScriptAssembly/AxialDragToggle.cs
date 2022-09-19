/*
AxialDragToggle.cs
Written by DokterDoyle for the Besiege Bots community
Amended by Xefyr

This class adds a toggle to blocks like Wings and Wing Panels
that disables their directional drag, if one is present.
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(AxialDrag))]
    public class AxialDragToggle : MonoBehaviour
    {
        void Awake()
        {
            AxialDrag AD = GetComponent<AxialDrag>();
            float VC = AD.velocityCap;

            //Mapper definition
            MToggle ADtoggle = AD.AddToggle("Disable Drag", "Disable drag", false);
            ADtoggle.Toggled += (bool value) => {AD.velocityCap = value ? 0 : VC;};

            //DisplayInMapper config
            ADtoggle.DisplayInMapper = true;
        }
    }
}
