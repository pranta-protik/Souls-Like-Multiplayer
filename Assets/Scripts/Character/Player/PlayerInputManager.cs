using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoulsLike
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;

        [SerializeField] private Vector2 _movementInput;

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
            }

            _playerControls.Enable();
        }

        private void OnDestroy() {
            // IF WE DESTROY THIS OBJECT, UNSUBSCRIBE FROM THIS EVENT (PREVENT MEMORY LEAK)
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }
}