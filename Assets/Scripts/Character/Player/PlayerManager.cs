namespace SoulsLike
{
    public class PlayerManager : CharacterManager
    {
        private PlayerLocomotionManager _playerLocomotionManager;

        protected override void Awake() {
            base.Awake();

            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update() {
            base.Update();

            // IF WE DO NOT OWN THIS GAME OBJECT, WE DO NOT CONTROL OR EDIT IT
            if (!IsOwner) {
                return;
            }

            // HANDLE MOVEMENT
            _playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate() {
            if (!IsOwner) {
                return;
            }

            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();

            // IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
            if (IsOwner) {
                PlayerCamera.Instance.playerManager = this;
            }
        }
    }
}