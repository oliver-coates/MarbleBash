using KahuInteractive.VisualFX;
using MarbleBash.Abilities;
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

        [SerializeField] private GameObject _explosionPrefab;

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

        private void Update()
        {
            _lifeTime += Time.deltaTime;

            if (_lifeTime < THROW_TIME)
            {
                LobUpdate();
            }
            else if (_isThrown == false)
            {
                Throw();
            }
        }

        private void Throw()
        {
            _isThrown = true;

            _rb.isKinematic = false;
            _rb.useGravity = true;

            _collider.enabled = true;

            _rb.linearVelocity = _playerMarble.movement.velocity;

            float velocityAlignment = Mathf.Clamp(Vector3.Dot(_playerMarble.movement.velocity.normalized, _playerMarble.movement.look.pitchForward), 0, 1);

            Vector3 inhertiedVelocity = _playerMarble.movement.velocity * velocityAlignment;
            Vector3 forceDir = (_playerMarble.movement.look.pitchForward + (Vector3.up * 0.25f)).normalized;

            Vector3 force = (forceDir * 750) + inhertiedVelocity;
            _rb.AddForce(force);
        }

        private void LobUpdate()
        {
            Vector3 _rearPosition = -(_playerMarble.movement.look.pitchForward * 2f);
            Vector3 _throwPosition = _playerMarble.movement.look.pitchRight + _playerMarble.movement.look.pitchUp;

            Vector3 targetPosition = Vector3.Lerp(_rearPosition, _throwPosition, _lifeTime / THROW_TIME);
            _offset = Vector3.Lerp(_offset, targetPosition, Time.deltaTime * 2f);

            transform.position = _playerMarble.transform.position + _offset;
        }

        private void OnCollisionEnter(Collision c)
        {
            ContactPoint contact = c.contacts[0];

            CreateExplosion(contact.point, contact.normal);
            
            Destroy(gameObject);
        }

        private void CreateExplosion(Vector3 position, Vector3 normal)
        {
            BombExplosion explosion = Instantiate(_explosionPrefab).GetComponent<BombExplosion>();
            explosion.Initialise(position, normal, 1.5f, 10f, true, Player.instance);
        }
    }


}

