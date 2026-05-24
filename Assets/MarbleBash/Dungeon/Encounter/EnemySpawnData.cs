using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Encounters
{
    public class EnemySpawnData
    {
        [SerializeField] private EnemyClass _class;
        public EnemyClass @class => _class;

        [SerializeField] private EnemyType _type;
        public EnemyType type => _type;

        [SerializeField] private EnemyLevelUpProfile _levelUpProfile;
        public EnemyLevelUpProfile levelUpProfile => _levelUpProfile;

        [SerializeField] private int _level;
        public int level => _level;

        [SerializeField] private Vector3 _position;
        public Vector3 position => _position;

        public EnemySpawnData(EnemyClass @class, int level, Vector3 position)
        {
            _class = @class;
            _type = @class.GetTypeAtLevel(level);
            _levelUpProfile = @class.levelUpProfile;
            _level = level;
            _position = position;
        }
    }
}