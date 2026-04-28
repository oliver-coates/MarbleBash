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
        #region Stats:
        [Header("Stats:")]
        public CoreStat mass; // Size, Damage, Health
        public CoreStat agility; // Speed
        public CoreStat recharge; // Energy recharge rate, ability recharge rate
        public CoreStat block; // Health & Shield
        public CoreStat luck; // Crit chance
        public CoreStat energy; // Max energy

        #endregion
    }


}

