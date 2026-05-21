using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Encounters
{
    [CreateAssetMenu(fileName = "New Enemy Squad", menuName = "Marble Bash/Enemies/Squad", order = 1)]
    internal class EnemySquad : ScriptableObject
    {
        [SerializeField] private int _cost;
        public int cost => _cost;

        [SerializeField] private EnemySquadElement[] _elements;
        public EnemySquadElement[] elements => _elements;


    }

    [System.Serializable]
    internal class EnemySquadElement
    {
        [SerializeField] private EnemyClass _enemyClass;
        public EnemyClass enemyClass => _enemyClass;

        [SerializeField, Min(1)] private int _minLevel;
        public int minLevel => _minLevel;

        [SerializeField, Min(1)] private int _maxLevel;
        public int maxLevel => _maxLevel;
    }
}