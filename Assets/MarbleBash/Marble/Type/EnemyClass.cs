using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Enemy
{
    [CreateAssetMenu(fileName = "EnemyClass", menuName = "Marble Bash/Enemies/Enemy Class")]
    public class EnemyClass : ScriptableObject
    {
        [SerializeField] private EnemyTypeCategory[] _enemyTypes;

        private Dictionary<ushort, EnemyType> _levelToTypeDictionary;

        public void Initialise()
        {
            // Set up the level to type dictionary:
            _levelToTypeDictionary = new();
            foreach (EnemyTypeCategory catergory in _enemyTypes)
            {
                ushort min = (ushort)catergory.minLevel;
                ushort max = (ushort)catergory.maxLevel;

                for (ushort level = min; level <= max; level++)
                {
                    _levelToTypeDictionary.Add(level, catergory.type);
                }
            }
        }
    
        public EnemyType GetTypeAtLevel(int level)
        {
            return _levelToTypeDictionary[(ushort) level];
        }
    }

    [System.Serializable]
    public class EnemyTypeCategory
    {
        [SerializeField] private EnemyType _type;
        public EnemyType type => _type;

        [SerializeField, Min(1)] private int _minLevel;
        public int minLevel => _minLevel;

        [SerializeField, Min(1)] private int _maxLevel;
        public int maxLevel => _maxLevel;


    }
}

