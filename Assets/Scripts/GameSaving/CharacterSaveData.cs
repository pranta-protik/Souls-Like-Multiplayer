using System;
using UnityEngine;

namespace SoulsLike
{
    // SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONO BEHAVIOUR AND IS INSTEAD SERIALIZABLE 
    [Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")] public int sceneIndex = 1;
        [Header("Character Name")] public string characterName = "Character";
        [Header("Time Played")] public float secondsPlayed;

        // WE CAN ONLY SAVE DATA FROM "BASIC" VARIABLE TYPES (Int, Float, String, Bool, etc.)
        [Header("World Coordinates")] public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")] public float currentHealth;
        public float currentStamina;

        [Header("Stats")] public int vitality;
        public int endurance;
    }
}