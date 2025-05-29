using UnityEngine;

namespace SoulsLike
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private Vector2 _movementInput;
        private PlayerControls _playerControls;

        private void OnEnable()
        {
            if (_playerControls == null)
            {
                _playerControls = new PlayerControls();
                _playerControls.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
            }
            _playerControls.Enable();
        }
    }
}