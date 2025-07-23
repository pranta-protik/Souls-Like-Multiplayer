using UnityEngine;

namespace SoulsLike
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager _playerManager;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("MOVEMENT SETTINGS")] [SerializeField]
        private float _walkingSpeed = 2f;

        [SerializeField] private float _runningSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 15f;
        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;

        [Header("DODGE")] private Vector3 _rollDirection;

        protected override void Awake() {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();
        }

        protected override void Update() {
            base.Update();

            if (_playerManager.IsOwner) {
                _playerManager.characterNetworkManager.verticalMovement.Value = verticalMovement;
                _playerManager.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                _playerManager.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else {
                verticalMovement = _playerManager.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = _playerManager.characterNetworkManager.horizontalMovement.Value;
                moveAmount = _playerManager.characterNetworkManager.moveAmount.Value;

                // IF NOT LOCKED ON, PASS MOVE AMOUNT
                _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0f, moveAmount);

                // IF LOCKED ON, PASS HORIZONTAL AND VERTICAL
            }
        }

        public void HandleAllMovement() {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetMovementValues() {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        private void HandleGroundedMovement() {
            if (!_playerManager.canMove) {
                return;
            }

            GetMovementValues();

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
            if (!_playerManager.canRotate) {
                return;
            }

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

        public void AttemptToPerformDodge() {
            if (_playerManager.isPerformingAction) {
                return;
            }

            // IF WE ARE MOVING WHEN WE ATTEMPT TO DODGE, WE PERFORM A ROLL
            if (moveAmount > 0) {
                _rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
                _rollDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
                _rollDirection.Normalize();
                _rollDirection.y = 0f;

                var playerRotation = Quaternion.LookRotation(_rollDirection);
                _playerManager.transform.rotation = playerRotation;

                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true, false, false);
            }
            // IF WE ARE STATIONARY, WE PERFORM A BACKSTEP
            else {
                _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true, false, false);
            }
        }
    }
}