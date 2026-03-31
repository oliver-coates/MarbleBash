using UnityEngine;
using UnityEngine.AI;

namespace MarbleBash.Enemy
{
    public class EnemyInstance : MonoBehaviour
    {
        private Rigidbody _rb;
        public Rigidbody rb
        {
            get
            {
                return _rb;
            }
        }

        private void Start()
        {
            _rb = this.GetComponentSafe<Rigidbody>();
        }        
    }


}
