using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For OrderBy
using TMPro; // For TextMeshPro elements
using UnityEngine.UI; // For UI Buttons

public class VisualAcuityTestManager_LandoltC : MonoBehaviour
{
    [Header("Test Configuration")]
    public List<LandoltCLevelInfo> acuityLevels; // Assign ScriptableObjects in Inspector, ordered largest to smallest angularSize
    public GameObject landoltCPrefab; // Assign the Landolt C 3D model/quad prefab
    public float virtualTestDistanceMeters = 6.0f; // Standard virtual distance for acuity tests

    [Header("VR UI References")]
    public GameObject landoltCDisplayObject; // The instantiated Landolt C
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI statusText; // For "Correct", "Incorrect", final score
    public GameObject orientationInputPanel; // Parent GameObject for orientation buttons
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;
    // Optional: Add buttons for oblique orientations if using 8 directions
    public Button upLeftButton;
    public Button upRightButton;
    public Button downLeftButton;
    public Button downRightButton;


    [Header("Test Parameters")]
    public int presentationsPerLevel = 3; // Number of times to show C at each level
    public int correctNeededToPassLevel = 2; // Min correct to pass level and move to smaller C
    [Tooltip("Number of incorrect responses at a single level to stop the test early.")]
    public int incorrectStrikesToStop = 2; // e.g., if user gets 2 wrong at one level, stop.

    private int currentLevelIndex;
    private int presentationsAtCurrentLevel;
    private int correctAtCurrentLevel;
    private int incorrectAtCurrentLevelStrikes; // For early stopping if many errors at one level
    private LandoltCOrientation currentOrientation;
    private VisualAcuityTestResult_LandoltC currentTestResult;
    private bool testIsRunning = false;
    private string eyeBeingTested;

    // Standard orientations
    private readonly LandoltCOrientation[] standardOrientations = {
        LandoltCOrientation.Up, LandoltCOrientation.Down, LandoltCOrientation.Left, LandoltCOrientation.Right
    };
    // Optional 8 orientations
    private readonly LandoltCOrientation[] eightOrientations = {
        LandoltCOrientation.Up, LandoltCOrientation.Down, LandoltCOrientation.Left, LandoltCOrientation.Right,
        LandoltCOrientation.UpLeft, LandoltCOrientation.UpRight, LandoltCOrientation.DownLeft, LandoltCOrientation.DownRight
    };
    private LandoltCOrientation[] activeOrientations;


    void Start()
    {
        // Sort acuity levels from largest angular size (easiest) to smallest (hardest)
        if (acuityLevels != null)
        {
            acuityLevels = acuityLevels.OrderByDescending(level => level.angularSizeMinutes).ToList();
        }

        AssignButtonListeners();
        SetUIState(false); // Initially inactive

        // Determine if using 4 or 8 orientations based on button assignments
        if (upLeftButton != null && upLeftButton.gameObject.activeInHierarchy) { // Check if oblique buttons are setup
            activeOrientations = eightOrientations;
        } else {
            activeOrientations = standardOrientations;
        }
    }

