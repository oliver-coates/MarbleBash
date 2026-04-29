using System;
using TMPro;
using UnityEngine;

namespace MarbleBash
{

    public class FloatingDamageNumberManager : MonoBehaviour
    {
        [Header("References:")]
        [SerializeField] private GameObject _damageNumberPrefab;



        #region Initialisation & Destruction
        void Start()
        {
            MarbleHealth.OnDamageTakenGlobal += OnDamageEvent;
        }

        private void OnDestroy()
        {
            MarbleHealth.OnDamageTakenGlobal -= OnDamageEvent;
        }
        #endregion



        private void OnDamageEvent(MarbleHealth.HealthChangedEvent healthChangeEvent)
        {
            // Ignore if this is the player marble
            if (healthChangeEvent.marble == Player.instance)
            {
                return;
            }

            if (healthChangeEvent.totalHealthChange < 0)
            {
                CreateDamageNumber(healthChangeEvent.marble, Mathf.Abs(healthChangeEvent.totalHealthChange));
            }
        }

        private void CreateDamageNumber(Marble marble, float damage)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(marble.transform.position);
            GameObject num = Instantiate(_damageNumberPrefab, transform);
            num.GetComponent<FloatingDamageNumber>().Setup(pos, damage);
        }
    }


}

