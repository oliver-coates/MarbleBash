using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class AbilityEffectManager : MarbleSubComponent
    {
        [SerializeField] private List<AbilityEffect> _currentEffects;

        #region Initialisation & Destruction

        protected override void Initialise()
        {
            AbilityEffect.OnEffectFinished += RemoveEffect;
            
            _currentEffects = new List<AbilityEffect>();
        }

        private void OnDestroy()
        {
            AbilityEffect.OnEffectFinished -= RemoveEffect;            
        }

        #endregion

        public void AddEffect<TEffectType>() where TEffectType : AbilityEffect, new()
        {
            // Create a new instance of the provided type:
            TEffectType effect = new TEffectType();
            effect.Initialise(_marble);

            _currentEffects.Add(effect);
        }

        private void RemoveEffect(AbilityEffect effect)
        {
            _currentEffects.Remove(effect);
        }

        private void Update()
        {
            TickCurrentEffects();
        }



        private void TickCurrentEffects()
        {
            foreach (AbilityEffect effect in _currentEffects)
            {
                effect.Tick();
            }
        }
    }


}

