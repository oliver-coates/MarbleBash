using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Parry : Ability
    {
        public Parry(Marble subject) : base(subject)
        {
            _name = "Parry";
        }

        protected override void Activate()
        {
            _subject.abilityTelegraph.Flash();
            _subject.statusEffects.AddEffect<Parrying>();
        }

        protected override bool IsAbleToActivate()
        {
            return true;
        }
    }


}

