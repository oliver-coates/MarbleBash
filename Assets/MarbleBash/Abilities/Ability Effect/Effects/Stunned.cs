using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Stunned : AbilityEffect
    {
        private MutableStatModifier _movementSpeedModifier;

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

        protected override void Start()
        {
            Debug.Log($"STUNNED!");
            // Disable movement speed:
            _movementSpeedModifier = new MutableStatModifier(0f, 0f);
            subject.stats.movementSpeed.AddModifier(_movementSpeedModifier);
        }

        protected override void Update()
        {
        }

        protected override void Finished()
        {
            Debug.Log($"finished stunned!");
            subject.stats.movementSpeed.RemoveModifier(_movementSpeedModifier);
        }

    }

}

