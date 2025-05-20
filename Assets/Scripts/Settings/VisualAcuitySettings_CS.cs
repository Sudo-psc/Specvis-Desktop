using System;
using System.Collections.Generic;
using UnityEngine; // For List

// Assuming LandoltCLevelInfo is defined elsewhere and the test manager loads
// a default sequence of these levels.

[Serializable]
public class VisualAcuitySettings_CS
{
    public string settingsName = "Default Landolt C Acuity Settings";
    public float virtualTestDistanceMeters = 6.0f; // Default test distance
    public int presentationsPerLevel = 3;
    public int correctNeededToPassLevel = 2;
    public int incorrectStrikesToStop = 2; // Stop if this many wrong at one level
    public bool useStandardOrientationsOnly = true; // True for Up, Down, Left, Right; False for 8 orientations

    // Optional: If specific acuity levels were to be selected or ordered via settings UI
    // public List<string> customLevelIDs; // List of LogMAR values or Snellen strings to look up LandoltCLevelInfo SOs

    public VisualAcuitySettings_CS()
    {
        // customLevelIDs = new List<string>();
    }
}
