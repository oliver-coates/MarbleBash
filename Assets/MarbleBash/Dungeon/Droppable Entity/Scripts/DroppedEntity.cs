using System;
using UnityEngine;

namespace MarbleBash
{

    public abstract class DroppedEntity : MonoBehaviour
    {   

        // References:
        protected Rigidbody _rb;      
        protected TrailRenderer _trail;

        // Settings:
        private float _size = 0.1f;
        protected float size
        {
            get
            {
                return _size;
            }
        }
        protected float _hoverHeight = 0.25f;
        
        // State:
        protected float _timeAlive;
        protected bool _isOnGround;
          
        
        /// <summary>
        /// Gets the components that this object uses.
        /// Call this from your initialisation method.
        /// </summary>
        protected void GetComponents()
        {
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

        /// <summary>
        /// Throws this item in a random direction.
        /// </summary>
        /// <param name="velocity"> How much force the item should be thrown with. </param>
        protected void Throw(float velocity, Vector3 direction)
        {
            Vector3 upDir = Vector3.up;
            
            float verticality = UnityEngine.Random.Range(0.35f, 0.85f);

            Vector3 throwForce = velocity * Vector3.Lerp(direction, upDir, verticality);
            
            _rb.AddForce(throwForce, ForceMode.VelocityChange);

        }

        protected void SetSize(float size)
        {
            _size = size;

            transform.localScale = new Vector3(size, size, size);
            _trail.startWidth = size;

        }
           
        protected Vector3 GetRandomThrowDirection()
        {
            Vector2 randomOnCircle = UnityEngine.Random.onUnitCircle;
            Vector3 direction = new (randomOnCircle.x, 0f, randomOnCircle.y);
        
            return direction.normalized;
        }

        protected float GetRandomThrowForce()
        {
            float throwForceVariance = 0.66f;
            float throwForce = 10f * (1 + UnityEngine.Random.Range(-throwForceVariance, throwForceVariance));
            
            return throwForce;
        }
         
        /// <summary>
        /// Positions the gameobject to be in a random point within the marble.
        /// </summary>
        /// <param name="upShift">Moves the point upwards, leave at 0 to be entirely within the marble</param>
        protected void PositionWithinMarble(Vector3 marbleCenter, float marbleRadius, float upShift = 0)
        {
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * marbleRadius;
            Vector3 upShiftVector = new Vector3(0, upShift, 0);

            transform.position = marbleCenter + randomPosition + upShiftVector;
        }
    }


}

