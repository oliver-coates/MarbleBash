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
            Tactic newTactic = CreateTacticFromName(tacticName);
            newTactic.Initialise(brain);

            return newTactic;
        }

        internal static Tactic CreateTacticFromName(string name)
        {
            switch (name)
            {
                case "Attack":
                    return new Attack();
                
                case "Dash Attack":
                    return new DashAttack();
                
                case "Leap Out Of Chasm":
                    return new LeapOutOfChasm();
                
                case "Pound Attack":
                    return new PoundAttack();

                default:
                    Debug.LogError($"Unhandled tactic name '{name}'");
                    return null;
            }
        }
    }

    // Note: Switched from using an enum to a string
    // internal abstract partial class Tactic
    // {
    //     [System.Serializable]
    //     internal enum Type
    //     {
    //         Idle = 0,
    //         Attack = 1,
    //         DashAttack = 2
    //     }
    // }
}

    