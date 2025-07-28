using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SoulsLike
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager Instance { get; private set; }

        [SerializeField] private PlayerManager _playerManager;

        [Header("Save/Load")] [SerializeField] private bool _saveGame;
        [SerializeField] private bool _loadGame;

        [Header("World Scene Index")] [SerializeField]
        private int _worldSceneIndex = 1;

        [Header("Save Data Writer")] private SaveFileDataWriter _saveFileDataWriter;

        [Header("Current Character Data")] public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string _saveFileName;

        [Header("Character Slots")] public CharacterSaveData characterSloat01;
        public CharacterSaveData characterSloat02;
        public CharacterSaveData characterSloat03;
        public CharacterSaveData characterSloat04;
        public CharacterSaveData characterSloat05;
        public CharacterSaveData characterSloat06;
        public CharacterSaveData characterSloat07;
        public CharacterSaveData characterSloat08;
        public CharacterSaveData characterSloat09;
        public CharacterSaveData characterSloat10;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterProfiles();
        }

        private void Update() {
            if (_saveGame) {
                _saveGame = false;
                SaveGame();
            }

            if (_loadGame) {
                _loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot) {
            var fileName = "";

            switch (characterSlot) {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "CharacterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "CharacterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "CharacterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "CharacterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "CharacterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "CharacterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "CharacterSlot_10";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        public void CreateNewGame() {
            // CREATE A NEW FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame() {
            // LOAD A PREVIOUS FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            _saveFileDataWriter = new SaveFileDataWriter();

            // GENERALLY WORKS ON MULTIPLE MACHINE TYPES 
            _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter.saveFileName = _saveFileName;
            currentCharacterData = _saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame() {
            // SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            _saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            _saveFileDataWriter = new SaveFileDataWriter();

            // GENERALLY WORKS ON MULTIPLE MACHINE TYPES 
            _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            _saveFileDataWriter.saveFileName = _saveFileName;

            // PASS THE PLAYERS INFO, FROM GAME, TO THEIR SAVE FILE
            _playerManager.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // WHITE THAT INFO ONTO A JSON FILE, SAVED TO THEIS MACHINE
            _saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        // LOAD ALL CHARACTER PROFILES ON DEVICE WHEN STARTING GAME
        private void LoadAllCharacterProfiles() {
            _saveFileDataWriter = new SaveFileDataWriter();
            _saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSloat01 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSloat02 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSloat03 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSloat04 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSloat05 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSloat06 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSloat07 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSloat08 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSloat09 = _saveFileDataWriter.LoadSaveFile();

            _saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSloat10 = _saveFileDataWriter.LoadSaveFile();
        }

        public IEnumerator LoadWorldScene() {
            var loadOperation = SceneManager.LoadSceneAsync(_worldSceneIndex);
            yield return null;
        }

        public int GetWorldSceneIndex() => _worldSceneIndex;
    }
}