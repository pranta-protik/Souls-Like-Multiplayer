using UnityEngine;

namespace SoulsLike
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager _playerManager;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")] [SerializeField]
        private float _walkingSpeed = 1.5f;

        [SerializeField] private float _runningSpeed = 4.5f;
        [SerializeField] private float _sprintingSpeed = 7f;
        [SerializeField] private float _rotationSpeed = 15f;
        [SerializeField] private int _sprintingStaminaCost = 2;
        private Vector3 _moveDirection;
        private Vector3 _targetRotationDirection;

        [Header("Jump")] [SerializeField] private float _jumpStaminaCost = 25f;
        [SerializeField] private float _jumpHeight = 4f;
        [SerializeField] private float _jumpForwardSpeed = 5f;
        [SerializeField] private float _freeFallSpeed = 2f;
        private Vector3 _jumpDirection;

        [Header("Dodge")] private Vector3 _rollDirection;
        [SerializeField] private float _dodgeStaminaCost = 25f;

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
                _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0f, moveAmount, _playerManager.playerNetworkManager.isSprinting.Value);

                // IF LOCKED ON, PASS HORIZONTAL AND VERTICAL
            }
        }

        public void HandleAllMovement() {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
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
            _moveDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            _moveDirection = _moveDirection + PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            _moveDirection.Normalize();
            _moveDirection.y = 0f;

            if (_playerManager.playerNetworkManager.isSprinting.Value) {
                _playerManager.characterController.Move(_moveDirection * (_sprintingSpeed * Time.deltaTime));
            }
            else {
                if (PlayerInputManager.Instance.moveAmount > 0.5f) {
                    _playerManager.characterController.Move(_moveDirection * (_runningSpeed * Time.deltaTime));
                }
                else if (PlayerInputManager.Instance.moveAmount <= 0.5f) {
                    _playerManager.characterController.Move(_moveDirection * (_walkingSpeed * Time.deltaTime));
                }
            }
        }

        private void HandleJumpingMovement() {
            if (_playerManager.isJumping) {
                _playerManager.characterController.Move(_jumpDirection * (_jumpForwardSpeed * Time.deltaTime));
            }
        }

        private void HandleFreeFallMovement() {
            if (!_playerManager.isGrounded) {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
                freeFallDirection.Normalize();
                freeFallDirection.y = 0f;

                _playerManager.characterController.Move(freeFallDirection * (_freeFallSpeed * Time.deltaTime));
            }
        }

        private void HandleRotation() {
            if (!_playerManager.canRotate) {
                return;
            }

            _targetRotationDirection = Vector3.zero;
            _targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            _targetRotationDirection = _targetRotationDirection + PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
            _targetRotationDirection.Normalize();
            _targetRotationDirection.y = 0f;

            if (_targetRotationDirection == Vector3.zero) {
                _targetRotationDirection = transform.forward;
            }

            var newRotation = Quaternion.LookRotation(_targetRotationDirection);
            var targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void HandleSprinting() {
            if (_playerManager.isPerformingAction) {
                _playerManager.playerNetworkManager.isSprinting.Value = false;
            }

            if (_playerManager.playerNetworkManager.currentStamina.Value <= 0) {
                _playerManager.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            // IF WE ARE MOVING, SPRINTING IS TRUE
            if (moveAmount >= 0.5f) {
                _playerManager.playerNetworkManager.isSprinting.Value = true;
            }
            // IF WE ARE STATIONARY/MOVING SLOWLY SPRINTING IS FALSE
            else {
                _playerManager.playerNetworkManager.isSprinting.Value = false;
            }

            _playerManager.playerNetworkManager.currentStamina.Value -= _sprintingStaminaCost * Time.deltaTime;
        }

        public void AttemptToPerformDodge() {
            if (_playerManager.isPerformingAction) {
                return;
            }

            if (_playerManager.playerNetworkManager.currentStamina.Value <= 0) {
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

            _playerManager.playerNetworkManager.currentStamina.Value -= _dodgeStaminaCost;
        }

        public void AttemptToPerformJump() {
            // IF WE ARE PERFORMING A GENERAL ACTION, WE DO NOT WANT TO ALLOW A JUMP (WILL CHANGE WHEN COMBAT IS ADDED)
            if (_playerManager.isPerformingAction) {
                return;
            }

            // IF WE ARE OUT OF STAMINA, WE DO NOT WISH TO ALLOW A JUMP
            if (_playerManager.playerNetworkManager.currentStamina.Value <= 0) {
                return;
            }

            // IF WE ARE ALREADY IN A JUMP, WE DO NOT WANT TO ALLOW A JUMP AGAIN UNTIL THE CURRENT JUMP HAS FINISHED
            if (_playerManager.isJumping) {
                return;
            }

            // IF WE ARE NOT GROUNDED, WE DO NOT WANT TO ALLOW A JUMP
            if (!_playerManager.isGrounded) {
                return;
            }

            _playerManager.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_Start_01", false, false);
            _playerManager.isJumping = true;

            _playerManager.playerNetworkManager.currentStamina.Value -= _jumpStaminaCost;

            _jumpDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
            _jumpDirection = _jumpDirection + PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
            _jumpDirection.Normalize();
            _jumpDirection.y = 0f;

            if (_jumpDirection != Vector3.zero) {
                // IF WE ARE SPRINTING, JUMP DIRECTION IS AT FULL DISTANCE
                if (_playerManager.playerNetworkManager.isSprinting.Value) {
                    _jumpDirection *= 1f;
                }
                // IF WE ARE RUNNING, JUMP DIRECTION IS AT HALF DISTANCE
                else if (PlayerInputManager.Instance.moveAmount > 0.5f) {
                    _jumpDirection *= 0.5f;
                }
                // IF WE ARE WALKING, JUMP DIRECTION IS AT QUARTER DISTANCE
                else if (PlayerInputManager.Instance.moveAmount <= 0.5f) {
                    _jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyJumpingVelocity() {
            _yVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravityForce);
        }
    }
}