using System;
using KahuInteractive.HassleFreeConfig;
using MarbleBash.Encounters;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyInstance : Marble
    {
        private CombatConfig _combatConfig;
        public new EnemyMovement movement;

        private AbilityTelegraphManager _telegraphManager;
        public AbilityTelegraphManager abilityTelegraph
        {
            get
            {
                return _telegraphManager;            
            }
        }

        internal void Initialise(EnemySpawnData data)
        {
            _stats = new MarbleStats(data.level, data.type, data.levelUpProfile);
            InitialiseInternal();

            movement = this.GetComponentSafe<EnemyMovement>();
            _telegraphManager = GetComponentInChildren<AbilityTelegraphManager>();

            _combatConfig = Configuration.Get<CombatConfig>();

            // In future, enemies should be setup with a method:
            transform.position = data.position;

            _health.OnDied += Die;
        }


        private void Update()
        {
            if (!_health.isDead && transform.position.y < -25f)
            {
                _health.FellOffMap();
            }
        }

        private void Die()
        {
            // Disable behaviour
            movement.enabled = false;
            _abilities.enabled = false;

            gameObject.layer = LayerMask.NameToLayer("Debris");

            SpawnXpGlobs(UnityEngine.Random.Range(10f, 25f));
            DropQuartz(1);

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
    
        private void DropQuartz(int amount)
        {
            QuartzCrystal quartz = Instantiate(_combatConfig.quartzPrefab).GetComponent<QuartzCrystal>();
            quartz.Initialise(this, 1);
        }
    }


}
