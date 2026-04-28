using System;
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


        private float _damageFlashIntensity;



        protected override void Initialise()
        {
            _marble.health.OnDamageTaken += DamageTaken;
            _marble.health.OnLivesChanged += LivesChanged;

            _damageFlashMaterial = new Material(_damageFlashRenderer.material);
            _damageFlashRenderer.material = _damageFlashMaterial;
        
            _config = Configuration.Get<HealthConfig>();
        }

        private void LivesChanged(int livesChanged)
        {
            Debug.Log($"Lives have changed! {livesChanged}");
        }

        private void DamageTaken(MarbleHealth.HealthChangedEvent @event)
        {
            // Calculate incoming damage as percentage of health:
            float damagePercentage = @event.healthChange / _marble.health.maxHealth;

            float newIntensity = damagePercentage * (1/_config.damagePercentageForFullFlashIntensity);

            _damageFlashIntensity = Mathf.Clamp(_damageFlashIntensity + newIntensity, 0, 1f);
        }

        private void Update()
        {
            float intensity = _config.damageFlashIntensityCurve.Evaluate(_damageFlashIntensity);

            _damageFlashMaterial.SetFloat("_Intensity", intensity);
            _damageFlashIntensity = Mathf.MoveTowards(_damageFlashIntensity, 0, Time.deltaTime * _config.damageFlashIntensityFalloff);      
        }
    }
}