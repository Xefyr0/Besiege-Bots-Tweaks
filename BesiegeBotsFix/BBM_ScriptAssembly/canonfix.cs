using UnityEngine;

namespace BotFix
{
    public class CanonFix : MonoBehaviour
    {
        void Awake()
        {
            CanonBlock CB = GetComponent<CanonBlock>();
            if (CB != null)
            {
                CB.randomDelay = 0f;
            }
        }
    }
}

