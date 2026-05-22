using System;
using System.Collections.Generic;
using KahuInteractive.HassleFreeConfig;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash
{
    
    /// <summary>
    /// Class representing the status of a marble:
    /// - Health, level, xp, name, etc
    /// </summary>
    public class MarbleStats
    {
        public const int MAXIMUM_LEVEL = 100;

        #region Events

        public event Action<int> OnXpChanged;
        public event Action<int> OnLevelUp;
        #endregion


        #region Type

        [SerializeField] private EnemyType _type;
        public EnemyType type => _type;

        #endregion


        #region Level
        [Header("Level:")]
        [SerializeField] private int _level;
        public int level
        {
            get
            {
                return _level;
            }
        }
        [SerializeField] private float _xp;
        public float xp
        {
            get
            {
                return _xp;
            }
        }
        
        [SerializeField] private float _xpNeededForLevelUp;
        public float xpNeededForLevelUp
        {
            get
            {
                return _xpNeededForLevelUp;
            }	
        }

        #endregion


        #region Core Stats
        [Header("Stats:")]
        // Points you can use to level up the core stats:
        private int _statLevelUpPoints;
        public int statLevelUpPoints => _statLevelUpPoints;

        public CoreStat mass; // Size, Damage, Health
        public CoreStat agility; // Speed
        public CoreStat recharge; // Energy recharge rate, ability recharge rate
        public CoreStat block; // Health & Shield
        public CoreStat luck; // Crit chance
        public CoreStat energy; // Max energy

        #endregion


        #region Mutable Stats
        private MutableStat[] _allMutableStats;
        
        // MASS:
        public MutableStat marbleSize;
        public MutableStat rigidbodyMass;
        public MutableStat maxHealth;

        // AGILITY:
        public MutableStat movementSpeed;
        public MutableStat jumpHeight;

        // RECHARGE:
        public MutableStat energyRechargeRate;
        public MutableStat abilityCooldownMultiplier;

        // BLOCK:
        public MutableStat maxShield;
        public MutableStat blockChance;

        // LUCK:
        public MutableStat criticalChance;
        public MutableStat lootRarity;

        // ENERGY:
        public MutableStat maxEnergy;
        public MutableStat shieldRechargeRate;
        
        // ADDITIONAL:
        public MutableStat rigidbodyDrag;

        #endregion


        #region Constructors
        public MarbleStats()
        {
            _level = 1;
            _xp = 0;
            RecalculateXpNeededForLevelUp();
            
            SetupStats();

        }

        public MarbleStats(int level, EnemyType type, EnemyLevelUpProfile levelUpProfile)
        {
            _type = type;
            
            SetupStats();

            BringToLevel(level, levelUpProfile);
        }

        #endregion


        #region Public Methods

        public void AddXp(int amount)
        {
            if (_level >= MAXIMUM_LEVEL)
            {
                return;
            }
            
            _xp += amount;

            if (_xp > _xpNeededForLevelUp)
            {
                _xp -= _xpNeededForLevelUp;
                LevelUp();
            }
            else
            {
                OnXpChanged?.Invoke(amount);
            }

        }

        public void LevelUpCoreStat(CoreStatType _toLevelUp)
        {
            if (_statLevelUpPoints > 0)
            {
                _statLevelUpPoints -= 1;
            }
            else
            {
                Debug.LogError("Not enough points to level up the stat.");
                return;
            }

            CoreStat toLevel = GetCoreStatFromType(_toLevelUp);
            toLevel.LevelUp();
        }

        internal void RecalulcateAllStats()
        {
            foreach (MutableStat stat in _allMutableStats)
            {
                stat.RecalculateValue();
            }
        }
        #endregion


        #region Internal Methods
        private void SetupStats()
        {
            // Mass:
            mass = new CoreStat();
            marbleSize = new MutableStat(
                Configuration.Read("mass_marble_size_base"), 
                mass,
                Configuration.Read("mass_marble_size_per_level")
            );
            rigidbodyMass = new MutableStat(
                Configuration.Read("mass_mass_base"),
                mass,
                Configuration.Read("mass_mass_per_level")
            );
            maxHealth = new MutableStat(
                Configuration.Read("mass_maxhealth_base"),
                mass,
                Configuration.Read("mass_maxhealth_per_level")
            );

            // Agility:
            agility = new CoreStat();
            movementSpeed = new MutableStat(
                Configuration.Read("agility_movespeed_base"), 
                agility,
                Configuration.Read("agility_movespeed_per_level")
            );
            jumpHeight = new MutableStat(
                Configuration.Read("agility_jump_height_base"),
                agility,
                Configuration.Read("agility_movespeed_per_level"));

            // Recharge:
            recharge = new CoreStat();
            energyRechargeRate = new MutableStat(
                Configuration.Read("recharge_recharge_rate_base"),
                recharge,
                Configuration.Read("recharge_recharge_rate_per_level")
            );
            abilityCooldownMultiplier = new MutableStat(
                Configuration.Read("recharge_cooldown_modifier_base"),
                recharge,
                Configuration.Read("recharge_cooldown_modifier_per_level")
            );

            // Block:
            block = new CoreStat();
            maxShield = new MutableStat(
                Configuration.Read("block_max_shield_base"),
                block,
                Configuration.Read("block_max_shield_per_level")
            );
            blockChance = new MutableStat(
                Configuration.Read("block_block_chance_base"),
                block,
                Configuration.Read("block_block_chance_per_level")
            );

            // Luck:
            luck = new CoreStat();
            criticalChance = new MutableStat(
                Configuration.Read("luck_critical_chance_base"),
                luck,
                Configuration.Read("luck_critical_chance_per_level")
            );
            lootRarity = new MutableStat(
                Configuration.Read("luck_loot_rarity_base"),
                luck,
                Configuration.Read("luck_loot_rarity_per_level")
            );

            // Energy:
            energy = new CoreStat();
            maxEnergy = new MutableStat(
                Configuration.Read("energy_max_energy_base"),
                energy,
                Configuration.Read("energy_max_energy_per_level")
            );
            shieldRechargeRate = new MutableStat(
                Configuration.Read("energy_shield_recharge_base"),
                energy,
                Configuration.Read("energy_shield_recharge_per_level")
            );
        
            // Additional:
            rigidbodyDrag = new MutableStat(0.25f);
            

            _allMutableStats = new MutableStat[]
            {
               marbleSize, rigidbodyMass, maxHealth,
               movementSpeed, rigidbodyDrag,
               energyRechargeRate, abilityCooldownMultiplier,
               maxShield, blockChance,
               criticalChance, lootRarity,
               maxEnergy, shieldRechargeRate  
            };
        }

        private void RecalculateXpNeededForLevelUp()
        {
            _xpNeededForLevelUp = _level * 100;
        }

        private void LevelUp()
        {
            _level += 1;
            _statLevelUpPoints += 1;
            RecalculateXpNeededForLevelUp();
            OnLevelUp?.Invoke(_level);
        }
    
        private void BringToLevel(int level, EnemyLevelUpProfile levelUpProfile)
        {
            for (int levelIndex = 1; levelIndex < level; levelIndex++)
            {
                _level += 1;
                _statLevelUpPoints += 1;

                LevelUpCoreStat(levelUpProfile.PickStatType());
            }            

            RecalculateXpNeededForLevelUp();
            
            // Just make sure the xp sits somewhere in the middle,
            // So enemies don't immediately just level up/down when we implement xp changing mechanics
            float lowerLimitXp = _xpNeededForLevelUp * 0.2f;
            float upperLimitXp = _xpNeededForLevelUp * 0.8f;
            _xp =  UnityEngine.Random.Range(lowerLimitXp, upperLimitXp);
        }

        private CoreStat GetCoreStatFromType(CoreStatType type)
        {
            switch (type)
            {
                case CoreStatType.Mass:
                    return mass;
                
                case CoreStatType.Agility:
                    return agility;
                
                case CoreStatType.Recharge:
                    return recharge;
                
                case CoreStatType.Block:
                    return block;
                
                case CoreStatType.Luck:
                    return luck;
                
                case CoreStatType.Energy:
                    return energy;
                
                default:
                    throw new Exception($"Unhandled Core Stat Type of '{type}'");
            }
        }
        
        public string GetStatsAsString()
        {
            return $"M: {mass.level} A: {agility.level} R: {recharge.level} B: {block.level} L: {luck.level} E: {energy.level}";
        }
        #endregion
    }
}       

