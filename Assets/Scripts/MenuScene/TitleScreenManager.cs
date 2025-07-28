using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Menus")] [SerializeField] private GameObject _titleScreenMainMenu;
        [SerializeField] private GameObject _titleScreenLoadMenu;

        [Header("Buttons")] [SerializeField] private Button _loadMenuReturnButton;
        [SerializeField] private Button _mainMenuLoadGameButton;

        public void StartNetworkAsHost() {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame() {
            WorldSaveGameManager.Instance.CreateNewGame();
            StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
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
    }
}