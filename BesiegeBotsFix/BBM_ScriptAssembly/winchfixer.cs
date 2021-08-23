using Modding;
using UnityEngine;

namespace BotFix
{
    public class Winchfixer : MonoBehaviour
    {
        void Start()
        {
            GetComponent<BlockBehaviour>().Prefab.myDamageType = DamageType.Blunt;
        }
    }
}


