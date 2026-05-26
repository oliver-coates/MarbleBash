using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Rocket : Ability
    {
        private MasksConfig _masks;
        private ProjectileConfig _projectiles;
        public Rocket(Marble subject) : base(subject)
        {
            _masks = Configuration.Get<MasksConfig>();
            _projectiles = Configuration.Get<ProjectileConfig>();
            _name = "Rocket";
        }

        protected override void Activate()
        {
            Vector3 targetPosition = GetTargetPosition();

            var r = GameObject.Instantiate(_projectiles.rocket).GetComponent<RocketProjectile>();
            
            Vector3 start = _subject.transform.position + Vector3.up + Vector3.right;
            r.Initialise(start, targetPosition);
        }

        private Vector3 GetTargetPosition()
        {
            if (_subject.isPlayer)
            {
                Vector2 screenCentre = new Vector2(Screen.width, Screen.height) * 0.5f;

                Ray centerScreenRay = Camera.main.ScreenPointToRay(screenCentre);

                if (Physics.Raycast(centerScreenRay, out RaycastHit info, 100f, _masks.notPlayerMask))
                {
                    return info.point;
                }
                else
                {
                    return _subject.transform.position + _subject.movement.lookDirection * 25f;
                }    
            }
            else
            {
                return (Player.transform.position - _subject.transform.position).normalized;
            }
            
        }

        protected override bool IsAbleToActivate()
        {
            return true;
        }
    }


}

