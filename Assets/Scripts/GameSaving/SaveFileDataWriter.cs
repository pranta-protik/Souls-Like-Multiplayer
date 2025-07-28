using System;
using System.IO;
using UnityEngine;

namespace SoulsLike
{
    public class SaveFileDataWriter
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        // BEFORE WE CREATE A NEW SAVE FILE, WE MUST CHECK TO SEE IF ONE OF THIS CHARACTER SLOT ALREADY EXISTS (MAX 10 CHARACTER SLOTS)
        public bool CheckToSeeIfFileExists() {
            if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName))) {
                return true;
            }

            return false;
        }

        // USED TO DELETE CHARACTER SAVE FILES
        public void DeleteSaveFile() {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        // USED TO CREATE A SAVE FILE UPON STARTING A NEW GAME
        public void CreateNewCharacterSaveFile(CharacterSaveData characterSaveData) {
            // MAKE A PATH TO SAVE THE FILE (A LOCATION ON THE MACHINE)
            var savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try {
                // CREATE THE DIRECTORY THE FILE WILL BE WRITTEN TO, IF IT DOES NOT ALREADY EXIST
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

                // SERIALIZE THE C# GAME DATA OBJECT INTO JSON
                var dataToStore = JsonUtility.ToJson(characterSaveData, true);

                using (var stream = new FileStream(savePath, FileMode.Create)) {
                    using (var fileWriter = new StreamWriter(stream)) {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch (Exception ex) {
                Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA, GAME NOT SAVED" + savePath + "\n" + ex);
            }
        }

        // USED TO LOAD A SAVE FILE UPON LOADING A PREVIOUS GAME
        public CharacterSaveData LoadSaveFile() {
            CharacterSaveData characterData = null;

            // MAKE A PATH TO LOAD THE FILE (A LOCATION ON THE MACHINE)
            var loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

            if (File.Exists(loadPath)) {
                try {
                    var dataToLoad = "";

                    using (var stream = new FileStream(loadPath, FileMode.Open)) {
                        using (var fileReader = new StreamReader(stream)) {
                            dataToLoad = fileReader.ReadToEnd();
                        }
                    }

                    // DESERIALIZE THE DATA FROM JSON BACK TO UNITY
                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex) {
                    Debug.LogError("ERROR WHILST TRYING TO LOAD CHARACTER DATA" + loadPath + "\n" + ex);
                }
            }

            return characterData;
        }
    }
}