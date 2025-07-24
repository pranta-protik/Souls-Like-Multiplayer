using UnityEngine;

namespace SoulsLike
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;

        protected override void Awake() {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
        }

        protected override void Update() {
            base.Update();

            // IF WE DO NOT OWN THIS GAME OBJECT, WE DO NOT CONTROL OR EDIT IT
            if (!IsOwner) {
                return;
            }

            // HANDLE MOVEMENT
            playerLocomotionManager.HandleAllMovement();
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
                PlayerInputManager.Instance.playerManager = this;
            }
        }
    }
}