using System;
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

        #region Stats:
        [Header("Stats:")]
        public CoreStat mass; // Size, Damage, Health
        public CoreStat agility; // Speed
        public CoreStat recharge; // Energy recharge rate, ability recharge rate
        public CoreStat block; // Health & Shield
        public CoreStat luck; // Crit chance
        public CoreStat energy; // Max energy

        #endregion

        public MarbleStats()
        {
            _level = 1;
            _xp = 0;
            RecalculateXpNeededForLevelUp();

            mass = new CoreStat();
            agility = new CoreStat();
            recharge = new CoreStat();
            block = new CoreStat();
            luck = new CoreStat();
            energy = new CoreStat();
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
        }
    }


}

