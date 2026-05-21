using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    internal abstract class ChestLoot
    {
        internal abstract void Spawn(Vector3 position, Quaternion rotation);
    }

    internal class QuartzChestLoot : ChestLoot
    {
        private GameObject _prefab;
        private int _amount;

        public QuartzChestLoot(int amount)
        {
            _prefab = Configuration.Get<CombatConfig>().quartzPrefab;    
            _amount = amount;
        }

        internal override void Spawn(Vector3 position, Quaternion rotation)
        {
            QuartzCrystal quartz = GameObject.Instantiate(_prefab).GetComponent<QuartzCrystal>();

            quartz.Initialise(_amount, position, rotation.eulerAngles);
        }
    }
}

