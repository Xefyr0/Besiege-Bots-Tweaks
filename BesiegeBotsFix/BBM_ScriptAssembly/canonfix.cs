using UnityEngine;

namespace BotFix
{
    [RequireComponent(typeof(CanonBlock))]
    public class CanonFix : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<CanonBlock>().randomDelay = 0f;
            Destroy(this);
        }
    }
}

