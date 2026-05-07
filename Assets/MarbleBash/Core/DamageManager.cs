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

        #region Public Methods
        /// <summary>
        /// Generates and applies damage to a marble.
        /// </summary>
        /// <param name="from">The marble causing the damage.</param>
        /// <param name="to">The marble recieving the damage.</param>
        /// <param name="amount">The amount of damage.</param>
        /// <param name="knockbackDirection">The direction of knockback, leave as Vector3.zero to have no knockback. 
        /// This will be normalized before it is applied to the marble.</param>
        public static void ApplyDamage(Marble from, Marble to, float amount, Vector3 knockbackDirection, float knockbackMultiplier=1f)
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
                knockbackAmount = amount * knockbackMultiplier * _config.knockbackMultiplier,
                knockbackDirection = knockbackDirection
            };

            to.health.TakeDamage(damage);
        }
    
        /// <summary>
        /// Handles a collision between the player and an enemy.
        /// Calculated and applies damage to each entity based on their speed, direction & mass.
        /// 
        /// ### HOW DAMAGE IS CALCULATED: ###
        ///
        /// 'Aligned Velocity' is the magnitude of a marble's velocity, scaled by how much their velocity is pointing towards their target.
        /// - For instance, a slight glancing blow does much less damage than a direct hit.
        /// 
        /// 'Raw Damage' is simply mass * aligned velocity.
        ///  - The size and the speed of your marble affect the damage dealt,
        ///  - And maximum damage is dealt when your velocity in pointing directly at the hit marble
        ///
        /// Damage Rampup: 
        /// The damage of the two marbles are then compared, if one marble's raw damage is greater than double the other marble, it 
        /// deals 100% damage while the other marble's damage is ignored.
        /// 
        /// Finally, buffs and multipliers are applied ontop of these final values.
        /// The final damage values are then dealt to the respective marbles.
        /// </summary>
        public static void HandleCollisionWithEnemy(Collision collision, EnemyInstance enemy)
        {
            // Find the aliged velocity between the player and enemy:
            float alignedVelocityPlayer = FindAlignedVelocity(Player.instance, enemy);
            float alignedVelocityEnemy = FindAlignedVelocity(enemy, Player.instance);

            // Find the raw damage from this collision 
            float rawPlayerDamage = FindRawDamageForCollision(Player.instance, alignedVelocityPlayer);
            float rawEnemyDamage = FindRawDamageForCollision(enemy, alignedVelocityEnemy);

            // Find damage scaling value:
            // A value of 1 means the player deals 100% damage and the enemy 0 
            // A value of 0 means both marbles deal 50% damage each 
            // A value of -1 means the enemy marble deals 100% and the player 0
            ApplyDamageRampup(rawPlayerDamage, rawEnemyDamage, out float playerDamage, out float enemyDamage);

            // Final damage calculations, applying any additional damage multipliers from scaling and from perks
            Vector3 enemyKnockback = GetStandardKnockbackDirection(Player.instance, enemy);
            ApplyDamage(Player.instance, enemy, playerDamage, enemyKnockback);

            Vector3 playerKnockback = GetStandardKnockbackDirection(enemy, Player.instance);
            ApplyDamage(enemy, Player.instance, enemyDamage, playerKnockback);
        }

        #endregion
    
        
        #region Internal damage calculation methods
        private static float FindAlignedVelocity(Marble primaryBody, Marble hitBody)
        {
            Vector3 velocity = primaryBody.cachedVelocity;

            // Find direction between entities
            Vector3 directionToHitMarble = (hitBody.transform.position - primaryBody.transform.position).normalized;

            // Find how aligned the current velocity is against the direction to hit the other marble
            float velocityAlignment = Vector3.Dot(velocity.normalized, directionToHitMarble);

            // Scale damage by how much the current velocity aligns with the hit marble:
            float alignedVelocity = velocity.magnitude * velocityAlignment;

            return alignedVelocity;
        }

        private static float FindRawDamageForCollision(Marble marble, float alignedVelocity)
        {
            // Base damage is simply mass * velocity
            float baseDamage = alignedVelocity * marble.rigidbody.mass;;
            
            // Vector3 position = marble.transform.position;
            // Vector3 velocity = marble.cachedVelocity;
            // Debug.DrawLine(position, position + velocity, Color.blue, 3f);
            // Debug.Log($"Collision!   base damage:{baseDamage:0.0}, alignment: {velocityAlignment:0.0}, scaled: {scaledDamage:0.0}");
            
            return baseDamage;
        }
            
        private static void ApplyDamageRampup(float rawPlayerDamage, float rawEnemyDamage, out float playerDamage, out float enemyDamage)
        {
            float damageRampup;
            float multiplier;

            if (rawPlayerDamage > rawEnemyDamage)
            {
                damageRampup = Mathf.Clamp(rawPlayerDamage / rawEnemyDamage, 0, 2f) / 2f;

                multiplier = Mathf.Lerp(0.5f, 1f, Mathf.Abs(damageRampup));
                
                playerDamage = rawPlayerDamage * multiplier;
                enemyDamage = rawEnemyDamage * (1f - multiplier);
            }
            else
            {
                damageRampup = Mathf.Clamp(rawEnemyDamage / rawPlayerDamage, 0f, 2f) / 2f;

                multiplier = Mathf.Lerp(0.5f, 1f, Mathf.Abs(damageRampup));
                
                enemyDamage = rawEnemyDamage * multiplier;
                playerDamage = rawPlayerDamage * (1f - multiplier);
            }
        }

        private static Vector3 GetStandardKnockbackDirection(Marble from, Marble to)
        {
            Vector3 dir = (to.transform.position - from.transform.position).normalized;
            return dir + Vector3.up * 0.33f;
        }

        #endregion
        
    }


}
