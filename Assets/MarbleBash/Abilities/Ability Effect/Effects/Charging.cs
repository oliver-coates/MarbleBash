using System;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Charging : AbilityEffect
    {
        private MutableStatModifier moveSpeedModifier;


        protected override void Start()
        {
            _duration = 1f;

            subject.collisionHandler.OnCollisionGround += CollisionGround;    
            if (subject.isPlayer)
            {
                subject.collisionHandler.AssignDamageListener(HitMarble);
            }

            // Increase move speed by 50%!
            moveSpeedModifier = new MutableStatModifier(0.5f);
            subject.stats.movementSpeed.AddModifier(moveSpeedModifier);            
        }

        protected override void Update()
        {
        }

        protected override void Finished()
        {
            subject.collisionHandler.UnassignDamageListener();
            if (subject.isPlayer)
            {
                subject.collisionHandler.OnCollisionGround -= CollisionGround;   
            }

            subject.stats.movementSpeed.RemoveModifier(moveSpeedModifier);            
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
            subject.abilityEffects.AddEffect<Stunned>(1.5f);

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

            Vector3 knockbackDir = cachedVelocity.normalized + Vector3.up;

            float damage = m.movement.cachedSpeed;
            DamageManager.ApplyDamage(subject, m, damage, knockbackDir);

            float stunDuration = damage * 0.25f;
            m.abilityEffects.AddEffect<Stunned>(stunDuration);
        }


    }


}

