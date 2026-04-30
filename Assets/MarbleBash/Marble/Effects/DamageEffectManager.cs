using System;
using System.Threading;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    public class DamageEffectManager : MarbleSubComponent
    {
        private HealthConfig _config;

        [Header("References:")]
        [SerializeField] private MeshRenderer _damageFlashRenderer;
        private Material _damageFlashMaterial;
        [SerializeField] private MeshRenderer _baseRenderer;
        private Material _baseMaterial;

        [SerializeField] private MeshRenderer _outlineRenderer;
        private Material _outlineMaterial;

        private float _damageFlashIntensity;

        private bool _isDead;
        private float _deathTimer;


        protected override void Initialise()
        {
            _marble.health.OnDamageTaken += DamageTaken;
            _marble.health.OnLivesChanged += LivesChanged;

            _damageFlashMaterial = new Material(_damageFlashRenderer.material);
            _damageFlashRenderer.material = _damageFlashMaterial;
        
            _baseMaterial = new Material(_baseRenderer.material);
            _baseRenderer.material = _baseMaterial;

            _outlineMaterial = new Material(_outlineRenderer.material);
            _outlineRenderer.material = _outlineMaterial;

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
            ApplyDamageTakenEffect(@event.healthChange);
        }


        private void ApplyDeathEffect()
        {
            _isDead = true;
            
            // Stop casting shadows
            _baseRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
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

            _damageFlashMaterial.SetFloat("_Intensity", intensity);
            _outlineMaterial.SetFloat("_DamageIntensity", intensity);

            _damageFlashIntensity = Mathf.MoveTowards(_damageFlashIntensity, 0, Time.deltaTime * _config.damageFlashIntensityFalloff);      

            if (_isDead)
            {
                _deathTimer += Time.deltaTime;
                
                _baseMaterial.SetFloat("_Desaturation", Mathf.Clamp(_deathTimer, 0, 1f));
                
                float trans = _config.deadMarbleFadeOutCurve.Evaluate(Mathf.Clamp(_deathTimer / _config.deadMarbleFadeOutTime, 0, 1f));
                _outlineMaterial.SetFloat("_Transparency", trans);
                _baseMaterial.SetFloat("_Transparency", trans);
            }
        }
    }
}