using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager Instance;

        [Header("Damage")] public TakeDamageEffect takeDamageEffect;

        [SerializeField] private List<InstantCharacterEffect> _instantEffectsList;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }

            GenerateEffectIDs();
        }

        private void GenerateEffectIDs() {
            for (var i = 0; i < _instantEffectsList.Count; i++) {
                _instantEffectsList[i].instantEffectID = i;
            }
        }
    }
}