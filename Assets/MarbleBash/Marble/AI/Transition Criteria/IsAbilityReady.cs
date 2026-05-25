namespace MarbleBash.Enemy
{

    internal class IsAbilityReady : TransitionCriteria
    {
        private string _abilityName;
        internal IsAbilityReady(Marble subject, string abilityName)
        {
            Initialise(subject);
            _abilityName = abilityName;
        }



        internal override bool Evaluate()
        {
            return !_subject.abilities.IsAbilityOnCooldown(_abilityName);
        }
    }
}