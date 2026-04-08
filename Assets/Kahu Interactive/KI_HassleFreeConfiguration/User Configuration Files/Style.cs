using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Style Config", menuName = "Configuration/Style")]
    public class StyleConfig : ConfigBase
    {
        [SerializeField] private Color _massColour;
        public Color massColour
        {
            get
            {
                return _massColour;
            }
        }

        [SerializeField] private Color _agilityColour;
        public Color agilityColour
        {
            get
            {
                return _agilityColour;
            }
        }

        [SerializeField] private Color _rechargeColour;
        public Color rechargeColour
        {
            get
            {
                return _rechargeColour;
            }
        }

        [SerializeField] private Color _blockColour;
        public Color blockColour
        {
            get
            {
                return _blockColour;
            }
        }

        [SerializeField] private Color _luckColour;
        public Color luckColour
        {
            get
            {
                return _luckColour;
            }
        }

        [SerializeField] private Color _energyColour;
        public Color energyColour
        {
            get
            {
                return _energyColour;
            }
        }

    }

}
