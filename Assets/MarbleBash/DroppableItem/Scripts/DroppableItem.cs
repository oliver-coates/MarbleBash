using System;
using UnityEngine;

namespace MarbleBash
{

    public abstract class DroppableItem : MonoBehaviour
    {   
        // References:
        private Rigidbody _rb;      
        private TrailRenderer _trail;

        // Settings:
        private float _size = 0.1f;
        protected float _throwForce = 0.5f;
        protected float _throwForceVariance = 0.25f;
        protected float _hoverHeight = 0.25f;
        
        // State:
        protected float _timer;
        private bool _isOnGround;  
        

        public void Initialise(Vector3 position)
        {
            _rb = this.GetComponentSafe<Rigidbody>();
            _trail = this.GetComponentSafe<TrailRenderer>();

            transform.position = position;

            // These settings should be set by the subclass:
            _throwForce = 12f;
            _throwForceVariance = 0.66f;
            _hoverHeight = 0.5f;

            Throw();

            // This will eventually be called by the subclass:
            SetSize(UnityEngine.Random.Range(0.1f, 0.25f));
        }


        private void HitGround()
        {
            _isOnGround = true;
            Destroy(_rb);
            OnHitGround();
        }
        protected abstract void OnHitGround();

        protected void Update()
        {
            _timer += Time.deltaTime;

            if (!_isOnGround)
            {
                if (_rb.linearVelocity.y < 0)
                {
                    CheckIfHitGround();    
                }       
            }
            
        }

        private void CheckIfHitGround()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, _hoverHeight, LayerMask.GetMask("Default")))
            {
                HitGround();
            }
        }

        private Vector3 GetThrowDirection()
        {
            Vector2 randomOnCircle = UnityEngine.Random.onUnitCircle * UnityEngine.Random.Range(0.75f, 1f);
            Vector3 direction = new (randomOnCircle.x, 1.2f, randomOnCircle.y);
        
            return direction.normalized;
        }

        private void Throw()
        {
            Vector3 dir = GetThrowDirection();
            float throwForce = _throwForce * (1 + UnityEngine.Random.Range(-_throwForceVariance, _throwForceVariance));
            
            _rb.AddForce(dir * throwForce, ForceMode.VelocityChange);

        }


        protected void SetSize(float size)
        {
            _size = size;

            transform.localScale = new Vector3(size, size, size);
            _trail.startWidth = size;

        }
        
        
    }


}

