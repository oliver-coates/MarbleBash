using System;
using KahuInteractive.HassleFreeConfig;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyInstance : Marble
    {
        private EnemyMovement _movement;

        protected override void Setup()
        {
            _movement = this.GetComponentSafe<EnemyMovement>();

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

            Destroy(gameObject, Configuration.Get<HealthConfig>().deadMarbleFadeOutTime);
        }

        protected override Vector3 GetLookDirection()
        {
            return (Player.transform.position - transform.position).normalized;
        }
    }


}
