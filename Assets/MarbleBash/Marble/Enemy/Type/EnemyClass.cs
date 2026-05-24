using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Enemy
{
    [CreateAssetMenu(fileName = "EnemyClass", menuName = "Marble Bash/Enemies/Enemy Class")]
    public class EnemyClass : ScriptableObject
    {
        #region Parameters
        [Header("Enemy Types:")]
        [SerializeField] private EnemyTypeCategory[] _enemyTypes;

        [Header("Level Up:")]
        [SerializeField] private EnemyLevelUpProfile _levelUpProfile;
        internal EnemyLevelUpProfile levelUpProfile => _levelUpProfile;

        [Header("Tactics:")]
        [SerializeField] private EnemyTacticTypeCategory[] _tactics;
        
        [Header("Abilities:")]
        [SerializeField] private EnemyAbilityTypeCategory[] _abilities;
        #endregion

    
        private Dictionary<ushort, EnemyType> _levelToTypeDictionary;

        internal void Initialise()
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
    
        public EnemyTacticTypeCategory[] GetTactics()
        {
            return _tactics;
        }

        public EnemyAbilityTypeCategory[] GetAbilities()
        {
            return _abilities;
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

    [System.Serializable]
    public class EnemyTacticTypeCategory
    {
        [SerializeField] private string _tactic;
        public string tacticName => _tactic;
        // [SerializeField] internal Tactic.Type tactic;
    }

    [System.Serializable]
    public class EnemyAbilityTypeCategory
    {
        [SerializeField] private string _abilityName;
        public string abilityName => _abilityName;
    }
}

