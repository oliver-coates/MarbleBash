using System;
using UnityEngine;

namespace MarbleBash
{
    public enum CoreStatType
    {
        Mass = 1,
        Agility = 2,
        Recharge = 3,
        Block = 4,
        Luck = 5,
        Energy = 6
    }

    [System.Serializable]
    public class CoreStat
    {
        [SerializeField] private int _level;
        public int level
        {
            get
            {
                return _level;
            }
        }
    
        public event Action<int> OnChange;



        public CoreStat()
        {
            _level = 1;
        }

        public void LevelUp()
        {
            _level += 1;

            OnChange?.Invoke(_level);
        }
    }


}

