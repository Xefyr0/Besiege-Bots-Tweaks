using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
 * SensorDisjoint.cs
 * Written by Xefyr for the Besiege Bots community
 * 
 * This class adds a slider to the Sensor block that it is attached to.
 * That slider can move the whole sensor trigger farther away from the main block,
 * reminiscent of a "disjoint" hitbox in combat games.
 */

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(SensorBlock))]
    class SensorDisjoint : MonoBehaviour
    {
        private BlockBehaviour BB;
        private MSlider DS;
        private Transform SensorPos;
        private float defaultAmount = 0;
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            SensorPos = BB.transform.FindChild("SensorPos");
            defaultAmount = SensorPos.localPosition.y;
            DS = BB.AddSlider("Disjoint", "Disjoint", -defaultAmount, 0f, 4f);
            DS.ValueChanged += (value => 
            {
                SensorPos.localPosition = new Vector3(SensorPos.localPosition.x, -value, SensorPos.localPosition.z);
            });
        }
    }
}
