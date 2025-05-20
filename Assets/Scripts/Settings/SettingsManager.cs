using UnityEngine;
using System;
using System.IO;

public static class SettingsManager
{
    private static string GetBasePath(string profileId = "default")
    {
        if (string.IsNullOrEmpty(profileId))
        {
            profileId = "default";
        }
        return Path.Combine(Application.persistentDataPath, "Settings", profileId);
    }

    public static void SaveSettings<T>(T settingsData, string settingsCategory, string profileId = "default")
    {
        if (settingsData == null)
        {
            Debug.LogError($"SettingsManager: settingsData for '{settingsCategory}' is null. Cannot save.");
            return;
        }
        if (string.IsNullOrEmpty(settingsCategory))
        {
            Debug.LogError("SettingsManager: settingsCategory cannot be null or empty.");
            return;
        }

        try
        {
            string directoryPath = GetBasePath(profileId);
            Directory.CreateDirectory(directoryPath); // Ensure directory exists

            string filePath = Path.Combine(directoryPath, $"{settingsCategory}.json");
            string jsonOutput = JsonUtility.ToJson(settingsData, true);
            File.WriteAllText(filePath, jsonOutput);

            Debug.Log($"Settings saved to: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"SettingsManager: Failed to save settings for '{settingsCategory}' in profile '{profileId}'. Error: {e.Message}");
        }
    }

    public static T LoadSettings<T>(string settingsCategory, string profileId = "default") where T : new()
    {
        if (string.IsNullOrEmpty(settingsCategory))
        {
            Debug.LogError("SettingsManager: settingsCategory cannot be null or empty for loading.");
            return new T();
        }

        try
        {
            string directoryPath = GetBasePath(profileId);
            string filePath = Path.Combine(directoryPath, $"{settingsCategory}.json");

            if (File.Exists(filePath))
            {
                string jsonInput = File.ReadAllText(filePath);
                T settingsData = JsonUtility.FromJson<T>(jsonInput);
                Debug.Log($"Settings loaded from: {filePath}");
                return settingsData ?? new T(); // Return new if FromJson returns null (e.g. empty file)
            }
            else
            {
                Debug.Log($"SettingsManager: No settings file found for '{settingsCategory}' in profile '{profileId}' at '{filePath}'. Returning default.");
                return new T(); // Return default/empty settings
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"SettingsManager: Failed to load settings for '{settingsCategory}' in profile '{profileId}'. Error: {e.Message}. Returning default.");
            return new T();
        }
    }

    // Regarding .sset files:
    // For initial setup, existing perimetry .sset files would need to be manually converted
    // or a separate one-time utility script (outside SettingsManager) could be written
    // to parse them and save them into the new JSON format using SettingsManager.SaveSettings().
    // SettingsManager itself will only deal with the JSON format for loading and saving.
    // Such a utility would read the .sset file, populate a PerimetrySettings_CS object,
    // and then call SaveSettings<PerimetrySettings_CS>(parsedSettings, "PerimetrySettings", patientIdFromSset);
}
