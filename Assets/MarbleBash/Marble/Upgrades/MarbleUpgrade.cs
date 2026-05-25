using UnityEngine;

namespace MarbleBash.Upgrades
{

    [CreateAssetMenu(fileName = "New Marble Upgrade", menuName = "Marble Bash/Upgrades/New Upgrade", order = 1)]
    public class MarbleUpgrade : ScriptableObject
    {
        [Header("Decorative:")]
        [SerializeField] private string _descritiveName;
        public string descritiveName => _descritiveName;

        [SerializeField, TextArea] private string _description;
        public string description => _description;
    
        [Header("Stats:")]
        [SerializeField] private MarbleUpgradeElement[] _elements;
        public MarbleUpgradeElement[] elements => _elements;
    
    }


}

