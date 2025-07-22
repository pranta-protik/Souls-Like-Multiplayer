using Unity.Netcode;
using UnityEngine;

namespace SoulsLike
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController characterController;
        private CharacterNetworkManager _characterNetworkManager;

        protected virtual void Awake() {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            _characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update() {
            // IF THIS CHARACTER IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION TO THE POSITION OF OUR TRANSFORM
            if (IsOwner) {
                _characterNetworkManager.networkPosition.Value = transform.position;
                _characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSITION OF ITS NETWORK TRANSFORM
            else {
                // Position
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    _characterNetworkManager.networkPosition.Value,
                    ref _characterNetworkManager.networkPositionVelocity,
                    _characterNetworkManager.networkPositionSmoothTime);

                // Rotation
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    _characterNetworkManager.networkRotation.Value,
                    _characterNetworkManager.networkRotationSmoothTime);
            }
        }

        protected virtual void LateUpdate() { }
    }
}