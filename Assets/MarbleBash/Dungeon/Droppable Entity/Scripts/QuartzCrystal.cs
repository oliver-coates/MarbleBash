using System;
using UnityEngine;

namespace MarbleBash
{
    public class QuartzCrystal : DroppedEntity
    {
        private int _amount;
        private bool _isChasingPlayer;
        private float _chasePlayerTimer;

        [Header("References:")]
        [SerializeField] private SphereCollider _trigger;

        /// <summary>
        /// Initialisation using a marble rather than a provided position and spawn radius
        /// </summary>
        /// <param name="marble"></param>
        /// <param name="amount"></param>
        public void Initialise(Marble marble, int amount)
        {
            float radius = marble.transform.localScale.x / 2f;
            Initialise(amount, marble.transform.position, radius);
        }

        public void Initialise(int amount, Vector3 position, float spawnRadius)
        {
            GetComponents();

            _amount = amount;
            _hoverHeight = 0.25f;
          
            SetupSize();

            PositionWithinMarble(position, spawnRadius, size);

            Vector3 throwDirection = GetRandomThrowDirection();
            float force = GetRandomThrowForce();
            Throw(force, throwDirection);
        }

        public void Initialise(int amount, Vector3 position, Vector3 direction)
        {
            GetComponents();

            _amount = amount;
            _hoverHeight = 0.25f;

            SetupSize();
            transform.position = position;

            float force = GetRandomThrowForce();
            Throw(force, direction);
        }

        private void SetupSize()
        {
            float size = 0.1f;
            _trigger.radius = size * 300f;
            _trigger.GetComponent<TriggerHandle>().onTriggerEnter += TriggerEnter;

            SetSize(size);
        }

        protected override void Update()
        {
            base.Update();

            if (_isChasingPlayer)
            {
                _chasePlayerTimer += Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, _chasePlayerTimer * Time.deltaTime * 5f);

                if (Vector3.Distance(transform.position, Player.transform.position) < size)
                {
                    HitPlayer();
                }            
            }
        }

        private void HitPlayer()
        {
            Destroy(gameObject);
        }

        protected override void OnHitGround()
        {
            
        }

        private void TriggerEnter(Collider other)
        {
            if (_isOnGround)
            {
                if (other.CompareTag("Player"))
                {
                    if (_isChasingPlayer == false)
                    {
                        ChasePlayer();
                    }
                }
            }
        }

        private void ChasePlayer()
        {
            _chasePlayerTimer = 1f;
            _isChasingPlayer = true;
        }
    }
}

