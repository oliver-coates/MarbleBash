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

            SpawnXpGlobs(UnityEngine.Random.Range(1, 25f));

            Destroy(gameObject, Configuration.Get<HealthConfig>().deadMarbleFadeOutTime);
        }

        private void SpawnXpGlobs(float xpToDrop)
        {
            int maxNumOrbsToDrop = Mathf.FloorToInt(Mathf.Sqrt(0.1f * xpToDrop)+1);
            int xpRemaining = Mathf.FloorToInt(xpToDrop);

            for (int dropIndex = 0; dropIndex < maxNumOrbsToDrop; dropIndex++)
            {
                int xp = Mathf.CeilToInt(xpRemaining * UnityEngine.Random.Range(0.33f, 0.66f));
                xpRemaining -= xp;

                DropXpGlob(xp);

                if (xpRemaining == 0)
                {
                    break;
                }
            }
            if (xpRemaining > 0)
            {
                DropXpGlob(xpRemaining);
            }
        }

        private void DropXpGlob(int xp)
        {
            XpGlob glob = Instantiate(_combatConfig.xpPrefab).GetComponent<XpGlob>();
            glob.Initialise(this, xp);
        }

        protected override Vector3 GetLookDirection()
        {
            return (Player.transform.position - transform.position).normalized;
        }
    }


}
