using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class StatusEffectManager : MarbleSubComponent
    {
        [SerializeField] private List<AbilityEffect> _currentEffects;
        private Dictionary<Type, AbilityEffect> _effectDict;

        protected override void Initialise()
        {
            _currentEffects = new List<AbilityEffect>();
            _effectDict = new Dictionary<Type, AbilityEffect>();
        }

        public TEffectType AddEffect<TEffectType>(float duration = 1f) where TEffectType : AbilityEffect, new()
        {
            Type type = typeof(TEffectType);
            TEffectType effect;

            if (IsEffectTypeActive<TEffectType>())
            {
                // Extend the duration of the current effect:
                effect = _effectDict[type] as TEffectType;
                effect.ExtendDuration(duration);      
            }
            else
            {
                // Create a new instance of the provided type:
                effect = CreateEffect<TEffectType>(duration);
                _effectDict.Add(type, effect);
            }
            
            return effect;
        }

        private bool IsEffectTypeActive<TEffectType>() where TEffectType : AbilityEffect
        {
            Type t = typeof(TEffectType);
            
            bool isInDict = _effectDict.ContainsKey(t); 
            if (isInDict)
            {
                return _effectDict[t] != null;
            }
            else
            {
                return false;
            }
        }

        public TEffectType GetEffect<TEffectType>() where TEffectType : AbilityEffect
        {
            if (IsEffectTypeActive<TEffectType>())
            {
                return _effectDict[typeof(TEffectType)] as TEffectType;
            }
            
            return null;
        }

        private TEffectType CreateEffect<TEffectType>(float duration) where TEffectType : AbilityEffect, new()
        {
            TEffectType effect = new TEffectType();
            effect.Initialise(_marble, duration);

            _currentEffects.Add(effect);
            return effect;
        }




        private void Update()
        {
            TickCurrentEffects();
        }

        private void TickCurrentEffects()
        {
            int adjustedIndex = 0;
            for (int index = 0; index < _currentEffects.Count; index++)
            {
                AbilityEffect effect = _currentEffects[adjustedIndex]; 
                effect.Tick();

                if (effect.isFinished)
                {
                    _currentEffects.Remove(effect);
                }   
                else
                {
                    adjustedIndex++;
                }
            }
        }

        
    }


}

