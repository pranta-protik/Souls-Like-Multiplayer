using UnityEngine;

namespace SoulsLike
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] private UIStatBar _healthBar;
        [SerializeField] private UIStatBar _staminaBar;

        public void RefreshHUD() {
            _healthBar.gameObject.SetActive(false);
            _healthBar.gameObject.SetActive(true);

            _staminaBar.gameObject.SetActive(false);
            _staminaBar.gameObject.SetActive(true);
        }

        public void SetNewHealthValue(int oldValue, int newValue) {
            _healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(int maxHealth) {
            _healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue) {
            _staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina) {
            _staminaBar.SetMaxStat(maxStamina);
        }
    }
}