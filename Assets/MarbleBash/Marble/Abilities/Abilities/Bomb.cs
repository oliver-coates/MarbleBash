using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Bomb : Ability
    {
        private MasksConfig _masks;
        private ProjectileConfig _projectiles;
        public Bomb(Marble subject) : base(subject)
        {
            _masks = Configuration.Get<MasksConfig>();
            _projectiles = Configuration.Get<ProjectileConfig>();
            _name = "Rocket";
        }

        protected override void Activate()
        {
            Vector3 targetPosition = GetTargetPosition();

            BombProjectile bomb = Object.Instantiate(_projectiles.bomb).GetComponent<BombProjectile>();
            
            // Vector3 start = _subject.transform.position + Vector3.up + Vector3.right;
            if (_subject.isPlayer)
            {
                bomb.Initialise((PlayerInstance) _subject);            
            }
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

