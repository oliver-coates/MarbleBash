namespace MarbleBash.Enemy
{

    internal class IsAbilityNotOnCooldown : TransitionCriteria
    {
        private string _abilityName;
        internal IsAbilityNotOnCooldown(Marble subject, string abilityName)
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