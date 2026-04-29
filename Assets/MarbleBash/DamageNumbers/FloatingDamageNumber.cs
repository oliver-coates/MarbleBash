using TMPro;
using UnityEngine;

namespace MarbleBash
{

    public class FloatingDamageNumber : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _transform;

        /// <summary>
        /// How far this text drifts left/right each frame.
        /// 0 means it goes straight up
        /// </summary>
        private float _drift;

        private float _lifeTimer;

        public void Setup(Vector2 position, float damage)
        {
            Vector2 randomisedPosition = position + Random.insideUnitCircle * 10f;

            Resize();

            _transform.anchoredPosition = randomisedPosition;
        
            _text.text = $"-{damage:#}";

            _drift = Random.Range(-0.5f, 0.5f);

            Destroy(gameObject, 1f);
        }

        private void Update()
        {
            Vector2 movementDrift = (Vector2.up + (Vector2.right * _drift)) * Time.deltaTime * 50;
            _transform.anchoredPosition += movementDrift;
        
            _text.color = new Color(1f, 1f, 1f, 1f - _lifeTimer);

            _lifeTimer += Time.deltaTime;
        }

        private void Resize()
        {
            float scaleFactor = Screen.width / 2560f;

            _transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
}

