using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace SoulsLike
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

        [Header("FLAGS")] public bool isPerformingAction = false;
        public bool isJumping = false;
        public bool isGrounded = true;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake() {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Update() {
            animator.SetBool("IsGrounded", isGrounded);

            // IF THIS CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION TO THE POSITION OF OUR TRANSFORM
            if (IsOwner) {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
            else {
                // Position
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);

                // Rotation
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate() { }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {
            if (IsOwner) {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // RESET ANY FLAGS HERE THAT NEED TO BE RESET

                // IF WE ARE GROUNDED, PLAY AN AERIAL DEATH ANIMATION

                if (!manuallySelectDeathAnimation) {
                    characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
                }
            }

            // PLAY SOME DEATH SFX

            yield return new WaitForSeconds(5f);

            // AWARD PLAYERS WITH RUNES

            // DISABLE CHARACTER
        }

        public virtual void ReviveCharacter() { }
    }
}