using UnityEngine;

namespace SoulsLike
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager _characterManager;

        [Header("Stamina Regeneration")] [SerializeField]
        private float _staminaRegenerationAmount = 2f;

        private float _staminaRegenerationTimer = 0f;
        private float _staminaTickTimer = 0f;
        [SerializeField] private float _staminaRegenerationDelay = 2f;

        protected virtual void Awake() {
            _characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Start() { }

        public int CalculateHealthBasedOnVitalityLevel(int vitality) {
            var health = 0f;

            // CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED
            health = vitality * 15f;

            return Mathf.RoundToInt(health);
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance) {
            var stamina = 0f;

            // CREATE AN EQUATION FOR HOW YOU WANT YOUR STAMINA TO BE CALCULATED
            stamina = endurance * 10f;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina() {
            // ONLY OWNERS CAN EDIT THEIR NETWORK VARIABLES
            if (!_characterManager.IsOwner) {
                return;
            }

            // WE DO NOT WANT TO REGENERATE STAMINA IF WE ARE USING IT
            if (_characterManager.characterNetworkManager.isSprinting.Value) {
                return;
            }

            if (_characterManager.isPerformingAction) {
                return;
            }

            _staminaRegenerationTimer += Time.deltaTime;

            if (_staminaRegenerationTimer >= _staminaRegenerationDelay) {
                if (_characterManager.characterNetworkManager.currentStamina.Value < _characterManager.characterNetworkManager.maxStamina.Value) {
                    _staminaTickTimer += Time.deltaTime;

                    if (_staminaTickTimer > 0.1f) {
                        _staminaTickTimer = 0f;
                        _characterManager.characterNetworkManager.currentStamina.Value += _staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount) {
            // WE ONLY WANT TO RESET THE REGENERATION IF THE ACTION USED STAMINA
            // WE DON'T WANT TO RESET THE REGENERATION IF WE ARE ALREADY REGENERATING STAMINA 
            if (currentStaminaAmount < previousStaminaAmount) {
                _staminaRegenerationTimer = 0f;
            }
        }
    }
}