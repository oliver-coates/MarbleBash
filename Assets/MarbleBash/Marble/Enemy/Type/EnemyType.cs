using System.Collections.Generic;
using KahuInteractive.HassleFreeConfig;
using MarbleBash.Upgrades;
using UnityEngine;

namespace MarbleBash.Enemy
{

    [CreateAssetMenu(fileName = "New Marble Type", menuName = "Marble Bash/Enemies/Enemy Type", order = 1)]
    public class EnemyType : ScriptableObject
    {
        [Header("Decorative")]
        [SerializeField] private string _descriptiveName;
        public string descriptiveName
        {
            get
            {
                return _descriptiveName;
            }	
        }
        
        [SerializeField] private Color _color;
        public Color color => _color;
    
        [Header("Upgrades:")]
        [SerializeField] private MarbleUpgrade[] _upgrades;
        public MarbleUpgrade[] upgrades => _upgrades;
    }


}

