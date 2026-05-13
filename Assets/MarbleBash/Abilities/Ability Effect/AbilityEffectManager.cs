using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class AbilityEffectManager : MarbleSubComponent
    {
        [SerializeField] private List<AbilityEffect> _currentEffects;

        protected override void Initialise()
        {
            _currentEffects = new List<AbilityEffect>();
        }

        public TEffectType AddEffect<TEffectType>() where TEffectType : AbilityEffect, new()
        {
            // Create a new instance of the provided type:
            TEffectType effect = new TEffectType();
            effect.Initialise(_marble);

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

