using UnityEngine;

namespace MarbleBash.Enemy
{
    internal static class EnemyBrainInitialiser
    {
        internal static void SetupBrain(EnemyBrain brain, EnemyClass enemyClass)
        {
            EnemyTacticTypeCategory[] categories = enemyClass.GetTactics();
            Tactic[] tactics = new Tactic[categories.Length];

            for (int tacticIndex = 0; tacticIndex < categories.Length; tacticIndex++)
            {
                EnemyTacticTypeCategory category = categories[tacticIndex];
                tactics[tacticIndex] = CreateTactic(category.tacticName, brain);
            }

            brain.Initialise(tactics);
        }

        internal static Tactic CreateTactic(string tacticName, EnemyBrain brain)
        {
            Tactic newTactic = Tactic.CreateFromName(tacticName);
            newTactic.Initialise(brain);

            return newTactic;
        }
    }
}

    