using UnityEngine;

namespace MarbleBash.Upgrades
{
    [System.Serializable]
    public class MarbleUpgradeElement
    {
        [SerializeField] private string _name;
        public string name => _name;
        
        [SerializeField] private float _value;
        public float value => _value;
    }
}