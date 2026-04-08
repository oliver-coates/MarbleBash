using UnityEngine;

namespace MarbleBash.Abilities
{

    public class EnemyAbilitiesInput : MonoBehaviour
    {
        [SerializeField] private float _timeToActivate = 2;
        [SerializeField] private float _timer;

        private Abilities _abilities;

        private void Start()
        {
            _abilities = this.GetComponentSafe<Abilities>();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            if (_timer < 0)
            {
                _timer = _timeToActivate;

                int abilityRandomIndex = Random.Range(0, 4);

                _abilities.AttemptActivateAbility(abilityRandomIndex);
            }
        }
    }


}
