using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MarbleBash.UI
{

    public class LevelView : MonoBehaviour
    {
        private bool _initialised;

        [Header("References:")]
        [SerializeField] private Image _xpFillRing;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("State:")]
        [SerializeField] private float _ringFillTarget;

        #region Initialisation & Destruction
        void Awake()
        {
            GameController.OnInitialiseUI += Setup;
        }

        private void OnDestroy()
        {
            GameController.OnInitialiseUI -= Setup;

            if (_initialised)
            {
                Player.instance.level.OnXpChanged -= PlayerXpChanged;
                Player.instance.level.OnLevelUp -= PlayerLevelUp;
            }
        }
        #endregion

        private void Setup()
        {
            _initialised = true;

            Player.instance.level.OnXpChanged += PlayerXpChanged;
            Player.instance.level.OnLevelUp += PlayerLevelUp;

            _ringFillTarget = 0f;
            _xpFillRing.fillAmount = 0;
            RedrawLevelText();
        }

        private void PlayerXpChanged(int amount)
        {
            RecalculateRingFillTarget();
        }

        private void PlayerLevelUp(int level)
        {
            RecalculateRingFillTarget();
            RedrawLevelText();            
        }

        private void RecalculateRingFillTarget()
        {
            float xpPercentage = (float)Player.instance.level.xp / (float)Player.instance.level.xpNeededForLevelUp;
            
            _ringFillTarget = xpPercentage;
        }

        private void RedrawLevelText()
        {
            _levelText.text = Player.instance.level.level.ToString();
        }
    
        private void Update()
        {
            _xpFillRing.fillAmount = Mathf.Lerp(_xpFillRing.fillAmount, _ringFillTarget, Time.deltaTime * 2f);
        }
    }


}

