using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MarbleBash.UI
{

    public class LevelView : MonoBehaviour
    {
        [Header("References:")]
        [SerializeField] private Image _xpFillRing;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("State:")]
        [SerializeField] private float _ringFillTarget;

        private void Start()
        {
            Player.instance.stats.OnXpChanged += PlayerXpChanged;
            Player.instance.stats.OnLevelUp += PlayerLevelUp;

            _ringFillTarget = 0f;
            _xpFillRing.fillAmount = 0;
            RedrawLevelText();
        }

        private void OnDestroy()
        {
            Player.instance.stats.OnXpChanged -= PlayerXpChanged;
            Player.instance.stats.OnLevelUp -= PlayerLevelUp;

        }

        private void PlayerXpChanged(int amount)
        {
            Debug.Log($"XP Changed");
            RecalculateRingFillTarget();
        }

        private void PlayerLevelUp(int level)
        {
            RecalculateRingFillTarget();
            RedrawLevelText();            
        }

        private void RecalculateRingFillTarget()
        {
            float xpPercentage = (float)Player.instance.stats.xp / (float)Player.instance.stats.xpNeededForLevelUp;
            
            _ringFillTarget = xpPercentage;
        }

        private void RedrawLevelText()
        {
            _levelText.text = Player.instance.stats.level.ToString();
        }
    
        private void Update()
        {
            _xpFillRing.fillAmount = Mathf.Lerp(_xpFillRing.fillAmount, _ringFillTarget, Time.deltaTime * 2f);
        }
    }


}

