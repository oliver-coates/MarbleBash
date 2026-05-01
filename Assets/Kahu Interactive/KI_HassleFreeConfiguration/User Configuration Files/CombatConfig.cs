using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Combat Config", menuName = "Configuration/Combat")]
    public class CombatConfig : ConfigBase
    {
        [Header("Global Tuners:")]

        [SerializeField] private float _damageMultiplier;
        /// <summary>
        /// Flat multiplier added to all damage calculations
        /// </summary>
        public float damageMultiplier
        {
            get
            {
                return _damageMultiplier;
            }	
        }

        [SerializeField] private float _knockbackMultiplier;
        /// <summary>
        /// Flat multiplier added to all knockback calculations
        /// </summary>
        public float knockbackMultiplier
        {
            get
            {
                return _knockbackMultiplier;
            }	
        }

        [Header("Ignore Damage Conditions:")]
        [Tooltip("If damage from an attack is less than this amount, the damage event is discarded.")]
        [SerializeField, Min(0)] private float _minimumDamageThreshold;
        public float minimumDamageThreshold
        {
            get
            {
                return _minimumDamageThreshold;
            }	
        }

            [Header("XP:")]
        [SerializeField] private GameObject _xpPrefab;
        public GameObject xpPrefab
        {
            get
            {
                return _xpPrefab;
            }	
        }

    }
}
