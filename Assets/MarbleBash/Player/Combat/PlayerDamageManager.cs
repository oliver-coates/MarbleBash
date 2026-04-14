using UnityEngine;

namespace MarbleBash
{

    public class PlayerDamageManager : MonoBehaviour
    {
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
        /// ### HOW DAMAGE IS CALCUALTED: ###
        /// 
        /// 'Aligned Velocity' is the magnitude of a marble's velocity, scaled by how much their velocity is pointing towards their target.
        /// - For instance, a slight glancing blow does much less damage than a direct hit.
        /// 
        /// 'Raw Damage' is simply mass * aligned velocity.
        ///  - The size and the speed of your marble affect the damage dealt,
        ///  - And maximum damage is dealt when your velocity in pointing directly at the hit marble
        /// 
        /// The damage of the two marbles are then compared, if one marble's raw damage is greater than double the other marble, it 
        /// deals 100% damage while the other marble's damage is ignored.
        /// 
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="enemy"></param>
        private void HandleCollisionWithEnemy(Collision collision, EnemyInstance enemy)
        {
            // Find the aliged velocity between the player and enemy:
            float alignedVelocityPlayer = FindAlignedVelocity(Player.instance, enemy);
            float alignedVelocityEnemy = FindAlignedVelocity(enemy, Player.instance);

            // Find the raw damage from this collision 
            float rawPlayerDamage = FindRawDamageForCollision(Player.instance, Player.rigidbody.mass, alignedVelocityPlayer);
            float rawEnemyDamage = FindRawDamageForCollision(enemy, enemy.rigidbody.mass, alignedVelocityEnemy);

            // Find damage scaling value:
            // A value of 1 means the player deals 100% damage and the enemy 0 
            // A value of 0 means both marbles deal 50% damage each 
            // A value of -1 means the enemy marble deals 100% and the player 0
            float damageScaling = FindDamageRampup(rawPlayerDamage, rawEnemyDamage);

            Debug.Log($"Player: {rawPlayerDamage}, Enemy: {rawEnemyDamage}, Damage scaling: {damageScaling}");

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



        private float FindRawDamageForCollision(Marble marble, float mass, float alignedVelocity)
        {
            // Base damage is simply mass * velocity
            float baseDamage = alignedVelocity * mass;
            
            // Vector3 position = marble.transform.position;
            // Vector3 velocity = marble.cachedVelocity;
            // Debug.DrawLine(position, position + velocity, Color.blue, 3f);
            // Debug.Log($"Collision!   base damage:{baseDamage:0.0}, alignment: {velocityAlignment:0.0}, scaled: {scaledDamage:0.0}");
            
            return baseDamage;
        }
    

        private static float FindDamageRampup(float playerVelocity, float enemyVelocity)
        {
            float damageRampup;
            
            if (playerVelocity > enemyVelocity)
            {
                damageRampup = Mathf.Clamp(playerVelocity / enemyVelocity, 0, 2f);
                damageRampup = damageRampup / 2f;
            }
            else
            {
                damageRampup = Mathf.Clamp(enemyVelocity / playerVelocity, 0f, 2f);
                damageRampup = damageRampup / 2f;
                damageRampup = -damageRampup;
            }

            return damageRampup;
        }

    }



}

