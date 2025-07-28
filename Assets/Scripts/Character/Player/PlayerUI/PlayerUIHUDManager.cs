using UnityEngine;

namespace SoulsLike
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] private UIStatBar _staminaBar;

        public void SetNewStaminaValue(float oldValue, float newValue) {
            _staminaBar.SetStat(Mathf.RoundToInt(newValue));
        }

        public void SetMaxStaminaValue(int maxStamina) {
            _staminaBar.SetMaxStat(maxStamina);
        }
    }
}