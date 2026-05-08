using System;
using UnityEngine;


namespace MarbleBash
{
    
    public class MarbleHitParticles : MonoBehaviour
    {
        private ParticleSystem _particles;

        #region Initialisation & Destruction
        private void Awake()
        {
            MarbleHealth.OnDamageTakenGlobal += MarbleTakenDamage;
        
            _particles = this.GetComponentSafe<ParticleSystem>();
        }

        private void OnDestroy()
        {
            MarbleHealth.OnDamageTakenGlobal -= MarbleTakenDamage;            
        }
        
        #endregion


        private void MarbleTakenDamage(MarbleHealth.HealthChangedEvent @event)
        {
            if (@event.damage.doDamageEffects == false)
            {
                return;
            }
            
            Vector3 location = @event.marble.transform.position;
            int amount = UnityEngine.Random.Range(0, 4) + Mathf.CeilToInt(-@event.totalHealthChange / 10);

            Emit(amount, location);
        }

        private void Emit(int amount, Vector3 location)
        {
            transform.position = location;
            _particles.Emit(amount);            
        }



    }
}