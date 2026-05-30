using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class AirStunned : AbilityEffect
    {
        private MutableStatModifier _movementSpeedModifier;
        private VFX_OneShotHandler _stunParticles;
        protected override void Start()
        {
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
                    StopEffect();
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

