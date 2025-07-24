using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoulsLike
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;

        public PlayerManager playerManager;
        private PlayerControls _playerControls;

        [Header("CAMERA MOVEMENT INPUT")] [SerializeField]
        private Vector2 _cameraInput;

        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("PLAYER MOVEMENT INPUT")] [SerializeField]
        private Vector2 _movementInput;

        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("PLAYER ACTION INPUT")] [SerializeField]
        private bool _walkInput = false;

        [SerializeField] private bool _dodgeInput = false;
        [SerializeField] private bool _sprintInput = false;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);

            // WHEN SCENE CHANGES, RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChanged;

            Instance.enabled = false;
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene) {
            // IF WE ARE LOADING INTO OUR WORLD SCENE, ENABLE OUR PLAYERS CONTROLS
            if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) {
                Instance.enabled = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            // OTHERWISE WE MUST BE AT THE MAIN MENU, DISABLE OUR PLAYERS CONTROLS
            // THIS IS SO OUR PLAYER CANT MOVE AROUND IF WE ENTER THINGS LIKE A CHARACTER CREATION MENU ETC. 
            else {
                Instance.enabled = false;
            }
        }

        private void OnEnable() {
            if (_playerControls == null) {
                _playerControls = new PlayerControls();

                _playerControls.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
                _playerControls.PlayerCamera.Movement.performed += i => _cameraInput = i.ReadValue<Vector2>();

                _playerControls.PlayerActions.Walk.performed += i => _walkInput = true;
                _playerControls.PlayerActions.Walk.canceled += i => _walkInput = false;
                _playerControls.PlayerActions.Dodge.performed += i => _dodgeInput = true;

                // HOLDING THE INPUT, SETS THE BOOL TO TRUE
                _playerControls.PlayerActions.Sprint.performed += i => _sprintInput = true;
                // RELEASING THE INPUT, SETS THE BOOL TO FALSE
                _playerControls.PlayerActions.Sprint.canceled += i => _sprintInput = false;
            }

            _playerControls.Enable();
        }

        private void OnDestroy() {
            // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT (PREVENT MEMORY LEAK)
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        // IF WE MINIMIZE OR LOWER THE WINDOW, STOP ADJUSTING INPUTS
        private void OnApplicationFocus(bool hasFocus) {
            if (enabled) {
                if (hasFocus) {
                    _playerControls.Enable();
                }
                else {
                    _playerControls.Disable();
                }
            }
        }

        private void Update() {
            HandleAllInputs();
        }

        private void HandleAllInputs() {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSpringing();
        }

        // MOVEMENT
        private void HandlePlayerMovementInput() {
            verticalInput = _movementInput.y;
            horizontalInput = _movementInput.x;

            // RETURNS THE ABSOLUTE NUMBER, (Meaning number without the negative sign, so it's always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // WE CLAMP THE VALUES, SO THEY ARE 0, 0.5 OR 1 (OPTIONAL)

            if (moveAmount > 0 && _walkInput) {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0 && !_walkInput) {
                moveAmount = 1f;
            }

            // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON-STRAFING MOVEMENT
            // WE USE THE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

            if (playerManager == null) {
                return;
            }

            // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT
            playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0f, moveAmount, playerManager.playerNetworkManager.isSprinting.Value);

            // IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT AS WELL


            // if (_moveAmount <= 0.5f && _moveAmount > 0) {
            //     _moveAmount = 0.5f;
            // }
            // else if (_moveAmount > 0.5f && _moveAmount <= 1f) {
            //     _moveAmount = 1f;
            // }
        }

        private void HandleCameraMovementInput() {
            cameraVerticalInput = _cameraInput.y;
            cameraHorizontalInput = _cameraInput.x;
        }

        // ACTION
        private void HandleDodgeInput() {
            if (_dodgeInput) {
                _dodgeInput = false;

                playerManager.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSpringing() {
            if (_sprintInput) {
                playerManager.playerLocomotionManager.HandleSprinting();
            }
            else {
                playerManager.playerNetworkManager.isSprinting.Value = false;
            }
        }
    }
}