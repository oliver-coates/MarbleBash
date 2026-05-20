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

        public void Initialise(Marble marble, int amount)
        {
            GetComponents();

            _amount = amount;
            _hoverHeight = 0.25f;

            float size = 0.1f;
            _trigger.radius = size * 100f;
            _trigger.GetComponent<TriggerHandle>().onTriggerEnter += TriggerEnter;
            SetSize(size);

            PositionWithinMarble(marble, size);

            Throw();
        }

        protected override void Update()
        {
            base.Update();

            if (_isChasingPlayer)
            {
                _chasePlayerTimer += Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, _chasePlayerTimer * Time.deltaTime * 2f);

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

