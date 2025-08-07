using UnityEngine;

namespace SoulsLike
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private PlayerManager _playerManager;

        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake() {
            base.Awake();

            _playerManager = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
        }

        protected override void Start() {
            base.Start();

            LoadWeaponsOnBothHand();
        }

        private void InitializeWeaponSlots() {
            var weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots) {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand) {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand) {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponsOnBothHand() {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        public void LoadRightWeapon() {
            if (_playerManager.playerInventoryManager.currentRightHandWeapon != null) {
                rightHandWeaponModel = Instantiate(_playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
            }
        }

        public void LoadLeftWeapon() {
            if (_playerManager.playerInventoryManager.currentLeftHandWeapon != null) {
                leftHandWeaponModel = Instantiate(_playerManager.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
            }
        }
    }
}