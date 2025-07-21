using UnityEngine;

namespace SoulsLike
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager _playerManager;

        public float verticalMovement;
        public float horizontalMovement;
        public float moveAmount;
        [SerializeField] private float _walkingSpeed = 2f;
        [SerializeField] private float _runningSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 15f;

        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;

        protected override void Awake() {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement() {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetVerticalAndHorizontalInputs() {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        }

        private void HandleGroundedMovement() {
            GetVerticalAndHorizontalInputs();

            // OUR MOVE DIRECTION IS BASED ON OUR CAMERAS FACING PERSPECTIVE & OUR MOVEMENT INPUTS
            _moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            _moveDirection = _moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0f;

            if (PlayerInputManager.Instance.moveAmount > 0.5f) {
                _playerManager.characterController.Move(_moveDirection * (_runningSpeed * Time.deltaTime));
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f) {
                _playerManager.characterController.Move(_moveDirection * (_walkingSpeed * Time.deltaTime));
            }
        }

        private void HandleRotation() {
            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            _targetRotationDirection = _targetRotationDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
            _targetRotationDirection.Normalize();
            _targetRotationDirection.y = 0f;

            if (_targetRotationDirection == Vector3.zero) {
                _targetRotationDirection = transform.forward;
            }

            var newRotation = Quaternion.LookRotation(_targetRotationDirection);
            var targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}