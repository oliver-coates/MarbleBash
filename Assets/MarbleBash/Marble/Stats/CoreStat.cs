using UnityEngine;

namespace MarbleBash
{
    [System.Serializable]
    public class CoreStat
    {
        [SerializeField] private int _value;
        public int value
        {
            get
            {
                return _value;
            }
        }
    
        public CoreStat()
        {
            _value = 1;
        }
    }


}

