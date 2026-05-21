using System.Collections.Generic;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Encounters
{

    public class Encounter
    {
        private List<ElementTEMP> _enemyElements;
        public ElementTEMP[] elements
        {
            get
            {
                return _enemyElements.ToArray();
            }
        }

        [SerializeField] private Vector3 _position;
        public Vector3 position => _position;

        [SerializeField] private float _radius;
        public float radius => _radius;

        internal Encounter()
        {
            _enemyElements = new();
        }

        public class ElementTEMP
        {
            internal EnemyClass enemyClass;
            
            internal int num;
            
            private int minLevel;
            private int maxLevel;
            internal int level
            {
                get
                {
                    return UnityEngine.Random.Range(minLevel, maxLevel+1);
                }

            }

        }
    }


}   

