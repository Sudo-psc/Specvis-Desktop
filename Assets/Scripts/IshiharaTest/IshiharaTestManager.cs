using UnityEngine;
using UnityEngine.UI; // For basic UI elements if not using TextMeshPro exclusively
using TMPro; // For TextMeshPro elements
using System.Collections.Generic;
using System.Linq; // For ToList if using array

public class IshiharaTestManager : MonoBehaviour
{
    [Header("Plate Data")]
    public List<IshiharaPlateInfo> plateSequence; // Assign ScriptableObjects in Inspector

    [Header("UI References")]
    public TextMeshProUGUI plateDisplayArea; // Shows current plate ID and instructions
    public TextMeshProUGUI statusMessageArea;  // Shows test complete, score, etc.
    public GameObject numberInputPanel;      // Parent GameObject for number input buttons
    public Button[] numberButtons;           // Assign 0-9 buttons
    public Button patternButton;             // Button for "Pattern/Unclear"
    public Button nothingButton;             // Button for "Nothing"
    public Button nextPlateButton;           // Optional: For manual advance or if no input is made

    private int currentPlateIndex = -1;
    private List<IshiharaUserResponse> userResponses;
    private IshiharaTestResult currentTestResult;

    private enum TestState { Idle, DisplayingPlate, WaitingForInput, TestFinished }
    private TestState currentState = TestState.Idle;

    // Conceptual reference to other managers/savers
    // public ResultsSaver resultsSaver; // Assign this if ResultsSaver is ready
    // public MasterTestCoordinator masterCoordinator; // To signal test completion

    void Start()
    {
        // Initialize UI and button listeners
        InitializeUI();
        // resultsSaver = FindObjectOfType<ResultsSaver>(); // Example: Find if present
        // masterCoordinator = FindObjectOfType<MasterTestCoordinator>(); // Example
    }

    void InitializeUI()
    {
        if (numberButtons != null)
        {
            for (int i = 0; i < numberButtons.Length; i++)
            {
                if (numberButtons[i] != null)
                {
                    string numberValue = i.ToString();
                    numberButtons[i].onClick.AddListener(() => ProcessUserInput(numberValue));
                }
            }
        }

        if (patternButton != null)
            patternButton.onClick.AddListener(() => ProcessUserInput("pattern"));
        if (nothingButton != null)
            nothingButton.onClick.AddListener(() => ProcessUserInput("nothing"));
        
        if (nextPlateButton != null) { // Optional: if manual advance is needed
            nextPlateButton.onClick.AddListener(ShowNextPlateIfAllowed);
            nextPlateButton.gameObject.SetActive(false); // Typically hidden until needed
        }


        if (plateDisplayArea != null) plateDisplayArea.text = "Press Start to begin Ishihara Test.";
        if (statusMessageArea != null) statusMessageArea.text = "";
        if (numberInputPanel != null) numberInputPanel.SetActive(false);
    }
    
    private void ShowNextPlateIfAllowed() {
        if (currentState == TestState.WaitingForInput || currentState == TestState.DisplayingPlate) {
            // Could record a "no response" or "skipped" if that's a valid test action
            // For now, just advances. If user made an input, ProcessUserInput would have already advanced.
            ShowNextPlate();
        }
    }

    public void StartTest()
    {
        if (plateSequence == null || plateSequence.Count == 0)
        {
            if (statusMessageArea != null) statusMessageArea.text = "Error: No Ishihara plates loaded.";
            Debug.LogError("IshiharaTestManager: Plate sequence is not set or is empty.");
            currentState = TestState.Idle;
            return;
        }

        currentPlateIndex = -1;
        userResponses = new List<IshiharaUserResponse>();
        currentState = TestState.DisplayingPlate;
        if (statusMessageArea != null) statusMessageArea.text = "";
        if (numberInputPanel != null) numberInputPanel.SetActive(true);
        ShowNextPlate();
    }

    void ShowNextPlate()
    {
        currentPlateIndex++;
        if (currentPlateIndex < plateSequence.Count)
        {
            IshiharaPlateInfo currentPlate = plateSequence[currentPlateIndex];
            if (plateDisplayArea != null)
            {
                plateDisplayArea.text = $"Plate {currentPlateIndex + 1} of {plateSequence.Count}\nID: {currentPlate.plateID}\n\nWhat do you see?";
            }
            // Clear previous input visualization if any (not implemented here)
            currentState = TestState.WaitingForInput;
            if (nextPlateButton != null) nextPlateButton.gameObject.SetActive(true); // Show manual next button
        }
        else
        {
            EndTest();
        }
    }

    public void ProcessUserInput(string selectedValue)
    {
        if (currentState != TestState.WaitingForInput) return;
        if (currentPlateIndex < 0 || currentPlateIndex >= plateSequence.Count) return;

        IshiharaPlateInfo currentPlate = plateSequence[currentPlateIndex];
        IshiharaUserResponse response = new IshiharaUserResponse(currentPlate.plateID, selectedValue, currentPlate.correctResponse);
        userResponses.Add(response);

        Debug.Log($"Plate: {currentPlate.plateID}, User Answer: {selectedValue}, Correct: {currentPlate.correctResponse}, IsCorrect: {response.isCorrect}");

        currentState = TestState.DisplayingPlate; // Transition state before showing next plate
        ShowNextPlate();
    }

    void EndTest()
    {
        currentState = TestState.TestFinished;
        if (numberInputPanel != null) numberInputPanel.SetActive(false);
        if (nextPlateButton != null) nextPlateButton.gameObject.SetActive(false);

        int correctCount = 0;
        foreach (var response in userResponses)
        {
            if (response.isCorrect)
            {
                correctCount++;
            }
        }

        currentTestResult = new IshiharaTestResult(userResponses, plateSequence.Count, correctCount);
        
        // Basic diagnosis logic (can be expanded significantly)
        if (correctCount == plateSequence.Count) {
            currentTestResult.preliminaryDiagnosis = "Normal Color Vision (based on this screening)";
        } else if (correctCount < plateSequence.Count * 0.7) { // Example threshold
            currentTestResult.preliminaryDiagnosis = "Possible Color Vision Deficiency";
        } else {
            currentTestResult.preliminaryDiagnosis = "Further testing may be required";
        }

        if (plateDisplayArea != null) plateDisplayArea.text = "Test Complete!";
        if (statusMessageArea != null)
        {
            statusMessageArea.text = $"Score: {currentTestResult.finalScore}\nDiagnosis: {currentTestResult.preliminaryDiagnosis}";
        }

        // Save results (conceptual call)
        // if (resultsSaver != null)
        // {
        //     resultsSaver.SaveIshiharaResults(currentTestResult);
        // }
        // else
        // {
        Debug.Log("IshiharaTestManager: ResultsSaver not assigned. Results not saved externally.");
        // }
        // Log results to console for now
        string resultsJson = JsonUtility.ToJson(currentTestResult, true);
        Debug.Log("Ishihara Test Results:\n" + resultsJson);


        // Notify MasterTestCoordinator (conceptual)
        // if (masterCoordinator != null)
        // {
        //     masterCoordinator.OnTestModuleCompleted(this.GetType().Name, currentTestResult);
        // }
    }

    // Public method to be called by a UI button to start the test
    public void UISTART_StartIshiharaTest() {
        StartTest();
    }
}
