using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.StatusEffects
{

    public class StatusEffectManager : MarbleSubComponent
    {
        [SerializeField] private List<StatusEffect> _currentEffects;

        #region Initialisation & Destruction

        protected override void Initialise()
        {
            StatusEffect.OnEffectFinished += RemoveEffect;
            
            _currentEffects = new List<StatusEffect>();
        }

        private void OnDestroy()
        {
            StatusEffect.OnEffectFinished -= RemoveEffect;            
        }

        #endregion

        public void AddEffect<TEffectType>() where TEffectType : StatusEffect, new()
        {
            // Create a new instance of the provided type:
            TEffectType effect = new TEffectType();
            effect.Initialise(_marble);

            _currentEffects.Add(effect);
        }

        private void RemoveEffect(StatusEffect effect)
        {
            _currentEffects.Remove(effect);
        }

        private void Update()
        {
            TickCurrentEffects();
        }



        private void TickCurrentEffects()
        {
            foreach (StatusEffect effect in _currentEffects)
            {
                effect.Tick();
            }
        }
    }


}

