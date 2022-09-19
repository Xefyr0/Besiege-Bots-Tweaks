using UnityEngine;

namespace BesiegeBotsTweaks
{
    public class SteeringBlockLimits : MonoBehaviour
    {
        private SteeringWheel SW;
        private void Awake()
        {
            SW = GetComponent<SteeringWheel>();
        }
        private void Start()
        {
            SW.LimitsSlider.UseLimitsToggle.IsActive = false;
            SW.LimitsSlider.UseLimitsToggle.ApplyValue();
            Object.Destroy(this);
        }
    }
}
