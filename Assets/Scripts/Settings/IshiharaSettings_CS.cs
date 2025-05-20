using System;
using System.Collections.Generic;
using UnityEngine; // Required for List and ScriptableObject if referencing directly

[Serializable]
public class IshiharaSettings_CS
{
    public string settingsName = "Default Ishihara Settings";
    public int numberOfPlatesToPresent = 14; // Example: Standard set size, could be configurable
    public bool randomizePlateOrder = false;

    // Optional: If you want to allow selection of a specific sequence of plates
    // public List<IshiharaPlateInfo> customPlateSequence; // Assign ScriptableObjects here
                                                          // Note: JsonUtility doesn't directly serialize lists of ScriptableObjects well.
                                                          // Better to store a list of plateIDs (strings) and load SOs at runtime.
    public List<string> customPlateIDSequence; // Store IDs to look up ScriptableObjects

    public IshiharaSettings_CS()
    {
        customPlateIDSequence = new List<string>();
    }
}
