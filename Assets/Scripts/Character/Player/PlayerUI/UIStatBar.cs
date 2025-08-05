using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike
{
    public class UIStatBar : MonoBehaviour
    {
        private Slider _slider;
        private RectTransform _rectTransform;

        [Header("Bar Options")] [SerializeField]
        private bool _scaleBarLengthWithStats = true;

        [SerializeField] private float _widthScaleMultiplier = 1f;

        protected virtual void Awake() {
            _slider = GetComponent<Slider>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public virtual void SetStat(int newValue) {
            _slider.value = newValue;
        }

        public virtual void SetMaxStat(int maxValue) {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;

            if (_scaleBarLengthWithStats) {
                _rectTransform.sizeDelta = new Vector2(maxValue * _widthScaleMultiplier, _rectTransform.sizeDelta.y);

                // RESETS THE POSITION OF THE BARS BASED ON THEIR LAYOUT GROUP'S SETTINGS
                PlayerUIManager.Instance.playerUIHUDManager.RefreshHUD();
            }
        }
    }
}