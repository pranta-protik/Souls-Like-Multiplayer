using UnityEngine;

namespace SoulsLike
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private float _vertical;
        private float _horizontal;

        protected virtual void Awake() {
            _characterManager = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement) {
            _characterManager.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
            _characterManager.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
        }
    }
}