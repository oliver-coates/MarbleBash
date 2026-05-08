using System;
using UnityEngine;

namespace MarbleBash
{

    public abstract class DroppedEntity : MonoBehaviour
    {   
        // References:
        private Rigidbody _rb;      
        private TrailRenderer _trail;

        // Settings:
        private float _size = 0.1f;
        protected float _hoverHeight = 0.25f;
        
        // State:
        protected float _timeAlive;
        protected bool _isOnGround;
          
        

        protected void InitialiseInternal()
        {
            // Grab components & Set position:
            _rb = this.GetComponentSafe<Rigidbody>();
            _trail = this.GetComponentSafe<TrailRenderer>();
        }


        private void HitGround()
        {
            _isOnGround = true;
            Destroy(_rb);
            OnHitGround();
        }
        protected abstract void OnHitGround();

        protected virtual void Update()
        {
            _timeAlive += Time.deltaTime;

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
            else if (transform.position.y < -30f)
            {
                HitGround();
            }
        }

        private Vector3 GetThrowDirection()
        {
            Vector2 randomOnCircle = UnityEngine.Random.onUnitCircle;
            Vector3 direction = new (randomOnCircle.x, 0f, randomOnCircle.y);
        
            return direction.normalized;
        }

        /// <summary>
        /// Throws this item in a random direction.
        /// </summary>
        /// <param name="velocity"> How much force the item should be thrown with. </param>
        /// <param name="forceVerticality"> A value of 0 will throw the item horizontallty, a value of 1 will throw it directly up.</param>
        protected void Throw(float velocity, float forceVerticality)
        {
            Vector3 horizontalDir = GetThrowDirection();
            Vector3 upDir = Vector3.up;

            Vector3 throwForce = velocity * Vector3.Lerp(horizontalDir, upDir, forceVerticality);
            
            _rb.AddForce(throwForce, ForceMode.VelocityChange);

        }


        protected void SetSize(float size)
        {
            _size = size;

            transform.localScale = new Vector3(size, size, size);
            _trail.startWidth = size;

        }
        
        
    }


}

