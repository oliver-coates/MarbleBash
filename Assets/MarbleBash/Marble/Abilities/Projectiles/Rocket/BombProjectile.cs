using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash
{

    public class BombProjectile : MonoBehaviour
    {
        
        private const float THROW_TIME = 0.125f;

        private Rigidbody _rb;
        private SphereCollider _collider;

        private PlayerInstance _playerMarble;
        private Vector3 _offset;
        private bool _isThrown;
        private float _lifeTime;


        public void Initialise(PlayerInstance player)
        {
            _playerMarble = player;

            _rb = this.GetComponentSafe<Rigidbody>();
            _rb.isKinematic = true;
            _rb.useGravity = false;
            
            _collider = this.GetComponentSafe<SphereCollider>();
            _collider.enabled = false;
            
            _lifeTime = 0f;

            transform.position = player.transform.position;
        }

        // public void Initialise(Vector3 startPos, Vector3 targetPosition)
        // {
        //     Vector3 direction = (targetPosition - startPos).normalized;
        
        //     transform.position = startPos;
        //     transform.rotation = Quaternion.LookRotation(direction);
        // }

        // private void Update()
        // {
        //     float speed = Time.deltaTime * 25f;
        //     transform.position = transform.position + transform.forward * speed;
        // }

        private void Update()
        {
            _lifeTime += Time.deltaTime;

            if (_lifeTime < THROW_TIME)
            {
                Vector3 _rearPosition = -(_playerMarble.movement.look.pitchForward * 2f);
                Vector3 _throwPosition = _playerMarble.movement.look.pitchRight + _playerMarble.movement.look.pitchUp;

                Vector3 targetPosition = Vector3.Lerp(_rearPosition, _throwPosition, _lifeTime / THROW_TIME);
                _offset = Vector3.Lerp(_offset, targetPosition, Time.deltaTime * 4f);
                
                transform.position = _playerMarble.transform.position  + _offset; 
            }
            else
            {
                if (_isThrown == false)
                {
                    _isThrown = true;

                    _rb.isKinematic = false;
                    _rb.useGravity = true;

                    _collider.enabled = true;

                    _rb.linearVelocity = _playerMarble.movement.velocity;

                    float velocityAlignment = Mathf.Clamp(Vector3.Dot(_playerMarble.movement.velocity.normalized, _playerMarble.movement.look.pitchForward), 0, 1);
                    
                    Vector3 inhertiedVelocity = _playerMarble.movement.velocity * velocityAlignment;
                    Vector3 forceDir = (_playerMarble.movement.look.pitchForward + ( Vector3.up* 0.25f) ).normalized;
                    
                    Vector3 force = (forceDir * 750) + inhertiedVelocity;
                    _rb.AddForce(force);
                }
            }
        }

        private void OnCollisionEnter(Collision c)
        {
            OneShotEffectData data = new OneShotEffectData("Ground Pound", c.contacts[0].point, Quaternion.identity, 3f);
            VFX.Play(data);

            Destroy(gameObject);
        }
    }


}

