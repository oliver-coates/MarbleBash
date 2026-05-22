using UnityEngine;

namespace MarbleBash.Abilities
{

    public abstract class EnemyAbilityUseRequirement
    {   
        protected Marble _subject;

        public EnemyAbilityUseRequirement(Marble subject)
        {
            _subject = subject;
        }

        public abstract bool Evaluate();
    }

    public class BeNearPlayerRequirement : EnemyAbilityUseRequirement
    {
        public BeNearPlayerRequirement(Marble subject) : base(subject)
        {
        }

        public override bool Evaluate()
        {
            return Vector3.Distance(_subject.transform.position, Player.transform.position) < 5f;
        }
    }

    public class MustSeePlayerRequirement : EnemyAbilityUseRequirement
    {
        public MustSeePlayerRequirement(Marble subject) : base(subject)
        {
        }

        public override bool Evaluate()
        {
            Vector3 directionToPlayer = (Player.transform.position - _subject.transform.position).normalized;
            Ray ray = new Ray(_subject.transform.position + directionToPlayer, directionToPlayer);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            return false;
        }
    }

}

