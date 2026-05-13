using UnityEngine;

namespace MarbleBash
{

    public class MutableStatModifier
    {   
        private float _addition;
        public float addition
        {
            get
            {
                return _addition;
            }
        }
    
        private float _multiplier;
        public float multiplier
        {
            get
            {
                return _multiplier;
            }
        }
    
        public MutableStatModifier(float addition = 0f, float multiplier = 1f)
        {
            _addition = addition;
            _multiplier = multiplier;
        }
    }


}

