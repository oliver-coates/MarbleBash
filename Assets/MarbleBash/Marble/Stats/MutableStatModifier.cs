using UnityEngine;

namespace MarbleBash
{

    public class MutableStatModifier
    {   
        public enum Source
        {
            Effect,
            Upgrade,
        }

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
        
        private Source _source;
        public Source source => _source;

        public MutableStatModifier(Source source, float addition = 0f, float multiplier = 1f)
        {
            _source = source;
            _addition = addition;
            _multiplier = multiplier;
        }
    }


}

