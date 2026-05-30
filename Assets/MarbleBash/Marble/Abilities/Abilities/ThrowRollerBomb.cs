using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class ThrowRollerBomb : Ability
    {

        public ThrowRollerBomb(Marble subject) : base(subject)
        {
            _name = "Throw Roller Bomb";
        }

        protected override void Activate()
        {
            CombatConfig config = Configuration.Get<CombatConfig>();

            float radius = _subject.transform.localScale.x / 2f;

            Vector3 startPos = _subject.transform.position;

            RollerBomb newRollerBomb = Object.Instantiate(config.rollerBombPrefab, startPos, Quaternion.identity).GetComponent<RollerBomb>();
            newRollerBomb.Initialise();
        }

        protected override bool IsAbleToActivate()
        {
            // TODO: Ensure this marble can see the player
            return true;
        }
    }

}


