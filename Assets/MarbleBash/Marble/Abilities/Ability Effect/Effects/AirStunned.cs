using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class AirStunned : AbilityEffect
    {
        private MutableStatModifier _movementSpeedModifier;
        private VFX_OneShotHandler _stunParticles;

        private const float TIME_GROUNDED_TO_REMOVE = 0.15f;

        private float _groundedTimer;

        protected override void Start()
        {
            _groundedTimer = 0f;

            // Disable movement speed:
            _movementSpeedModifier = new MutableStatModifier(MutableStatModifier.Source.Effect, 0f, 0f);
            subject.stats.movementSpeed.AddModifier(_movementSpeedModifier);

            // Disable AI
            subject.isStunned = true;            

            // Particles:
            _stunParticles = VFX.Play(new OneShotEffectData("Dazed", Vector3.zero, Quaternion.identity, _duration, subject.transform));
        }

        protected override void Update()
        {
            if (timeElapsed > 0.25f)
            {
                if (subject.movement.isGrounded)
                {
                    _groundedTimer += Time.deltaTime;

                    if (_groundedTimer > TIME_GROUNDED_TO_REMOVE)
                    {
                        StopEffect();                    
                    }
                }            
            }
        }

        protected override void Finished()
        {
            // Add move speed back:
            subject.stats.movementSpeed.RemoveModifier(_movementSpeedModifier);
            
            // Re-enable AI
            subject.isStunned = false;

            // Remove stun particles
            if (_stunParticles != null)
            {
                _stunParticles.Finish();            
            }
        }

    }

}

