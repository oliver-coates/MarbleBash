using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Masks Config", menuName = "Configuration/Masks")]
    public class MasksConfig : ConfigBase
    {
        [SerializeField] private LayerMask _groundedLayerMask;
        public LayerMask groundedLayerMask
        {
            get
            {
                return _groundedLayerMask;
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

    }

}
