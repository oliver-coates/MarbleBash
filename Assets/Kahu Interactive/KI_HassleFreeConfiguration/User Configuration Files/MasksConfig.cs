using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Masks Config", menuName = "Configuration/Masks")]
    public class MasksConfig : ConfigBase
    {
        [SerializeField] private LayerMask _defaultLayer;
        public LayerMask defaultLayer => _defaultLayer;

        [SerializeField] private LayerMask _grounded;
        public LayerMask grounded
        {
            get
            {
                return _grounded;
            }
        }

        [SerializeField] private LayerMask _debris;
        public LayerMask debris
        {
            get
            {
                return _debris;
            }	
        }

        [SerializeField] private LayerMask _notPlayerMask;
        public LayerMask notPlayerMask
        {
            get
            {
                return _notPlayerMask;
            }	
        }

        [SerializeField] private LayerMask _allMarbles;
        public LayerMask allMarbles
        {
            get
            {
                return _allMarbles;
            }	
        }
    
        [SerializeField] private LayerMask _enemyMarbles;
        public LayerMask enemyMarbles
        {
            get
            {
                return _enemyMarbles;
            }	
        }

        [SerializeField] private LayerMask _playerMarble;
        public LayerMask playerMarble
        {
            get
            {
                return _playerMarble;
            }	
        }
    }

}
