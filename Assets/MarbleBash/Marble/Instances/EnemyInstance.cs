using System;
using KahuInteractive.HassleFreeConfig;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyInstance : Marble
    {
        private CombatConfig _combatConfig;
        private EnemyMovement _movement;

        protected override void Setup()
        {
            _movement = this.GetComponentSafe<EnemyMovement>();

            _combatConfig = Configuration.Get<CombatConfig>();

            // In future, enemies should be setup with a method:
            _stats = new MarbleStats();

            _health.OnLivesChanged += LivesChanged;
        }

        private void LivesChanged(int livesRemaining)
        {
            // Check to see if we have died:
            if (livesRemaining == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Disable behaviour
            _movement.enabled = false;
            _abilities.enabled = false;

            gameObject.layer = LayerMask.NameToLayer("Debris");

            SpawnXpGlobs(_stats.level * 10);

            Destroy(gameObject, Configuration.Get<HealthConfig>().deadMarbleFadeOutTime);
        }

        private void SpawnXpGlobs(float xpToDrop)
        {
            int numToDrop = 10;

            for (int globIndex = 0; globIndex < numToDrop; globIndex++)
            {
                xpGlob glob = Instantiate(_combatConfig.xpPrefab).GetComponent<xpGlob>();

                glob.Setup(this, xpToDrop);    
            }
            
        }

        protected override Vector3 GetLookDirection()
        {
            return (Player.transform.position - transform.position).normalized;
        }
    }


}
