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
    }
}