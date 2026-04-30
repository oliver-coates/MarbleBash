using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Health Config", menuName = "Configuration/Health")]
    public class HealthConfig : ConfigBase
    {
        [Header("Visuals:")]
        [Range(0, 1f), SerializeField] private float _damagePercentageForFullFlashIntensity;
        public float damagePercentageForFullFlashIntensity
        {
            get
            {
                return _damagePercentageForFullFlashIntensity;
            }
        }
    
        [SerializeField] private float _damageFlashIntensityFalloff;
        public float damageFlashIntensityFalloff
        {
            get
            {
                return _damageFlashIntensityFalloff;
            }	
        }

        [SerializeField] private AnimationCurve _damageFlashIntensityCurve;
        public AnimationCurve damageFlashIntensityCurve
        {
            get
            {
                return _damageFlashIntensityCurve;
            }	
        }

        [Header("Death Effects:")]
        [SerializeField] private float _deadMarbleFadeOutTime;
        public float deadMarbleFadeOutTime
        {
            get
            {
                return _deadMarbleFadeOutTime;
            }	
        }
        [SerializeField] private AnimationCurve _deadMarbleFadeOutCurve;
        public AnimationCurve deadMarbleFadeOutCurve
        {
            get
            {
                return _deadMarbleFadeOutCurve;
            }	
        }
    }

}
