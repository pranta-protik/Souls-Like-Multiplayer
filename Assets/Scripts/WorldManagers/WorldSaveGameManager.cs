using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoulsLike
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        [SerializeField] private int _worldSceneIndex = 1;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_worldSceneIndex);
            yield return null;
        }
    }
}