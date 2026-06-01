using System;
using KahuInteractive.VisualFX;
using MarbleBash.Abilities;
using UnityEngine;

namespace MarbleBash.Enemy
{
    internal class PoundAttack : Tactic
    {
        private const float MAX_DURATION = 3f;
        private const float SPEED_BOOST = 0.75f;
        private const float STUN_TIME_MULTIPLIER = 0.25f;

        private bool _hasPounded;
        private MutableStatModifier _moveSpeedModifier;

        protected override void Start()
        {
            _hasPounded = false;
            _duration = MAX_DURATION;
        
            _marble.abilityTelegraph.Flash();

            _marble.rigidbody.linearVelocity *= 0.5f;
            _marble.movement.AttemptJump();

            _moveSpeedModifier = new MutableStatModifier(MutableStatModifier.Source.Effect, SPEED_BOOST);
            _marble.stats.movementSpeed.AddModifier(_moveSpeedModifier);
        }

        internal override void SetupTransitions()
        {
            // Attempt to jump at player when the dash ability is ready
            TacticTransition doPoundAttack = new TacticTransition(
                this,
                new TransitionCriteria[]
                {
                    new IsAbilityNotOnCooldown(_marble, "Ground Pound"),
                    new IsWithinDistanceToPlayer(_marble, 1f, 4f),
                    _brain.AddCriteria<IsVelocityAlignedAgainstPlayer>()
                }
            );
            _brain.defaultTactic.AddTransition(doPoundAttack);
        }

        protected override void Update()
        {
            Vector3 movementTarget = Player.movement.groundedPosition;
            movementTarget.y = _marble.transform.position.y;
            _marble.movement.MoveTowardsPoint(movementTarget);

            if (_marble.rigidbody.linearVelocity.y < -1f && !_hasPounded)
            {
                bool successfull = _marble.abilities.AttemptActivateAbility("Ground Pound");
                if (successfull)
                {
                    _hasPounded = true;
                    _marble.collisionHandler.OnCollisionGround += HitGround;
                    
                    VFX.Play(new OneShotEffectData(
                        "Ground Pound Indicator",
                        Vector3.zero, Quaternion.identity,
                        1f, _marble.transform));                
                }

            }

        }

        private void HitGround(Collision collision)
        {
            _marble.collisionHandler.OnCollisionGround -= HitGround;
            
            float stunLength = collision.impulse.magnitude * STUN_TIME_MULTIPLIER;
            _marble.statusEffects.AddEffect<Stunned>(stunLength);
        }

        protected override void OnDurationFinished()
        {
            _marble.stats.movementSpeed.RemoveModifier(_moveSpeedModifier);
            _marble.movement.MoveAlongPath();
        }

        protected override void OnTransition()
        {
            _marble.stats.movementSpeed.RemoveModifier(_moveSpeedModifier);
            _marble.movement.MoveAlongPath();
        }


        protected override Tactic GetNextTactic()
        {
            return _brain.defaultTactic;
        }

        internal override string GetName()
        {
            return "Pound Attack";
        }
    }
}

