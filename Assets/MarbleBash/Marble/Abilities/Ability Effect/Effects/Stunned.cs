using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Stunned : AbilityEffect
    {
        private MutableStatModifier _movementSpeedModifier;

        protected override void Start()
        {
            // Disable movement speed:
            _movementSpeedModifier = new MutableStatModifier(MutableStatModifier.Source.Effect, 0f, 0f);
            subject.stats.movementSpeed.AddModifier(_movementSpeedModifier);

            // Disable AI
            if (!subject.isPlayer)
            {
                (subject as EnemyInstance).brain.isStunned = true;            
            }

            // Particles:
            VFX.Play(new OneShotEffectData("Dazed", Vector3.zero, Quaternion.identity, _duration, subject.transform));
        }

        protected override void Update()
        {
        }

        protected override void Finished()
        {
            subject.stats.movementSpeed.RemoveModifier(_movementSpeedModifier);
            
            // Re-enable AI
            if (!subject.isPlayer)
            {
                (subject as EnemyInstance).brain.isStunned = false;            
            }
        }

    }

}

