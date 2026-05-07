using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{


    public static class DamageManager
    {
        private static CombatConfig _config;

        public static void Initialise()
        {
            _config = Configuration.Get<CombatConfig>();    
        }

        public static void ApplyDamage(Marble from, Marble to, float amount)
        {
            // Ignore if less than the minimum damage threshold.
            if (amount < _config.minimumDamageThreshold)
            {
                return;
            }

            // Damage events hold all information regarding the damage being dealt:
            DamageEvent damage = new DamageEvent(from, to)
            {
                amount = amount * _config.damageMultiplier,
                knockbackAmount = amount * _config.knockbackMultiplier
            };

            to.health.TakeDamage(damage);
        }
    }


}
