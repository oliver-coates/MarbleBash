using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash
{
    
    /// <summary>
    /// Class representing the status of a marble:
    /// - Health, level, xp, name, etc
    /// </summary>
    [System.Serializable]
    public class MarbleStats
    {
        #region Events:

        public event Action<int> OnXpChanged;
        public event Action<int> OnLevelUp;
        #endregion

        #region Level:
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


        #region Core Stats:
        [Header("Stats:")]
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
        public MutableStat rigidbodyDrag;

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

        #endregion

        public MarbleStats()
        {
            _level = 1;
            _xp = 0;
            RecalculateXpNeededForLevelUp();
            
            SetupStats();
        }

        private void SetupStats()
        {
            // Mass:
            mass = new CoreStat();
            marbleSize = new MutableStat(0.70f, mass, 0.05f);
            rigidbodyMass = new MutableStat(0.20f, mass, 0.05f);
            maxHealth = new MutableStat(20f, mass, 5f);

            // Agility:
            agility = new CoreStat();
            movementSpeed = new MutableStat(50f, agility, 25f);
            rigidbodyDrag = new MutableStat(0.05f, agility, 0.05f);

            // Recharge:
            recharge = new CoreStat();
            energyRechargeRate = new MutableStat(2.5f, recharge, 2.5f);
            abilityCooldownMultiplier = new MutableStat(1.05f, recharge, -0.05f);

            // Block:
            block = new CoreStat();
            maxShield = new MutableStat(0f, block, 10f);
            blockChance = new MutableStat(0f, block, 0.25f);

            // Luck:
            luck = new CoreStat();
            criticalChance = new MutableStat(0.5f, luck, 0.5f);
            lootRarity = new MutableStat(0.95f, luck, 0.05f);

            // Energy:
            energy = new CoreStat();
            maxEnergy = new MutableStat(40f, energy, 10f);
            shieldRechargeRate = new MutableStat(5f, energy, 2.5f);
        
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
    
        public void AddXp(int amount)
        {
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

        private void LevelUp()
        {
            _level += 1;
            RecalculateXpNeededForLevelUp();
            OnLevelUp?.Invoke(_level);

            // TEMP DEBUG:
            mass.LevelUp();
        }
    
        public void RecalulcateAllStats()
        {
            foreach (MutableStat stat in _allMutableStats)
            {
                stat.RecalculateValue();
            }
        }
    }


}

