using UnityEngine;

namespace MarbleBash
{
    /// <summary>
    /// Marble class acts as a shared access point for abilities to act upon.
    /// When an ability does a certain action, it is applied against the components
    /// defined in this class. (Example: the dash ability pushes the rigidbody in
    /// the direction of _lookDirection)
    /// </summary>
    [System.Serializable]
    public abstract class Marble : MonoBehaviour
    {
        [Header("Stats:")]
        [SerializeField] protected MarbleStats _stats;
        public MarbleStats stats
        {
            get
            {
                return _stats;
            }
        }

        [Header("Health:")]
        [SerializeField] protected MarbleHealth _health;
        public MarbleHealth health
        {
            get
            {
                return _health;
            }
        }


        private Vector3 _cachedVelocity;
        public Vector3 cachedVelocity
        {
            get
            {
                return _cachedVelocity;
            }
        }

        public Vector3 lookDirection
        {
            get
            {
                return GetLookDirection();
            }
        }

        protected Rigidbody _rigidbody;
        public new Rigidbody rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }


        private void Start()
        {
            _health = new MarbleHealth(this);
            Setup();

            Debug.Assert(_rigidbody != null);

            // Setup subcomponents:
            foreach (MarbleSubComponent subcomponent in transform.GetComponentsInChildren<MarbleSubComponent>())
            {
                subcomponent.Initialise(this);
            }
        }

        private void FixedUpdate()
        {
            _cachedVelocity = _rigidbody.linearVelocity;
        }

        protected abstract void Setup();

        protected abstract Vector3 GetLookDirection();

        

    }
}
