using UnityEngine;

namespace MarbleBash.Enemy
{
    [System.Serializable]
    public class EnemyLevelUpProfile
    {
        private static readonly CoreStatType[] Types = {CoreStatType.Mass, CoreStatType.Agility, CoreStatType.Recharge, CoreStatType.Block, CoreStatType.Luck, CoreStatType.Energy}; 
        private static readonly int numStats = Types.Length;


        [SerializeField, Min(0)] private float _massWeight = 0.2f;
        public float massWeight => _massWeight;

        [SerializeField, Min(0)] private float _agilityWeight = 0.2f;
        public float agilityWeight => _agilityWeight;

        [SerializeField, Min(0)] private float _rechargeWeight = 0.2f;
        public float rechargeWeight => _rechargeWeight;

        [SerializeField, Min(0)] private float _blockWeight = 0.2f;
        public float blockWeight => _blockWeight;

        [SerializeField, Min(0)] private float _luckWeight = 0f;
        public float luckWeight => _luckWeight;

        [SerializeField, Min(0)] private float _energyWeight = 0.2f;
        public float energyWeight => _energyWeight;
    

        public CoreStatType PickStatType()
        {
            float[] weights = {_massWeight, _agilityWeight, _rechargeWeight, _blockWeight, _luckWeight, _energyWeight};

            // Assembe all of our weights into brackets, each spanning from a minimum value to the next bracket's minimum value.
            StatTypeBracket[] brackets = new StatTypeBracket[numStats];
            float total = 0f;
            for (int weightIndex = 0; weightIndex < numStats; weightIndex++)
            {
                total += weights[weightIndex];

                brackets[weightIndex] = new StatTypeBracket()
                {
                    threshold = total,
                    type = Types[weightIndex]
                };

            }            

            // Pick a random number and get the type that sits within that bracket
            float random = UnityEngine.Random.Range(0f, total);
            for (int bracketIndex = 0; bracketIndex < numStats; bracketIndex++)
            {
                StatTypeBracket bracket = brackets[bracketIndex];
                if (random < bracket.threshold)
                {
                    return bracket.type;
                }
            }
            
            return 0;
        }

        private class StatTypeBracket
        {
            public float threshold;
            public CoreStatType type;
        }
    }

}


