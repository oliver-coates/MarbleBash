using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Stunned : AbilityEffect
    {
        private MutableStatModifier _movementSpeedModifier;

        public void Initialise(float duration)
        {
            _duration = duration;
        
            // Disable movement speed:
            _movementSpeedModifier = new MutableStatModifier(0f, 0f);
            subject.stats.movementSpeed.AddModifier(_movementSpeedModifier);

            // Particles:
            VFX.Play(new OneShotEffectData("Dazed", Vector3.zero, Quaternion.identity, _duration, subject.transform));
        }

        protected override void Start()
        {
            
        }

        protected override void Update()
        {
        }

        protected override void Finished()
        {
            subject.stats.movementSpeed.RemoveModifier(_movementSpeedModifier);
        }

    }

}

