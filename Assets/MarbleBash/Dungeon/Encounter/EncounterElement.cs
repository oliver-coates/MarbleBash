using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Encounters
{
    [System.Serializable]
    public class EncounterElement
    {
        [SerializeField] private EnemyClass _enemyClass;
        public EnemyClass enemyClass => _enemyClass;

        [SerializeField] private int _num = 1;
        public int num => _num;
        
        [SerializeField] private int _minLevel = 1;
        [SerializeField] private int _maxLevel = 1;
        internal int level
        {
            get
            {
                return UnityEngine.Random.Range(_minLevel, _maxLevel+1);
            }

        }

        internal EncounterElement(EnemyClass @class, int number, int minimumLevel, int maximumLevel=0)
        {
            _enemyClass = @class;
            _num = number;

            _minLevel = minimumLevel;
            
            if (maximumLevel != 0)
            {
                _maxLevel = maximumLevel;
            }
            else
            {
                _maxLevel = _minLevel;
            }
        }
    }
}

