using UnityEngine;

namespace SoulsLike
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        protected virtual void Awake() {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayRollSoundFX() {
            _audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
        }
    }
}