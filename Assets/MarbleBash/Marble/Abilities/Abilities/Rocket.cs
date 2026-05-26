using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Rocket : Ability
    {
        private MasksConfig _masks;
        public Rocket(Marble subject) : base(subject)
        {
            _masks = Configuration.Get<MasksConfig>();
            _name = "Rocket";
        }

        protected override void Activate()
        {
            Vector3 targetPosition = GetTargetPosition();

            Debug.DrawLine(_subject.transform.position, targetPosition, Color.red);
            Debug.Break();
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

