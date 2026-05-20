using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Combat Config", menuName = "Configuration/Combat")]
    public class CombatConfig : ConfigBase
    {
        [Header("Loot:")]
        [SerializeField] private GameObject _xpPrefab;
        public GameObject xpPrefab
        {
            get
            {
                return _xpPrefab;
            }	
        }

        [SerializeField] private GameObject _quartzPrefab;
        public GameObject quartzPrefab
        {
            get
            {
                return _quartzPrefab;
            }	
        }

    }
}
