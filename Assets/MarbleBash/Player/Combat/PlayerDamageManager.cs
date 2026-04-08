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


        private void HandleCollisionWithEnemy(Collision collision, EnemyInstance enemy)
        {
            // Find velocity against each other:
            Vector3 playerVelocity = Player.instance.cachedVelocity;
            Vector3 enemyVelocity = enemy.cachedVelocity;
            Debug.Log($"Player before: {Player.rigidbody.linearVelocity.magnitude:0.0}, After: {playerVelocity.magnitude:0.0}");
            Debug.Log($"Enemy before: {enemy.rigidbody.linearVelocity.magnitude:0.0}, After: {enemyVelocity.magnitude:0.0}");

            // Find direction between entities
            Vector3 directionToEnemy = (enemy.transform.position - Player.transform.position).normalized;
            Vector3 directionToPlayer = -directionToEnemy;

            // Find the raw damage from this collision 
            float rawPlayerDamage = FindDamageForCollision(Player.rigidbody, directionToEnemy, Player.rigidbody.mass, out float scaledPlayerVelocity);
            float rawEnemyDamage = FindDamageForCollision(enemy.rigidbody, directionToPlayer, enemy.rigidbody.mass, out float scaledEnemyVelocity);

            // Find damage scaling value:
            // A value of 1 means the player deals 100% damage and the enemy 0 
            // A value of 0 means both marbles deal 50% damage each 
            // A value of -1 means the enemy marble deals 100% and the player 0
            float damageScaling = FindDamageScaling(scaledPlayerVelocity, scaledEnemyVelocity);

            Debug.Log($"Player: {scaledPlayerVelocity}, Enemy: {scaledEnemyVelocity}, Damage scaling: {damageScaling}");

        }

        /// <summary>
        /// Reconstructs the velocities of two bodies after a collision.
        /// </summary>
        static void FindIncidentVelocity(Collision collision, Rigidbody A, Rigidbody B, out Vector3 aVelocity, out Vector3 bVelocity)
        {
            Vector3 impulse = collision.impulse;

            // Both participants of a collision see the same impulse, so we need to flip it for one of them.
            if (Vector3.Dot(collision.GetContact(0).normal, impulse) < 0f)
            {
                impulse *= -1f;                
            }

            bVelocity = B.linearVelocity + (impulse / B.mass);
            aVelocity = A.linearVelocity - (impulse / A.mass);
        }

        private static float FindDamageScaling(float playerVelocity, float enemyVelocity)
        {
            float damageScaling;
            if (playerVelocity > enemyVelocity)
            {
                damageScaling = playerVelocity / enemyVelocity;
            }
            else
            {
                damageScaling = -(enemyVelocity / playerVelocity);
            }
            damageScaling = Mathf.Clamp(damageScaling, -1f, 1f);
            return damageScaling;
        }

        private float FindDamageForCollision(Rigidbody rb, Vector3 directionToTarget, float mass, out float scaledVelocity)
        {
            Vector3 velocity = rb.linearVelocity;
            Vector3 position = rb.transform.position;

            // Base damage is simply mass * velocity
            float baseDamage = velocity.magnitude * mass;

            // Scale damage by how much the current velocity aligns with the hit marble:
            float velocityAlignment = Vector3.Dot(velocity.normalized, directionToTarget);
            scaledVelocity = velocity.magnitude * velocityAlignment;
            float scaledDamage = baseDamage * velocityAlignment;

            Debug.DrawLine(position, position + velocity, Color.blue, 3f);
            Debug.DrawLine(position, position + directionToTarget, Color.red, 3f);
            // Debug.Log($"Collision!   base damage:{baseDamage:0.0}, alignment: {velocityAlignment:0.0}, scaled: {scaledDamage:0.0}");
            
            return scaledDamage;
        }
    
    }



}

