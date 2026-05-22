using System.Collections.Generic;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Encounters
{

    public class Encounter
    {
        private List<EncounterElement> _enemyElements;
        public EncounterElement[] elements
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

        internal Encounter(Vector3 pos, float radius)
        {
            _position = pos;
            _radius = radius;

            
            _enemyElements = new();
        }

        public void AddElement(EncounterElement toAdd)
        {
            _enemyElements.Add(toAdd);
        }
    }


}   

