using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoulsLike
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        [SerializeField] private int _worldSceneIndex = 1;

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

        public IEnumerator LoadNewGame() {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_worldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex() => _worldSceneIndex;
    }
}