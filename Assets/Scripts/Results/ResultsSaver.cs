using UnityEngine;
using System;
using System.IO;
using System.Text; // For StringBuilder
using System.Collections.Generic; // For List
using System.Globalization; // For CultureInfo.InvariantCulture

// Assuming these data structures are defined in separate files as per previous steps:
// using Assets.Scripts.Results.PatientInfo_CS;
// using Assets.Scripts.Results.PerimetrySettings_CS;
// using Assets.Scripts.CoreIntegration.PerimetryResult_CS; // Or wherever PerimetryResult_CS is defined

public static class ResultsSaver
{
    /// <summary>
    /// Saves any test result data as a JSON file.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    /// <param name="resultData">The actual result data object.</param>
    /// <param name="patientId">The ID of the patient.</param>
    /// <param name="testType">A string identifying the test type (e.g., "Ishihara", "Perimetry_VisualField").</param>
    /// <param name="testTimestamp">The timestamp of when the test was completed.</param>
    /// <returns>The full path of the saved file, or null on error.</returns>
    public static string SaveResultAsJson<T>(T resultData, string patientId, string testType, DateTime testTimestamp)
    {
        if (resultData == null)
        {
            Debug.LogError("ResultsSaver: resultData cannot be null.");
            return null;
        }
        if (string.IsNullOrEmpty(patientId))
        {
            Debug.LogWarning("ResultsSaver: patientId is null or empty. Using 'UnknownPatient'.");
            patientId = "UnknownPatient";
        }
        if (string.IsNullOrEmpty(testType))
        {
            Debug.LogWarning("ResultsSaver: testType is null or empty. Using 'UnknownTest'.");
            testType = "UnknownTest";
        }

        try
        {
            string directoryPath = Path.Combine(Application.persistentDataPath, "TestResults", patientId, testType);
            Directory.CreateDirectory(directoryPath); // Ensures the directory exists

            string filename = $"{testType}_{testTimestamp:yyyyMMdd_HHmmss}.json";
            string filePath = Path.Combine(directoryPath, filename);

            string jsonOutput = JsonUtility.ToJson(resultData, true);
            File.WriteAllText(filePath, jsonOutput);

            Debug.Log($"Results saved to: {filePath}");
            return filePath;
        }
        catch (Exception e)
        {
            Debug.LogError($"ResultsSaver: Failed to save JSON result. Error: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Exports Perimetry test results to the original Specvis desktop format (session_info.txt and session_data.txt).
    /// </summary>
    /// <param name="perimetryResult">The C# perimetry result object.</param>
    /// <param name="patientInfo">Patient information.</param>
    /// <param name="settings">Perimetry settings used for the test.</param>
    /// <param name="patientId">Patient ID for directory structure.</param>
    /// <param name="testTimestamp">Timestamp for subdirectory naming.</param>
    /// <returns>True if export was successful, false otherwise.</returns>
    public static bool ExportPerimetryToDesktopFormat(
        PerimetryResult_CS perimetryResult,
        PatientInfo_CS patientInfo,
        PerimetrySettings_CS settings,
        string patientId,
        DateTime testTimestamp)
    {
        if (perimetryResult == null || patientInfo == null || settings == null)
        {
            Debug.LogError("ResultsSaver: Null data provided for Perimetry desktop format export.");
            return false;
        }
        if (string.IsNullOrEmpty(patientId))
        {
            Debug.LogWarning("ResultsSaver: patientId is null or empty for export. Using 'UnknownPatient'.");
            patientId = "UnknownPatient";
        }

        try
        {
            string exportSubDir = testTimestamp.ToString("yyyyMMdd_HHmmss");
            string directoryPath = Path.Combine(Application.persistentDataPath, "TestResults", patientId, "Perimetry_Exported", exportSubDir);
            Directory.CreateDirectory(directoryPath);

            // --- Create session_info.txt ---
            StringBuilder infoBuilder = new StringBuilder();
            infoBuilder.AppendLine("SESSION INFORMATION:");
            infoBuilder.AppendLine($"\tProcedure Date:\t{testTimestamp:dd/MM/yyyy}");
            infoBuilder.AppendLine($"\tProcedure Time:\t{testTimestamp:HH:mm:ss}");
            infoBuilder.AppendLine($"\tPatient ID:\t{patientInfo.patientId}");
            infoBuilder.AppendLine($"\tPatient Name:\t{patientInfo.patientName}");
            infoBuilder.AppendLine($"\tPatient Date of Birth:\t{patientInfo.dateOfBirth}");
            infoBuilder.AppendLine($"\tPatient Gender:\t{patientInfo.gender}");
            infoBuilder.AppendLine($"\tPatient Notes:\t{patientInfo.notes}");
            infoBuilder.AppendLine();

            infoBuilder.AppendLine("SCREEN SETTINGS:");
            infoBuilder.AppendLine($"\tScreen Settings Name:\t{settings.screenSettingsName}");
            infoBuilder.AppendLine($"\tScreen Width (pixels):\t{settings.screenWidthInPixels}");
            infoBuilder.AppendLine($"\tScreen Height (pixels):\t{settings.screenHeightInPixels}");
            infoBuilder.AppendLine($"\tScreen Width (mm):\t{settings.screenWidthInMillimeters.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tScreen Height (mm):\t{settings.screenHeightInMillimeters.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tViewing Distance (mm):\t{settings.viewingDistanceInMillimeters.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tScreen Background Color (Hex):\t{settings.screenBackgroundColorHex}");
            infoBuilder.AppendLine($"\tDistortion Correction - Quadratic:\t{settings.screenDistortionCorrectionQuadraticCoefficient.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tDistortion Correction - Linear:\t{settings.screenDistortionCorrectionLinearCoefficient.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tDistortion Correction - Constant:\t{settings.screenDistortionCorrectionConstantCoefficient.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine();

            infoBuilder.AppendLine("LUMINANCE SCALE SETTINGS:");
            infoBuilder.AppendLine($"\tLuminance Scale ID:\t{settings.luminanceScaleId}");
            infoBuilder.AppendLine($"\tLuminance/Brightness Scale Used:\t{settings.luminanceOrBrightnessScaleIsUsed}");
            infoBuilder.AppendLine($"\tMaximum Luminance/Brightness:\t{settings.maximumLuminanceOrBrightnessOfTheScreen.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tMinimum Luminance/Brightness:\t{settings.minimumLuminanceOrBrightnessOfTheScreen.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine();

            infoBuilder.AppendLine("STIMULUS AND BACKGROUND SETTINGS (PERIMETRY):");
            infoBuilder.AppendLine($"\tStimulus Shape:\t{settings.stimulusShape}");
            infoBuilder.AppendLine($"\tStimulus Size (degrees):\t{settings.stimulusSizeInVisualDegrees.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tStimulus Color (Hex):\t{settings.stimulusColorHex}");
            infoBuilder.AppendLine($"\tStimulus Display Time (ms):\t{settings.stimulusDisplayTime}");
            infoBuilder.AppendLine($"\tProcedure Background Color (Hex):\t{settings.procedureBackgroundColorHex}"); // Background during test
            infoBuilder.AppendLine();

            infoBuilder.AppendLine("FIXATION POINT AND OTHER SETTINGS:");
            infoBuilder.AppendLine($"\tFixation Point Type:\t{settings.fixationPointType}");
            infoBuilder.AppendLine($"\tFixation Point Color (Hex):\t{settings.fixationPointColorHex}");
            infoBuilder.AppendLine($"\tFixation Point Size (degrees):\t{settings.fixationPointSizeInVisualDegrees.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tFixation Monitoring On:\t{settings.fixationMonitoringIsOn}");
            infoBuilder.AppendLine($"\tFixation Monitoring Technique:\t{settings.fixationMonitoringTechnique}");
            infoBuilder.AppendLine();

            infoBuilder.AppendLine("PROCEDURE SETTINGS:");
            infoBuilder.AppendLine($"\tProcedure Type:\t{settings.procedureType}");
            infoBuilder.AppendLine($"\tTest Strategy:\t{settings.testStrategy}");
            infoBuilder.AppendLine($"\tBrightness Level - Background:\t{settings.brightnessLevelBackground.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tBrightness Level - Fixation Point:\t{settings.brightnessLevelFixPoint.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tBrightness Level - Stimulus (Initial):\t{settings.brightnessLevelStimulus.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tMinimum Brightness Level - Stimulus:\t{settings.minimumBrightnessLevelStimulus.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tMaximum Brightness Level - Stimulus:\t{settings.maximumBrightnessLevelStimulus.ToString(CultureInfo.InvariantCulture)}");
            infoBuilder.AppendLine($"\tDecibel Range (Min):\t{settings.decibelRangeMinimum.ToString(CultureInfo.InvariantCulture)} dB");
            infoBuilder.AppendLine($"\tDecibel Range (Max):\t{settings.decibelRangeMaximum.ToString(CultureInfo.InvariantCulture)} dB");
            infoBuilder.AppendLine($"\tDecibel Step:\t{settings.decibelStepBetweenValues.ToString(CultureInfo.InvariantCulture)} dB");
            infoBuilder.AppendLine($"\tMaximum Presentations per Stimulus:\t{settings.maximumNumberOfPresentationsPerStimulus}");
            infoBuilder.AppendLine($"\tReversals to Deactivate Stimulus:\t{settings.numberOfReversalsToDeactivateStimulus}");
            infoBuilder.AppendLine($"\tStimuli Positions (X,Y degrees):\t{settings.stimuliPositions}"); // Already a string
            infoBuilder.AppendLine();

            infoBuilder.AppendLine("RESULTS OVERVIEW:");
            infoBuilder.AppendLine($"\tEye Tested:\t{perimetryResult.eyeTested}"); // Assuming PerimetryResult_CS has eyeTested
            infoBuilder.AppendLine($"\tMean Threshold (dB or other unit):\t{perimetryResult.meanOfTheThresholds.ToString(CultureInfo.InvariantCulture)}"); // Ensure unit consistency
            infoBuilder.AppendLine($"\tTotal Procedure Duration (ms):\t{perimetryResult.totalProcedureDuration}");
            infoBuilder.AppendLine($"\tNumber of Stimuli Presented:\t{perimetryResult.listOfStimuli.Count}");
            // Add other summary results if available in PerimetryResult_CS

            File.WriteAllText(Path.Combine(directoryPath, "session_info.txt"), infoBuilder.ToString());

            // --- Create session_data.txt ---
            StringBuilder dataBuilder = new StringBuilder();
            dataBuilder.AppendLine("Index\tPosPxX\tPosPxY\tDistDegFromFixPointX\tDistDegFromFixPointY\tThresholdBrightness\tThresholdLuminance\tThresholdDecibel");

            foreach (var stimulusResult in perimetryResult.listOfStimuli)
            {
                dataBuilder.Append(stimulusResult.index.ToString(CultureInfo.InvariantCulture)).Append("\t");
                dataBuilder.Append(stimulusResult.positionOnTheScreenInPixelsX.ToString(CultureInfo.InvariantCulture)).Append("\t");
                dataBuilder.Append(stimulusResult.positionOnTheScreenInPixelsY.ToString(CultureInfo.InvariantCulture)).Append("\t");
                dataBuilder.Append(stimulusResult.distanceFromFixPointOnTheFieldOfViewInDegreesX.ToString(CultureInfo.InvariantCulture)).Append("\t");
                dataBuilder.Append(stimulusResult.distanceFromFixPointOnTheFieldOfViewInDegreesY.ToString(CultureInfo.InvariantCulture)).Append("\t");
                dataBuilder.Append(stimulusResult.thresholdBrightness.ToString(CultureInfo.InvariantCulture)).Append("\t"); // Assuming these fields exist in PerimetryStimulusResult_CS
                dataBuilder.Append(stimulusResult.thresholdLuminance.ToString(CultureInfo.InvariantCulture)).Append("\t");
                dataBuilder.Append(stimulusResult.thresholdDecibel.ToString(CultureInfo.InvariantCulture));
                dataBuilder.AppendLine();
            }

            File.WriteAllText(Path.Combine(directoryPath, "session_data.txt"), dataBuilder.ToString());

            Debug.Log($"Perimetry results exported to desktop format in: {directoryPath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"ResultsSaver: Failed to export Perimetry results to desktop format. Error: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Discusses export formats for new modules (Ishihara, D-15, Landolt C).
    /// </summary>
    public static void DiscussExportForNewModules()
    {
        Debug.Log("Export for New Modules (Ishihara, D-15, Landolt C):");
        Debug.Log("Primary save format for these modules is JSON via SaveResultAsJson<T>().");
        Debug.Log("Ishihara: Standardized text formats are less common. Results are often a list of plate responses and a score (e.g., X/Y correct). A simple CSV or text file could list PlateID, UserResponse, CorrectResponse, IsCorrect.");
        Debug.Log("Farnsworth D-15: Results are often plotted on a specific diagram. A text export could list the user's cap order (e.g., 'P-1-3-2-5-4...'), total error score, and diagnosed axis. CSV: UserOrder,CorrectOrder,CapNumber.");
        Debug.Log("Landolt C Visual Acuity: Results are typically the final acuity score (e.g., '20/20', 'logMAR 0.0'). A detailed export could list each trial: LevelPresented, OrientationPresented, OrientationChosen, IsCorrect.");
        Debug.Log("For now, any export beyond JSON for these new modules would be a new feature. The JSON format provides all necessary data for any subsequent conversion or analysis tool.");
    }
}

// Helper C# classes for Perimetry results to match session_data.txt (if not already defined elsewhere)
// Ensure these are serializable if they come from JSON, though for export they are just being read.
[Serializable]
public class PerimetryResult_CS // This should match the structure from Task 14
{
    public string eyeTested = "Unknown";
    public string procedureType = "N/A";
    public string procedureDate = "N/A";
    public string procedureTime = "N/A";
    public string patientId = "N/A";
    public List<PerimetryStimulusResult_CS> listOfStimuli = new List<PerimetryStimulusResult_CS>();
    public double meanOfTheThresholds = 0;
    public long totalProcedureDuration = 0;
    // Add other fields from Task 14's PerimetryResult_CS if needed for session_info
}

[Serializable]
public class PerimetryStimulusResult_CS // This should match the structure from Task 14
{
    public int index = 0;
    public double positionOnTheScreenInPixelsX = 0;
    public double positionOnTheScreenInPixelsY = 0;
    public double distanceFromFixPointOnTheFieldOfViewInDegreesX = 0;
    public double distanceFromFixPointOnTheFieldOfViewInDegreesY = 0;
    
    // Assuming these 'threshold' fields represent the final determined value for the stimulus
    public double thresholdBrightness = 0; // This might be the 'currentBrightnessValue' when stimulus becomes inactive
    public double thresholdLuminance = 0; // Needs conversion logic if not directly available
    public double thresholdDecibel = 0;   // Needs conversion logic if not directly available

    // Fields from original ProcedureBasicStimulus that might be useful:
    public string stimulusType = "N/A";
    public double stimulusSizeDegX = 0;
    public bool isActive = true; // True if still being tested, false if threshold found/deactivated
    public List<bool> responseHistory = new List<bool>();
    public List<double> reversalLevels = new List<double>();
}
