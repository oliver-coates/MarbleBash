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

        /// <summary>
        /// The direction of the knockback caused by this damage, 
        /// </summary>
        public Vector3 knockbackDirection;

        /// <summary>
        /// Whether effects like floating damage numbers should be shown for this event.
        /// True by default
        /// </summary>
        public bool doDamageEffects;

        public DamageEvent(Marble source, Marble target)
        {
            _source = source; 
            _target = target;

            _location = target.transform.position;

            doDamageEffects = true;
            
        }

    }


}

