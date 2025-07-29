using UnityEngine;

namespace SoulsLike
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        private PlayerControls _playerControls;

        [Header("Title Screen Inputs")] [SerializeField]
        private bool _deleteCharacterSlot = false;

        private void Update() {
            if (_deleteCharacterSlot) {
                _deleteCharacterSlot = false;
                TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable() {
            if (_playerControls == null) {
                _playerControls = new PlayerControls();
                _playerControls.UI.X.performed += i => _deleteCharacterSlot = true;
            }

            _playerControls.Enable();
        }

        private void OnDisable() {
            _playerControls.Disable();
        }
    }
}