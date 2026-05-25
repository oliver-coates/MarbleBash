using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash
{
    [System.Serializable]
    public class MutableStat 
    {
        [SerializeField] private float _value;
        public float value
        {
            get
            {
                return _value;
            }	
        }
    
        public event Action<float> OnChange;

        private List<MutableStatModifier> _modifiers;
        private float _baseValue;
        private CoreStat _linkedCoreStat;
        private float _valueModifierPerCoreStatLevel;    

        public MutableStat(float baseValue)
        {
            _modifiers = new ();
            _baseValue = baseValue;
        }

        public MutableStat(float baseValue, CoreStat statToLink, float valuePerLevel)
        {
            _modifiers = new();
            _baseValue = baseValue;

            LinkToCoreStat(statToLink, valuePerLevel);
        }

        public void AddModifier(MutableStatModifier modifier)
        {
            _modifiers.Add(modifier);

            RecalculateValue();
        }

        public void RemoveModifier(MutableStatModifier modifier)
        {
            _modifiers.Remove(modifier);

            RecalculateValue();
        }

        public void LinkToCoreStat(CoreStat coreStat, float valuePerLevel)
        {
            _linkedCoreStat = coreStat;
            _linkedCoreStat.OnChange += CoreStatChanged;
            _valueModifierPerCoreStatLevel = valuePerLevel;

            RecalculateValue();
        }

        public void RecalculateValue()
        {
            _value = _baseValue;

            if (_linkedCoreStat != null)
            {
                _value += (_linkedCoreStat.level-1) * _valueModifierPerCoreStatLevel;
            }

            float multiplier = 1;
            foreach (MutableStatModifier modifier in _modifiers)
            {
                _value += modifier.addition;
                multiplier *= modifier.multiplier;
            }

            _value *= multiplier;

            OnChange?.Invoke(_value);
        }
        
        private void CoreStatChanged(int coreStatLevel)
        {
            RecalculateValue();
        }
    }
}