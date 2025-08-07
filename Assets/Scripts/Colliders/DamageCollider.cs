using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")] public float physicalDamage = 0f;
        public float magicDamage = 0f;
        public float fireDamage = 0f;
        public float lightningDamage = 0f;
        public float holyDamage = 0f;

        [Header("Contact Point")] protected Vector3 _contactPoint;

        [Header("Characters Damaged")] protected List<CharacterManager> _charactersDamagedList = new();

        private void OnTriggerEnter(Collider other) {
            var damageTarget = other.GetComponent<CharacterManager>();

            if (damageTarget != null) {
                _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget) {
            if (_charactersDamagedList.Contains(damageTarget)) {
                return;
            }

            _charactersDamagedList.Add(damageTarget);

            var damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = _contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}