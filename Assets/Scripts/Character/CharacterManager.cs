using UnityEngine;

namespace SoulsLike
{
    public class CharacterManager : MonoBehaviour
    {
        protected virtual void Awake() {
            DontDestroyOnLoad(this);
        }

        protected virtual void Update() { }
    }
}