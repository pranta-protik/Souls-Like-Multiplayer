using UnityEngine;

namespace SoulsLike
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // IF THE DAMAGE IS CAUSED BY ANOTHER CHARACTERS ATTACK IT WILL BE STORED HERE

        [Header("Damage")] public float physicalDamage = 0f; // (IN THE FUTURE WILL BE SPLIT INTO "STANDARD", "STRIKE", "SLASH" AND "PIERCE")
        public float magicDamage = 0f;
        public float fireDamage = 0f;
        public float lightningDamage = 0f;
        public float holyDamage = 0f;

        [Header("Final Damage")] public int finalDamageDealt = 0; // THE DAMAGE THE CHARACTER TAKES AFTER ALL CALCULATIONS HAVE BEEN MADE

        [Header("Poise")] public float poiseDamage = 0f;
        public bool poiseIsBroken = false; // IF A CHARACTER'S POISE IS BROKEN, THEY WILL BE "STUNNED" AND PAY A DAMAGE ANIMATION

        [Header("Animation")] public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")] public bool willPlayDamageFX = true;
        public AudioClip elementalDamageSoundFX; // USED ON TOP OF REGULAR SFX IF THERE IS ELEMENTAL DAMAGE PRESENT (MAGIC/FIRE/LIGHTNING/HOLY)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom; // USED TO DETERMINE WHAT DAMAGE ANIMATION TO PLAY (MOVE BACKWARDS, TO THE LEFT, TO THE RIGHT ETC)

        public Vector3 contactPoint; // USED TO DETERMINE WHERE THE BLOOD FX INSTANTIATE

        public override void ProcessEffect(CharacterManager characterManager) {
            base.ProcessEffect(characterManager);

            // IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
            if (characterManager.isDead.Value) {
                return;
            }

            CalculateDamage(characterManager);
        }

        private void CalculateDamage(CharacterManager characterManager) {
            if (!characterManager.IsOwner) {
                return;
            }

            if (characterCausingDamage != null) { }

            // ADD ALL DAMAGE TYPES TOGETHER, AND APPLY FINAL DAMAGE
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0) {
                finalDamageDealt = 1;
            }

            Debug.Log("Final Damage Given: " + finalDamageDealt);

            characterManager.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        }
    }
}