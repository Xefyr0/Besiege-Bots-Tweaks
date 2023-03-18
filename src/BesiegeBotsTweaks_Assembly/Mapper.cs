using ObjectExplorer.Mappings;
using UnityEngine;

namespace BesiegeBotsTweaks
{
    class Mapper : MonoBehaviour
    {
        private void Awake()
        {
            MFloat<RealisticMotorTorque> velocity = new MFloat<RealisticMotorTorque>("Input Speed", c => c.Velocity);
            MFloat<RealisticMotorTorque> targetVelocity = new MFloat<RealisticMotorTorque>("Target Velocity", c => c.targetVelocity);

            ObjectExplorer.ObjectExplorer.AddMappings("ThermalFire", velocity, targetVelocity);
        }
    }
}
