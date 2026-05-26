using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Charging : AbilityEffect
    {
        private MutableStatModifier moveSpeedModifier;

        private float _baseChargeDuration;
        private float _baseSpeedMultiplier;
        private float _selfStunTimeMultiplier;
        private float _hitStunTimeMultiplier;
        private float _knockbackVelocityUpAmount;

        protected override void Start()
        {
            PullConfigValues();
            _duration = _baseChargeDuration;

            subject.collisionHandler.OnCollisionGround += CollisionGround;    
            if (subject.isPlayer)
            {
                subject.collisionHandler.AssignDamageListener(HitMarble);
            }
            else
            {
                subject.collisionHandler.OnCollisionMarble += HitMarble;
            }

            // Increase move speed
            float speedMultiplier = _baseSpeedMultiplier;
            moveSpeedModifier = new MutableStatModifier(MutableStatModifier.Source.Effect, speedMultiplier);
            subject.stats.movementSpeed.AddModifier(moveSpeedModifier);            
        }

        private void PullConfigValues()
        {
            _baseChargeDuration = Configuration.Read("charge_base_duration");
            _baseSpeedMultiplier = Configuration.Read("charge_base_speed_multiplier");
            _selfStunTimeMultiplier = Configuration.Read("charge_impact_self_stun_time_multiplier");
            _hitStunTimeMultiplier = Configuration.Read("charge_impact_other_stun_time_multiplier");
            _knockbackVelocityUpAmount = Configuration.Read("charge_knockback_velocity_up_amount");
        }

        protected override void Update()
        {
        }

        protected override void Finished()
        {
            if (subject.isPlayer)
            {
                subject.collisionHandler.UnassignDamageListener();
            }
            else
            {
                subject.collisionHandler.OnCollisionMarble -= HitMarble;
            }
            
            subject.collisionHandler.OnCollisionGround -= CollisionGround;   

            subject.stats.movementSpeed.RemoveModifier(moveSpeedModifier);            
        }

        private void CollisionGround(Collision collision)
        {
            if (collision.impulse.ShearTo2D().magnitude > 0.5f)
            {
                StopEffect();
                StunSubjectMarble(collision);
            }
        }

        private void StunSubjectMarble(Collision collision)
        {
            float stunDuration = collision.impulse.magnitude * _selfStunTimeMultiplier;
            subject.statusEffects.AddEffect<Stunned>(stunDuration);

            Vector3 knockbackDir = (collision.contacts[0].normal + Vector3.up).normalized;
            
            Vector3 knockbackVelocity = subject.movement.cachedSpeed * knockbackDir;
            Vector3 currentVelocity = subject.rigidbody.linearVelocity;

            subject.rigidbody.linearVelocity = (knockbackVelocity * 0.2f) + (currentVelocity * 0.2f);
            subject.rigidbody.angularVelocity = subject.rigidbody.angularVelocity * 0.25f;
        }

        private void HitMarble(Collision collision, Marble hitMarble)
        {
            if (hitMarble.stats.isImmuneFromDamage)
            {
                return;
            }
            
            Vector3 cachedVelocity = subject.movement.cachedVelocity;
            subject.rigidbody.linearVelocity = cachedVelocity * 0.9f;

            // Get the knockback direction
            Vector3 knockbackDir = cachedVelocity.normalized + (Vector3.up * _knockbackVelocityUpAmount);
            
            // Offset the hit marble by the hit direction (to prevent follow up hits)
            hitMarble.transform.position += (knockbackDir * 0.25f);

            // Apply damage:
            float damage = hitMarble.movement.cachedSpeed;
            DamageManager.ApplyDamage(subject, hitMarble, damage, knockbackDir);
            
            // Apply stun:
            float stunDuration = damage * _hitStunTimeMultiplier;
            hitMarble.statusEffects.AddEffect<Stunned>(stunDuration);

            
        }


    }


}

