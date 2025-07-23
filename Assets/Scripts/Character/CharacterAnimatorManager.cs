using Unity.Netcode;
using UnityEngine;

namespace SoulsLike
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        private CharacterManager _characterManager;
        private float _vertical;
        private float _horizontal;

        protected virtual void Awake() {
            _characterManager = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement) {
            _characterManager.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
            _characterManager.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false) {
            _characterManager.applyRootMotion = applyRootMotion;
            _characterManager.animator.CrossFade(targetAnimation, 0.2f);
            // CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
            // FOR EXAMPLE, IF YOU GET DAMAGED, AND BEGIN PERFORMING A DAMAGE ANIMATION
            // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
            // WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
            _characterManager.isPerformingAction = isPerformingAction;
            _characterManager.canRotate = canRotate;
            _characterManager.canMove = canMove;

            // TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
            _characterManager.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation,
                applyRootMotion);
        }
    }
}