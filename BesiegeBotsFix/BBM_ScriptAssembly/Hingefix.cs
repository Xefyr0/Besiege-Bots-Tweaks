using UnityEngine;

namespace BotFix
{
    public class Hfix : MonoBehaviour
    {
        private StrengthenTreads ST;
        void Awake()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (GetComponent<StrengthenTreads>())
                {
                    ST = GetComponent<StrengthenTreads>();
                    ST.breakForce = 50000;
                    ST.breakTorque = 50000;
                }
            }
        }
    }
}

