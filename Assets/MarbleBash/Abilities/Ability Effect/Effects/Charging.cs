using System;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Charging : AbilityEffect
    {
        public Charging() : base()
        {
            _duration = 1f;
        }

        protected override void Start()
        {
            PlayerCollisionHandler.AssignDamageListener(HitMarble);
            PlayerCollisionHandler.OnCollisionGround += CollisionGround;
        }

        

        protected override void Update()
        {
            Vector3 dir = Player.look.yawForward;

            subject.rigidbody.AddForce(1000f * Time.deltaTime * dir);
        }

        protected override void Finished()
        {
            PlayerCollisionHandler.UnassignDamageListener();
            PlayerCollisionHandler.OnCollisionGround -= CollisionGround;
        }

        private void CollisionGround(Collision collision)
        {
            if (collision.impulse.magnitude > 0.5f)
            {
                StopEffect();
                StunSubjectMarble(collision);
            }
        }

        private void StunSubjectMarble(Collision collision)
        {
            Stunned s = subject.abilityEffects.AddEffect<Stunned>();
            s.Initialise(1.5f);

            Vector3 knockbackDir = (collision.contacts[0].normal + Vector3.up).normalized;
            
            Vector3 knockbackVelocity = subject.movement.cachedSpeed * knockbackDir;
            Vector3 currentVelocity = subject.rigidbody.linearVelocity;

            subject.rigidbody.linearVelocity = (knockbackVelocity * 0.2f) + (currentVelocity * 0.2f);
            subject.rigidbody.angularVelocity = subject.rigidbody.angularVelocity * 0.25f;
        }

        private void HitMarble(Collision c, Marble m)
        {
            Vector3 cachedVelocity = subject.movement.cachedVelocity;
            
            subject.rigidbody.linearVelocity = cachedVelocity;

            Vector3 knockbackDir = cachedVelocity.normalized + (Vector3.up * 0.33f);

            DamageManager.ApplyDamage(subject, m, m.movement.cachedSpeed, knockbackDir);

        }

    }


}

