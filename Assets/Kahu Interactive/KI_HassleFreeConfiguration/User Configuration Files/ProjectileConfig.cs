using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Projectile Config", menuName = "Configuration/Projectiles")]
    public class ProjectileConfig : ConfigBase
    {
        [SerializeField] private GameObject _bomb;
        public GameObject bomb => _bomb;

    }

}
