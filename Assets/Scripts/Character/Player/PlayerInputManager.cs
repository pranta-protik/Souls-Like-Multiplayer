using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoulsLike
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;

        [SerializeField] private Vector2 _movementInput;
        [SerializeField] private float _leftShiftPressed;

        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        private PlayerControls _playerControls;

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
                _playerControls.PlayerMovement.Walk.performed += i => _leftShiftPressed = i.ReadValue<float>();
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
            HandleMovementInput();
        }

        private void HandleMovementInput() {
            verticalInput = _movementInput.y;
            horizontalInput = _movementInput.x;

            // RETURNS THE ABSOLUTE NUMBER, (Meaning number without the negative sign, so it's always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // WE CLAMP THE VALUES, SO THEY ARE 0, 0.5 OR 1 (OPTIONAL)

            if (moveAmount > 0 && _leftShiftPressed > 0f) {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0 && _leftShiftPressed <= 0f) {
                moveAmount = 1f;
            }

            // if (_moveAmount <= 0.5f && _moveAmount > 0) {
            //     _moveAmount = 0.5f;
            // }
            // else if (_moveAmount > 0.5f && _moveAmount <= 1f) {
            //     _moveAmount = 1f;
            // }
        }
    }
}