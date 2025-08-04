using UnityEngine;

namespace SoulsLike
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)
        // PROCESS TIMED EFFECTS (POISON, BUILD UPS)
        // PROCESS STATIC EFFECTS (ADDING/REMOVING BUFFS FROM TALISMANS ETC)

        private CharacterManager _characterManager;

        protected virtual void Awake() {
            _characterManager = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect effect) {
            effect.ProcessEffect(_characterManager);
        }
    }
}