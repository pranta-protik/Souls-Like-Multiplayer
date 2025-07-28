using TMPro;
using UnityEngine;

namespace SoulsLike
{
    public class UICharacterSaveSlot : MonoBehaviour
    {
        private SaveFileDataWriter _saveFileWriter;

        [Header("Game Slot")] public CharacterSlot characterSlot;

        [Header("Character Info")] public TextMeshProUGUI characterName;

        public TextMeshProUGUI timePlayed;

        private void OnEnable() {
            LoadSaveSlot();
        }

        private void LoadSaveSlot() {
            _saveFileWriter = new SaveFileDataWriter();
            _saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // SAVE SLOT 01
            if (characterSlot == CharacterSlot.CharacterSlot_01) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat01.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 02
            else if (characterSlot == CharacterSlot.CharacterSlot_02) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat02.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 03
            else if (characterSlot == CharacterSlot.CharacterSlot_03) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat03.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 04
            else if (characterSlot == CharacterSlot.CharacterSlot_04) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat04.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 05
            else if (characterSlot == CharacterSlot.CharacterSlot_05) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat05.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 06
            else if (characterSlot == CharacterSlot.CharacterSlot_06) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat06.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 07
            else if (characterSlot == CharacterSlot.CharacterSlot_07) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat07.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 08
            else if (characterSlot == CharacterSlot.CharacterSlot_08) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat08.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 09
            else if (characterSlot == CharacterSlot.CharacterSlot_09) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat09.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 10
            else if (characterSlot == CharacterSlot.CharacterSlot_10) {
                _saveFileWriter.saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                // IF THE FILE EXISTS, GET INFORMATION FROM IT
                if (_saveFileWriter.CheckToSeeIfFileExists()) {
                    characterName.text = WorldSaveGameManager.Instance.characterSloat10.characterName;
                }
                // IF IT DOES NOT, DISABLE THIS GAME OBJECT
                else {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}