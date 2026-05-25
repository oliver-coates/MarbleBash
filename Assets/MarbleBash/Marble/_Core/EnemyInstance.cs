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
        [SerializeField] private EnemyType _type;
        public EnemyType type => _type;

        #region References / Subcomponents:
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
        
        
        private EnemyBrain _brain;
        public EnemyBrain brain => brain;
        #endregion

        internal void Initialise(EnemySpawnData data)
        {
            _level = new MarbleLevel(data.level, data.levelUpProfile);
            InitialiseInternal();

            movement = this.GetComponentSafe<EnemyMovement>();
            _telegraphManager = this.GetComponentInChildrenSafe<AbilityTelegraphManager>();
            _brain = this.GetComponentInChildrenSafe<EnemyBrain>();

            _combatConfig = Configuration.Get<CombatConfig>();

            SetType(data.@class, data.type);
            transform.position = data.position;

            health.OnDied += Die;
        }

        private void Update()
        {
            if (!health.isDead && transform.position.y < -25f)
            {
                health.FellOffMap();
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
    
        private void SetType(EnemyClass @class, EnemyType type)
        {
            if (_type != null)
            {
                Debug.LogError("This marble already has a type set.");
                return;
            }
            
            _type = type;

            // Abilities:
            _abilities.SetupAbilitiesFromEnemyClass(this, @class);

            // Ability upgrades:
            upgrades.SetupUpgradesFromEnemyType(type);

            // AI:
            EnemyBrainInitialiser.SetupBrain(_brain, @class);

            // Visuals:
            _materials.baseMat.SetColor("_BaseColor", _type.color);
        }
    }


}