    void AssignButtonListeners()
    {
        if (upButton != null) upButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.Up));
        if (downButton != null) downButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.Down));
        if (leftButton != null) leftButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.Left));
        if (rightButton != null) rightButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.Right));

        if (upLeftButton != null) upLeftButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.UpLeft));
        if (upRightButton != null) upRightButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.UpRight));
        if (downLeftButton != null) downLeftButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.DownLeft));
        if (downRightButton != null) downRightButton.onClick.AddListener(() => ProcessUserInput(LandoltCOrientation.DownRight));
    }

    void SetUIState(bool isTesting)
    {
        if (orientationInputPanel != null) orientationInputPanel.SetActive(isTesting);
        if (landoltCDisplayObject != null) landoltCDisplayObject.SetActive(isTesting);
        if (statusText != null && !isTesting) statusText.text = ""; // Clear status when not testing
        if (instructionText != null) instructionText.text = isTesting ? "Indicate the gap direction:" : "Press Start for Landolt C Test.";
    }

    public void StartTest(string eyeToTest) // "Right" or "Left"
    {
        if (acuityLevels == null || acuityLevels.Count == 0)
        {
            Debug.LogError("No acuity levels defined!");
            if(statusText != null) statusText.text = "Error: Acuity levels not set.";
            return;
        }
        if (landoltCPrefab == null)
        {
            Debug.LogError("Landolt C prefab not assigned!");
             if(statusText != null) statusText.text = "Error: Landolt C prefab missing.";
            return;
        }

        eyeBeingTested = eyeToTest;
        currentTestResult = new VisualAcuityTestResult_LandoltC(eyeBeingTested);
        // TODO: Implement eye occlusion based on eyeBeingTested

        currentLevelIndex = 0; // Start with the largest C (easiest level)
        presentationsAtCurrentLevel = 0;
        correctAtCurrentLevel = 0;
        incorrectAtCurrentLevelStrikes = 0;
        testIsRunning = true;

        if (landoltCDisplayObject == null)
        {
            landoltCDisplayObject = Instantiate(landoltCPrefab);
            // Position it at the virtualTestDistanceMeters straight ahead
            landoltCDisplayObject.transform.position = new Vector3(0, 0, virtualTestDistanceMeters); 
            // Parent it to camera rig if it should move with head, or keep world-fixed.
            // For acuity, typically world-fixed or fixed relative to a virtual testing apparatus.
        }
        
        SetUIState(true);
        ShowNextOptotype();
    }

    void ShowNextOptotype()
    {
        if (!testIsRunning || currentLevelIndex >= acuityLevels.Count)
        {
            EndTest();
            return;
        }

        LandoltCLevelInfo level = acuityLevels[currentLevelIndex];

        // Calculate required scale for the Landolt C object
        // Angular size (in radians) = Object Height / Distance
        // Object Height = Distance * Angular size (in radians)
        float angularSizeRadians = level.angularSizeMinutes * Mathf.Deg2Rad / 60f;
        float requiredHeight = virtualTestDistanceMeters * Mathf.Tan(angularSizeRadians); // Using Tan for accuracy

        // Assuming the Landolt C prefab is 1 Unity unit in height by default.
        // If not, a baseScaleFactor would be needed: requiredHeight / basePrefabHeight.
        if (landoltCDisplayObject != null) {
            landoltCDisplayObject.transform.localScale = new Vector3(requiredHeight, requiredHeight, requiredHeight);

            // Set random orientation
            currentOrientation = activeOrientations[Random.Range(0, activeOrientations.Length)];
            landoltCDisplayObject.transform.rotation = GetRotationForOrientation(currentOrientation);
            landoltCDisplayObject.SetActive(true);
        }
        
        if (instructionText != null) instructionText.text = $"Level: {level.acuityValueSnellen} ({presentationsAtCurrentLevel + 1}/{presentationsPerLevel})";
        if (statusText != null) statusText.text = ""; // Clear previous correct/incorrect
    }

    Quaternion GetRotationForOrientation(LandoltCOrientation orientation)
    {
        switch (orientation)
        {
            case LandoltCOrientation.Up: return Quaternion.Euler(0, 0, 90);
            case LandoltCOrientation.Down: return Quaternion.Euler(0, 0, -90);
            case LandoltCOrientation.Left: return Quaternion.Euler(0, 0, 180);
            case LandoltCOrientation.Right: return Quaternion.Euler(0, 0, 0); // Assuming prefab default is gap right
            case LandoltCOrientation.UpLeft: return Quaternion.Euler(0,0,135);
            case LandoltCOrientation.UpRight: return Quaternion.Euler(0,0,45);
            case LandoltCOrientation.DownLeft: return Quaternion.Euler(0,0,-135);
            case LandoltCOrientation.DownRight: return Quaternion.Euler(0,0,-45);
            default: return Quaternion.identity;
        }
    }

    public void ProcessUserInput(LandoltCOrientation chosenOrientation)
    {
        if (!testIsRunning || landoltCDisplayObject == null || !landoltCDisplayObject.activeSelf) return;

        LandoltCLevelInfo currentLevelInfo = acuityLevels[currentLevelIndex];
        bool isCorrect = (chosenOrientation == currentOrientation);

        currentTestResult.AddResponse(new LandoltCTrialResponse(
            currentLevelInfo.acuityValueSnellen,
            currentLevelInfo.logMARValue,
            currentLevelInfo.angularSizeMinutes,
            currentOrientation,
            chosenOrientation
        ));

        if (statusText != null) statusText.text = isCorrect ? "Correct" : "Incorrect";

        if (isCorrect)
        {
            correctAtCurrentLevel++;
        } else {
            incorrectAtCurrentLevelStrikes++;
        }
        presentationsAtCurrentLevel++;

        // Staircase/Progression Logic:
        if (presentationsAtCurrentLevel >= presentationsPerLevel || incorrectAtCurrentLevelStrikes >= incorrectStrikesToStop)
        {
            if (correctAtCurrentLevel >= correctNeededToPassLevel && incorrectAtCurrentLevelStrikes < incorrectStrikesToStop)
            {
                // Passed current level, try next smaller size
                currentTestResult.SetFinalAcuity(currentLevelInfo.acuityValueSnellen, currentLevelInfo.logMARValue); // Tentatively set acuity
                currentLevelIndex++;
                if (currentLevelIndex >= acuityLevels.Count) // All levels passed
                {
                    EndTest();
                    return;
                }
            }
            else
            {
                // Failed current level (not enough correct OR too many strikes)
                // The final acuity is the *previous* successfully passed level.
                // If it's the first level and failed, acuity is worse than the first level.
                if (currentLevelIndex == 0 && (correctAtCurrentLevel < correctNeededToPassLevel || incorrectAtCurrentLevelStrikes >= incorrectStrikesToStop) ) {
                    currentTestResult.SetFinalAcuity($"< {currentLevelInfo.acuityValueSnellen}", currentLevelInfo.logMARValue + 0.1f); // Indicate worse than largest
                }
                // else, the acuity was already set by the previous successful level.
                EndTest();
                return;
            }
            // Reset for next level
            presentationsAtCurrentLevel = 0;
            correctAtCurrentLevel = 0;
            incorrectAtCurrentLevelStrikes = 0;
        }
        
        // Brief pause before showing next C, or immediate
        Invoke(nameof(ShowNextOptotype), 0.5f); // 0.5s delay
    }

    void EndTest()
    {
        testIsRunning = false;
        SetUIState(false);
        if (landoltCDisplayObject != null) landoltCDisplayObject.SetActive(false);

        // If no level was ever passed, finalAcuity might still be "Not Determined" or worse than largest.
        if (currentTestResult.finalAcuitySnellen.Equals("Not Determined") && acuityLevels.Count > 0) {
             // This case implies they couldn't even pass the first (largest) level correctly.
             currentTestResult.SetFinalAcuity($"< {acuityLevels[0].acuityValueSnellen}", acuityLevels[0].logMARValue + 0.1f); // Or some other indicator
        }
        
        currentTestResult.testProcedureNotes = $"Presentations per level: {presentationsPerLevel}, Correct needed: {correctNeededToPassLevel}, Strikes to stop: {incorrectStrikesToStop}";

        if (statusText != null) statusText.text = $"Test Complete! Acuity: {currentTestResult.finalAcuitySnellen} (logMAR: {currentTestResult.finalAcuityLogMAR:F2})";
        if (instructionText != null) instructionText.text = "Test Finished.";
        
        string resultsJson = JsonUtility.ToJson(currentTestResult, true);
        Debug.Log("Landolt C Test Results (" + eyeBeingTested + "):\n" + resultsJson);

        // Conceptual: Save results and notify master coordinator
        // if (resultsSaver != null) resultsSaver.SaveVisualAcuityResults_LandoltC(currentTestResult);
        // if (masterCoordinator != null) masterCoordinator.OnTestModuleCompleted(this.GetType().Name, currentTestResult);
    }

    // Public method for UI button to start
    public void UISTART_TestRightEye() {
        StartTest("Right");
    }
    public void UISTART_TestLeftEye() {
        StartTest("Left");
    }
}
