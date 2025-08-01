using UnityEngine;

namespace SoulsLike
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        private CharacterManager _characterManager;

        [Header("Ground Check & Jumping")] [SerializeField]
        protected float _gravityForce = -5.55f;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckSphereRadius = 1f;
        [SerializeField] protected Vector3 _yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (Jumping or Falling)
        [SerializeField] protected float _groundedYVelocity = -20f; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GROUND WHILST THEY ARE GROUNDED

        [SerializeField]
        protected float _fallStartYVelocity = -5f; // THE FORCE AT WHICH OUR CHARACTER BEGINS TO FALL WHEN THEY BECOME UNGROUNDED (RISES AS THEY FALL LONGER)

        protected bool _fallingVelocityHasBeenSet = false;
        protected float _inAirTime = 0f;

        protected virtual void Awake() {
            _characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Update() {
            HandleGroundCheck();

            if (_characterManager.isGrounded) {
                // IF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD
                if (_yVelocity.y < 0f) {
                    _inAirTime = 0f;
                    _fallingVelocityHasBeenSet = false;
                    _yVelocity.y = _groundedYVelocity;
                }
            }
            else {
                // IF WE ARE NOT JUMPING AND OUR FALLING VELOCITY HAS NOT BEEN SET
                if (!_characterManager.isJumping && !_fallingVelocityHasBeenSet) {
                    _fallingVelocityHasBeenSet = true;
                    _yVelocity.y = _fallStartYVelocity;
                }

                _inAirTime += Time.deltaTime;
                _characterManager.animator.SetFloat("InAirTimer", _inAirTime);
                _yVelocity.y += _gravityForce * Time.deltaTime;
            }

            // THERE SHOULD ALWAYS BE SOME FORCE APPLIED TO THE Y VELOCITY
            _characterManager.characterController.Move(_yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck() {
            _characterManager.isGrounded = Physics.CheckSphere(_characterManager.transform.position, _groundCheckSphereRadius, _groundLayer);
        }

        // DRAWS OUR GROUND CHECK SPHERE IN SCENE VIEW
        protected void OnDrawGizmosSelected() {
            Gizmos.DrawSphere(_characterManager.transform.position, _groundCheckSphereRadius);
        }
    }
}