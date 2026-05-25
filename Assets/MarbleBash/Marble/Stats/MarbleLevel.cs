using System;
using System.Collections.Generic;
using KahuInteractive.HassleFreeConfig;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash
{
    
    /// <summary>
    /// Class representing the level of a marble.
    /// </summary>
    public class MarbleLevel
    {
        public const int MAXIMUM_LEVEL = 100;

        #region Events

        public event Action<int> OnXpChanged;
        public event Action<int> OnLevelUp;
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

        private EnemyLevelUpProfile _levelUpProfile;

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


        #region Constructors
        public MarbleLevel()
        {
            _level = 1;
            _xp = 0;

            SetupCoreStats();
            RecalculateXpNeededForLevelUp();
        }

        public MarbleLevel(int level, EnemyLevelUpProfile levelUpProfile)
        {            
            SetupCoreStats();
            _levelUpProfile = levelUpProfile;
            BringToLevel(level);
        }

        private void SetupCoreStats()
        {
            mass = new CoreStat();
            agility = new CoreStat();
            recharge = new CoreStat();
            block = new CoreStat();
            luck = new CoreStat();
            energy = new CoreStat();
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

        public string GetStatsAsString()
        {
            return $"M: {mass.level} A: {agility.level} R: {recharge.level} B: {block.level} L: {luck.level} E: {energy.level}";
        }
        #endregion


        #region Internal Methods
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

            if (_levelUpProfile != null)
            {
                IncreaseStatByLevelUpProfile();
            }
        }

        private void IncreaseStatByLevelUpProfile()
        {
            LevelUpCoreStat(_levelUpProfile.PickStatType());
        }

        private void BringToLevel(int level)
        {
            for (int levelIndex = 1; levelIndex < level; levelIndex++)
            {
                _level += 1;
                _statLevelUpPoints += 1;

                IncreaseStatByLevelUpProfile();
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
    
        #endregion
    }
}       

