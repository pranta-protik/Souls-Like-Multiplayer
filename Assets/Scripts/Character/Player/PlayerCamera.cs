using UnityEngine;

namespace SoulsLike
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance;

        public GameObject cameraObject;

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
        }
    }
}