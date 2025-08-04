using System;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete Later")] [SerializeField]
        private InstantCharacterEffect _effectToTest;

        [SerializeField] private bool _processEffect = false;

        private void Update() {
            if (_processEffect) {
                _processEffect = false;

                var effect = Instantiate(_effectToTest);
                ProcessInstantEffect(effect);
            }
        }
    }
}