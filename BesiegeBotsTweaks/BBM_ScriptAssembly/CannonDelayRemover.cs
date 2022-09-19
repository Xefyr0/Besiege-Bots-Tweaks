/*
CannonDelayRemover.cs
Written by Xefyr for the Besiege Bots community
Adapted from the work of DokterDoyle

This class disables the random delay vanilla cannons have between a keypress and firing.
In other words, cannon blocks with this Component attached will all fire simultaneously and instantly.
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

