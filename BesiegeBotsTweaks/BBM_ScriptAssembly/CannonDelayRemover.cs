/*
CannonDelayRemover.cs
Written by Xefyr for the Besiege Bots community
Adapted from the work of DokterDoyle
*/

using UnityEngine;

namespace BesiegeBotsTweaks
{
    [RequireComponent(typeof(CanonBlock))]
    public class CannonDelayRemover : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<CanonBlock>().randomDelay = 0f;
            Destroy(this);
        }
    }
}

