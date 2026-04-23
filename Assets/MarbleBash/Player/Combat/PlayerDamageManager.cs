using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    public class PlayerDamageManager : MonoBehaviour
    {
        private CombatConfig _config;

        private void Start()
        {
            _config = Configuration.Get<CombatConfig>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                EnemyInstance enemy = collision.collider.GetComponentSafe<EnemyInstance>();
                HandleCollisionWithEnemy(collision, enemy);
            }
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
        private void HandleCollisionWithEnemy(Collision collision, EnemyInstance enemy)
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
            ApplyDamage(Player.instance, enemy, playerDamage, collision);
            ApplyDamage(enemy, Player.instance, enemyDamage, collision);
        }

        private float FindAlignedVelocity(Marble primaryBody, Marble hitBody)
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

        private float FindRawDamageForCollision(Marble marble, float alignedVelocity)
        {
            // Base damage is simply mass * velocity
            float baseDamage = alignedVelocity * marble.rigidbody.mass;;
            
            // Vector3 position = marble.transform.position;
            // Vector3 velocity = marble.cachedVelocity;
            // Debug.DrawLine(position, position + velocity, Color.blue, 3f);
            // Debug.Log($"Collision!   base damage:{baseDamage:0.0}, alignment: {velocityAlignment:0.0}, scaled: {scaledDamage:0.0}");
            
            return baseDamage;
        }
            
        private void ApplyDamageRampup(float rawPlayerDamage, float rawEnemyDamage, out float playerDamage, out float enemyDamage)
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

        private void ApplyDamage(Marble from, Marble to, float amount, Collision collision)
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

            to.TakeDamage(damage);
        }        
    }



}

