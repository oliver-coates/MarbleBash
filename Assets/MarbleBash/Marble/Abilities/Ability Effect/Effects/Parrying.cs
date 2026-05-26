
using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{


    public class Parrying : AbilityEffect
    {
        private float _durationBase;
        private float _knockbackVelocityMultiplier;
        private float _knockbackVelocityUpBonus;
        private float _stunTimeMultiplier;

        private void GetConfigValues()
        {
            _durationBase = Configuration.Read("parry_base_duration");
            _knockbackVelocityMultiplier = Configuration.Read("parry_knockback_velocity_multiplier");
            _knockbackVelocityUpBonus = Configuration.Read("parry_knockback_up_velocity_bonus");
            _stunTimeMultiplier = Configuration.Read("parry_stun_time_multiplier");
        }

        protected override void Start()
        {
            GetConfigValues();
            _duration = _durationBase;

            subject.stats.isImmuneFromDamage = true;
            if (subject.isPlayer)
            {
                subject.collisionHandler.AssignDamageListener(HitMarble);
            }
            else
            {
                subject.collisionHandler.OnCollisionMarble += HitMarble;
            }
        }

        protected override void Finished()
        {
            subject.stats.isImmuneFromDamage = false;
            if (subject.isPlayer)
            {
                subject.collisionHandler.UnassignDamageListener();
            }
            else
            {
                subject.collisionHandler.OnCollisionMarble -= HitMarble;
            }
        }

        protected override void Update()
        {
        }


        private void HitMarble(Collision collision, EnemyInstance instance)
        {
            HitMarble(collision, instance as Marble);
        }
        private void HitMarble(Collision collision, Marble marble)
        {
            // Keep our velocity the same
            subject.rigidbody.linearVelocity = subject.movement.cachedVelocity;

            // Send the other marble flying
            Vector3 knockbackVector = (-marble.movement.cachedVelocity) + (Vector3.up * marble.movement.cachedSpeed * _knockbackVelocityUpBonus);
            marble.rigidbody.linearVelocity = knockbackVector * _knockbackVelocityMultiplier;
            
            // Apply stun
            float stunTime = marble.movement.cachedSpeed * _stunTimeMultiplier;
            marble.statusEffects.AddEffect<Stunned>(stunTime);
        }

        

    }

}

