using UnityEngine;

namespace SoulsLike
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;
        
        public PlayerManager playerManager;
        public GameObject cameraObject;
        [SerializeField] private Transform _cameraPivotTransform;

        // CHANGE THESE TO TWEAK CAMERA PERFORMANCE
        [Header("Camera Settings")] [SerializeField]
        private float _cameraSmoothSpeed = 1f; // THE BIGGER THIS NUMBER, THE LONGER FOR THE CAMERA TO REACH ITS POSITION DURING MOVEMENT

        [SerializeField] private float _leftAndRightRotationSpeed = 25f;
        [SerializeField] private float _upAndDownRotationSpeed = 25f;
        [SerializeField] private float _minimumPivot = -30f; // THE LOWEST POINT YOU ARE ABLE TO LOOK DOWN
        [SerializeField] private float _maximumPivot = 60f; // THE HIGHEST POINT YOU ARE ABLE TO LOOK UP
        [SerializeField] private float _cameraCollisionRadius = 0.2f;
        [SerializeField] private LayerMask _collideWithLayers;

        [Header("Camera Values")] private Vector3 _cameraVelocity;
        private Vector3 _cameraObjectPosition; // USED FOR CAMERA COLLISIONS (MOVES THE CAMERA OBJECT TO THIS POSITION UPON COLLIDING)
        [SerializeField] private float _leftAndRightLookAngle;
        [SerializeField] private float _upAndDownLookAngle;
        private float _cameraZPosition; // VALUES USED FOR CAMERA COLLISIONS
        private float _targetCameraZPosition; // VALUES USED FOR CAMERA COLLISIONS

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
            _cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions() {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }

        private void HandleFollowTarget() {
            var targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref _cameraVelocity,
                _cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations() {
            // ROTATE LEFT AND RIGHT BASED ON HORIZONTAL MOVEMENT OF THE MOUSE
            _leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * _leftAndRightRotationSpeed * Time.deltaTime;
            // ROTATE UP AND DOWN BASED ON VERTICAL MOVEMENT OF THE MOUSE
            _upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * _upAndDownRotationSpeed * Time.deltaTime;
            // CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN A MIN AND MAX VALUE
            _upAndDownLookAngle = Mathf.Clamp(_upAndDownLookAngle, _minimumPivot, _maximumPivot);

            var cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // ROTATE THIS GAME OBJECT LEFT AND RIGHT 
            cameraRotation.y = _leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // ROTATE THIS GAME OBJECT UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = _upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            _cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions() {
            _targetCameraZPosition = _cameraZPosition;
            RaycastHit hit;
            // DIRECTION FOR COLLISION CHECK
            var direction = cameraObject.transform.position - _cameraPivotTransform.position;
            direction.Normalize();

            // WE CHECK IF THERE IS AN OBJECT IN FRONT OF OUR DESIRED DIRECTION
            if (Physics.SphereCast(_cameraPivotTransform.position, _cameraCollisionRadius, direction, out hit, Mathf.Abs(_targetCameraZPosition),
                    _collideWithLayers)) {
                // IF THERE IS, WE GET OUR DISTANCE FROM IT
                var distanceFromHitObject = Vector3.Distance(_cameraPivotTransform.position, hit.point);
                // WE THEN EQUATE OUR TARGET Z POSITION TO THE FOLLOWING
                _targetCameraZPosition = -(distanceFromHitObject - _cameraCollisionRadius);
            }

            // IF OUR TARGET POSITION IS LESS THAN OUR COLLISION RADIUS, WE SUBTRACT OUR COLLISION RADIUS (MAKING IT SNAP BACK)
            if (Mathf.Abs(_targetCameraZPosition) < _cameraCollisionRadius) {
                _targetCameraZPosition = -_cameraCollisionRadius;
            }

            // WE THEN APPLY OUR FINAL POSITION USING A LERP OVER A TIME OF 0.2F
            _cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = _cameraObjectPosition;
        }
    }
}