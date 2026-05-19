namespace MarbleBash.Enemy
{

    internal class IsAbilityReady : TransitionCriteria
    {
        private string _abilityName;
        internal IsAbilityReady(EnemyBrain brain, string abilityName) : base(brain)
        {
            _abilityName = abilityName;
        }



        internal override bool Evaluate()
        {
            return !_subject.abilities.IsAbilityOnCooldown(_abilityName);
        }
    }
}