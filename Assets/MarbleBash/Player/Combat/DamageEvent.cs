using UnityEngine;

namespace MarbleBash
{

    public class DamageEvent
    {
        
        [SerializeField] private Marble _source;
        /// <summary>
        /// The marble that caused the damage.
        /// Currently assuming all damage is from marbles, this will need to be changed in future
        /// </summary>
        public Marble source
        {
            get
            {
                return _source;
            }	
        }
    
        
        [SerializeField] private Marble _target;
        /// <summary>
        /// The marble that recieved the damage.
        /// </summary>
        public Marble target
        {
            get
            {
                return _target;
            }	
        }

        
        [SerializeField] private Vector3 _direction;
        /// <summary>
        /// The direction of the damage, from the center of the damaging marble towards the damaged marble.
        /// Calculated automatically in the constructor
        /// </summary>
        public Vector3 direction
        {
            get
            {
                return _direction;
            }	
        }

        
        [SerializeField] private Vector3 _location;
        /// <summary>
        /// The position of the damaged marble in world space.
        /// Calculated automatically in the constructor.
        /// </summary>
        public Vector3 location
        {
            get
            {
                return _location;
            }	
        }


        /// <summary>
        /// Amount of damage.
        /// </summary>
        public float amount;

        /// <summary>
        /// The amount of knockback.
        /// </summary>
        public float knockbackAmount;

        public DamageEvent(Marble source, Marble target)
        {
            _source = source; 
            _target = target;

            _location = target.transform.position;
            _direction = (source.transform.position - target.transform.position).normalized;
        }
    

    }


}

