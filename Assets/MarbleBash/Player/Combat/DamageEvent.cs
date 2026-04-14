using UnityEngine;

namespace MarbleBash
{

    public class DamageEvent
    {
        // Currently assuming all damage is from marbles, this will need to be changed in future
        [SerializeField] private Marble _damageSource;
        public Marble damageSource
        {
            get
            {
                return _damageSource;
            }	
        }
    
        [SerializeField] private float _amount;
        public float amount
        {
            get
            {
                return _amount;
            }	
        }
    
        [SerializeField] private Vector3 _normal;
        public Vector3 normal
        {
            get
            {
                return _normal;
            }	
        }

        public DamageEvent(Marble source, Marble target, float damageAmount)
        {
            _normal = (source.transform.position - target.transform.position).normalized;
            _amount = damageAmount;
            _damageSource = source; 
        }
    }


}

