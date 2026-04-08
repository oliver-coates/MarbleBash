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
        public Vector3 lookDirection
        {
            get
            {
                return GetLookDirection();
            }
        }

        protected Rigidbody _rb;
        public Rigidbody rb
        {
            get
            {
                return _rb;
            }
        }

        [SerializeField] protected MarbleStats _stats;
        public MarbleStats stats
        {
            get
            {
                return _stats;
            }
        }


        private void Start()
        {
            Setup();

            Debug.Assert(_rb != null);
        }


        protected abstract void Setup();

        protected abstract Vector3 GetLookDirection();

    }
}
