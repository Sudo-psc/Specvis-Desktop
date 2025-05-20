using System;
using UnityEngine; // For Color, Vector2 etc.

[Serializable]
public class PerimetrySettings_CS
{
    // Mirroring UISettingsScreenAndLuminanceScale.java
    public string screenSettingsName = "DefaultScreenSettings";
    public int screenWidthInPixels = 1920;
    public int screenHeightInPixels = 1080;
    public double screenWidthInMillimeters = 500;
    public double screenHeightInMillimeters = 300;
    public double viewingDistanceInMillimeters = 570;
    public string screenBackgroundColorHex = "#808080"; // Default gray
    public double screenDistortionCorrectionQuadraticCoefficient = 0;
    public double screenDistortionCorrectionLinearCoefficient = 0;
    public double screenDistortionCorrectionConstantCoefficient = 0;
    public string luminanceScaleId = "DefaultLuminanceScale";
    public string luminanceOrBrightnessScaleIsUsed = "Luminance"; // "Luminance" or "Brightness"
    public double maximumLuminanceOrBrightnessOfTheScreen = 100; // cd/m2 or abstract unit
    public double minimumLuminanceOrBrightnessOfTheScreen = 0.1; // cd/m2 or abstract unit

    // Mirroring UISettingsStimulusAndBackground.java (simplified)
    // Assuming these are part of the Perimetry Procedure Settings
    public string stimulusShape = "Circle"; // "Circle", "Square", etc.
    public double stimulusSizeInVisualDegrees = 1.0;
    public string stimulusColorHex = "#FFFFFF"; // White
    public int stimulusDisplayTime = 200; // ms
    public string procedureBackgroundColorHex = "#000000"; // Black for perimetry

    // Mirroring UISettingsFixationAndOther.java (simplified)
    public string fixationPointType = "Cross"; // "Cross", "Circle", "Central Dot"
    public string fixationPointColorHex = "#FF0000"; // Red
    public double fixationPointSizeInVisualDegrees = 0.5;
    public bool fixationMonitoringIsOn = false;
    public string fixationMonitoringTechnique = "Blind Spot Monitoring"; // e.g., Heijl-Krakau

    // Mirroring ProcedureBasicSettingsGeneral.java (relevant parts)
    public string procedureType = "BASIC_FIX_MONITOR_NONE";
    public double brightnessLevelBackground = 0.1; // Abstract or cd/m2
    public double brightnessLevelFixPoint = 0.7;
    public double brightnessLevelStimulus = 0.5; // Initial stimulus brightness
    public double minimumBrightnessLevelStimulus = 0.01;
    public double maximumBrightnessLevelStimulus = 1.0; // Or max cd/m2
    public string stimuliPositions = "0,0;5,5;-5,-5;5,-5;-5,5"; // Example string

    // Decibel range (from original session_info.txt structure)
    public double decibelRangeMinimum = 0; // dB
    public double decibelRangeMaximum = 40; // dB
    public double decibelStepBetweenValues = 1.0; // dB

    // Other specific fields that might be needed for session_info.txt
    public string testStrategy = "Full Threshold"; // e.g., ZEST, SITA Fast
    public int maximumNumberOfPresentationsPerStimulus = 10;
    public int numberOfReversalsToDeactivateStimulus = 2;
}
