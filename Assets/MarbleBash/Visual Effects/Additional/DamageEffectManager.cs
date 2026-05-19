using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    public class DamageEffectManager : MarbleSubComponent
    {
        private HealthConfig _config;

        

        private float _damageFlashIntensity;

        private bool _isDead;
        private float _deathTimer;


        protected override void Initialise()
        {
            _marble.health.OnDamageTaken += DamageTaken;
            _marble.health.OnLivesChanged += LivesChanged;

            _config = Configuration.Get<HealthConfig>();
        }

        private void LivesChanged(int livesRemaining)
        {
            // Debug.Log($"Lives have changed! {livesChanged}");
            if (livesRemaining == 0)
            {
                ApplyDeathEffect();
            }
        }

        private void DamageTaken(MarbleHealth.HealthChangedEvent @event)
        {
            if (@event.damage.doDamageEffects)
            {
                ApplyDamageTakenEffect(@event.healthChange);            
            }
        }


        private void ApplyDeathEffect()
        {
            _isDead = true;
            
            // Stop casting shadows
            _marble.materials.baseRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }


        private void ApplyDamageTakenEffect(float damage)
        {
            // Calculate incoming damage as percentage of health:
            float damagePercentage = damage / _marble.health.maxHealth;

            float newIntensity = damagePercentage * (1/_config.damagePercentageForFullFlashIntensity);

            _damageFlashIntensity = Mathf.Clamp(_damageFlashIntensity + newIntensity, 0, 1f);
        }

        private void Update()
        {
            float intensity = _config.damageFlashIntensityCurve.Evaluate(_damageFlashIntensity);

            _marble.materials.damageFlash.SetFloat("_Intensity", intensity);
            _marble.materials.outline.SetFloat("_DamageIntensity", intensity);

            _damageFlashIntensity = Mathf.MoveTowards(_damageFlashIntensity, 0, Time.deltaTime * _config.damageFlashIntensityFalloff);      

            if (_isDead)
            {
                _deathTimer += Time.deltaTime;
                
                _marble.materials.baseMat.SetFloat("_Desaturation", Mathf.Clamp(_deathTimer, 0, 1f));
                
                float trans = _config.deadMarbleFadeOutCurve.Evaluate(Mathf.Clamp(_deathTimer / _config.deadMarbleFadeOutTime, 0, 1f));
                _marble.materials.outline.SetFloat("_Transparency", trans);
                _marble.materials.baseMat.SetFloat("_Transparency", trans);
            }
        }
    }
}