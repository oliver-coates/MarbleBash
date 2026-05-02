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

            mass = new CoreStat();
            agility = new CoreStat();
            recharge = new CoreStat();
            block = new CoreStat();
            luck = new CoreStat();
            energy = new CoreStat();
        }
    }


}

