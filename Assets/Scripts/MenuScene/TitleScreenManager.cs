using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager Instance;

        [Header("Menus")] [SerializeField] private GameObject _titleScreenMainMenu;
        [SerializeField] private GameObject _titleScreenLoadMenu;

        [Header("Buttons")] [SerializeField] private Button _loadMenuReturnButton;
        [SerializeField] private Button _mainMenuLoadGameButton;
        [SerializeField] private Button _mainMenuNewGameButton;
        [SerializeField] private Button _deleteCharacterPopUpConfirmButton;

        [Header("Pop Ups")] [SerializeField] private GameObject _noCharacterSlotsPopUp;
        [SerializeField] private Button _noCharacterSlotsOkayButton;
        [SerializeField] private GameObject _deleteCharacterSlotPopUp;

        [Header("Character Slots")] public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        public void StartNetworkAsHost() {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame() {
            WorldSaveGameManager.Instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu() {
            // CLOSE MAIN MENU
            _titleScreenMainMenu.SetActive(false);

            // OPEN LOAD MENU
            _titleScreenLoadMenu.SetActive(true);

            // FIND THE FIRST LOAD SLOT AND AUTO SELECT IT
            _loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu() {
            // CLOSE LOAD MENU
            _titleScreenLoadMenu.SetActive(false);

            // OPEN MAIN MENU
            _titleScreenMainMenu.SetActive(true);

            // SELECT THE LOAD BUTTON
            _mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp() {
            _noCharacterSlotsPopUp.SetActive(true);
            _noCharacterSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp() {
            _noCharacterSlotsPopUp.SetActive(false);
            _mainMenuNewGameButton.Select();
        }

        // CHARACTER SLOTS

        public void SelectCharacterSlot(CharacterSlot characterSlot) {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot() {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot() {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT) {
                _deleteCharacterSlotPopUp.SetActive(true);
                _deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot() {
            _deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.Instance.DeleteGame(currentSelectedSlot);

            // WE DISABLE AND THEN ENABLE THE LOAD MENU, TO REFRESH THE SLOTS (The deleted slots will now become inactive)
            _titleScreenLoadMenu.SetActive(false);
            _titleScreenLoadMenu.SetActive(true);

            _loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp() {
            _deleteCharacterSlotPopUp.SetActive(false);
            _loadMenuReturnButton.Select();
        }
    }
}