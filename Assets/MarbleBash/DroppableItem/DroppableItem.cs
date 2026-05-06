using System;
using UnityEngine;

namespace MarbleBash
{

    public abstract class DroppableItem : MonoBehaviour
    {   
        protected float _timer;
        private bool _isOnGround;  
        private Rigidbody _rb;      


        public void Initialise(Vector3 position)
        {
            _rb = this.GetComponentSafe<Rigidbody>();

            transform.position = position;

            Throw();
        }

        private void HitGround()
        {
            _isOnGround = true;
            Destroy(_rb);
            Debug.Log($"hit ground");
            OnHitGround();
        }
        protected abstract void OnHitGround();

        protected void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > 0.1f && !_isOnGround )
            {
                Ray ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, 0.1f))
                {
                    HitGround();
                }    
            }
            
            
        }


        private Vector3 GetThrowDirection()
        {
            Vector2 randomOnCircle = UnityEngine.Random.onUnitCircle * UnityEngine.Random.Range(0.75f, 1f);
            Vector3 direction = new Vector3(randomOnCircle.x, 1, randomOnCircle.y);
        
            return direction.normalized;
        }

        private void Throw()
        {
            Vector3 dir = GetThrowDirection();
            float throwForce = 10 * UnityEngine.Random.Range(0.75f, 1f);
            
            _rb.AddForce(dir * throwForce, ForceMode.VelocityChange);

        }
    
        
        
    }


}

