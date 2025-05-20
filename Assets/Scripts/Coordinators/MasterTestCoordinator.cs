using UnityEngine;
using System.Collections.Generic; // For potential future use with multiple instances of settings/results

// Assuming these settings and result classes are defined elsewhere:
// using Assets.Scripts.Settings; // For PatientProfile_CS, PerimetrySettings_CS, etc.
// using Assets.Scripts.Results; // For result data structures
// Assuming individual test managers are defined:
// using Assets.Scripts.Perimetry;
// using Assets.Scripts.IshiharaTest;
// using Assets.Scripts.FarnsworthD15Test;
// using Assets.Scripts.LandoltCTest;


public class MasterTestCoordinator : MonoBehaviour
{
    public static MasterTestCoordinator Instance { get; private set; }

    public enum AppState
    {
        MainMenu,
        ShowingSettings_Patient,
        ShowingSettings_Perimetry,
        ShowingSettings_Ishihara,
        ShowingSettings_D15,
        ShowingSettings_LandoltC,
        ShowingSettings_Global, // For things like display calibration
        RunningPerimetry,
        RunningIshihara,
        RunningD15,
        RunningLandoltC,
        ShowingResults
    }
    public AppState currentAppState { get; private set; }

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsRootPanel; // Parent panel for all settings sections
    public GameObject patientSettingsPanel; // Specific UI panel for patient selection/editing
    public GameObject perimetrySettingsPanel;
    public GameObject ishiharaSettingsPanel;
    public GameObject d15SettingsPanel;
    public GameObject landoltCSettingsPanel;
    public GameObject globalDisplaySettingsPanel; // For screen calibration etc.

    public GameObject perimetryTestPanel;   // Panel active during the perimetry test itself
    public GameObject ishiharaTestPanel;    // Panel for Ishihara test execution
    public GameObject d15TestPanel;         // Panel for D-15 test execution
    public GameObject landoltCTestPanel;    // Panel for Landolt C test execution
    public GameObject resultsDisplayPanel;  // Panel for showing results summary

    [Header("Test Managers")]
    public PerimetryTestManager_VisualField perimetryTestManager; // Assign in Inspector
    public IshiharaTestManager ishiharaTestManager;             // Assign in Inspector
    public FarnsworthD15TestManager farnsworthD15TestManager;   // Assign in Inspector
    public VisualAcuityTestManager_LandoltC landoltCTestManager; // Assign in Inspector

    // Current context
    private PatientProfile_CS _currentPatient;
    private PerimetrySettings_CS _currentPerimetrySettings;
    private IshiharaSettings_CS _currentIshiharaSettings;
    private D15Settings_CS _currentD15Settings;
    private VisualAcuitySettings_CS _currentLandoltCSettings;
    // private GlobalDisplaySettings_CS _currentGlobalDisplaySettings;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if it needs to persist across scene loads
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadAllSettings(); // Load default/saved settings at start
        ShowMainMenu();
    }

    void LoadAllSettings()
    {
        // Load or create default patient (could be a "Select Patient" screen first)
        _currentPatient = SettingsManager.LoadSettings<PatientProfile_CS>("PatientProfile", "LastUsed") ?? new PatientProfile_CS();
        if (_currentPatient.patientId == "P001" && _currentPatient.patientName == "Default Patient") {
            // If it's truly the default, maybe prompt for patient creation or load a specific one
            // For now, we'll use it.
        }


        _currentPerimetrySettings = SettingsManager.LoadSettings<PerimetrySettings_CS>("PerimetrySettings", _currentPatient.patientId) ?? new PerimetrySettings_CS();
        _currentIshiharaSettings = SettingsManager.LoadSettings<IshiharaSettings_CS>("IshiharaSettings", _currentPatient.patientId) ?? new IshiharaSettings_CS();
        _currentD15Settings = SettingsManager.LoadSettings<D15Settings_CS>("D15Settings", _currentPatient.patientId) ?? new D15Settings_CS();
        _currentLandoltCSettings = SettingsManager.LoadSettings<VisualAcuitySettings_CS>("LandoltCSettings", _currentPatient.patientId) ?? new VisualAcuitySettings_CS();
        // _currentGlobalDisplaySettings = SettingsManager.LoadSettings<GlobalDisplaySettings_CS>("GlobalDisplaySettings") ?? new GlobalDisplaySettings_CS();

        // TODO: Update settings UI elements with these loaded values when settings panels are shown
    }

    void SetPanelActive(GameObject panelToActivate)
    {
        mainMenuPanel.SetActive(panelToActivate == mainMenuPanel);
        settingsRootPanel.SetActive(
            panelToActivate == settingsRootPanel ||
            panelToActivate == patientSettingsPanel ||
            panelToActivate == perimetrySettingsPanel ||
            panelToActivate == ishiharaSettingsPanel ||
            panelToActivate == d15SettingsPanel ||
            panelToActivate == landoltCSettingsPanel ||
            panelToActivate == globalDisplaySettingsPanel
        );

        // Specific settings sub-panels (children of settingsRootPanel)
        if (settingsRootPanel.activeSelf) {
            patientSettingsPanel.SetActive(panelToActivate == patientSettingsPanel);
            perimetrySettingsPanel.SetActive(panelToActivate == perimetrySettingsPanel);
            ishiharaSettingsPanel.SetActive(panelToActivate == ishiharaSettingsPanel);
            d15SettingsPanel.SetActive(panelToActivate == d15SettingsPanel);
            landoltCSettingsPanel.SetActive(panelToActivate == landoltCSettingsPanel);
            globalDisplaySettingsPanel.SetActive(panelToActivate == globalDisplaySettingsPanel);
        } else { // Ensure all settings sub-panels are off if root is off
            patientSettingsPanel.SetActive(false);
            perimetrySettingsPanel.SetActive(false);
            ishiharaSettingsPanel.SetActive(false);
            d15SettingsPanel.SetActive(false);
            landoltCSettingsPanel.SetActive(false);
            globalDisplaySettingsPanel.SetActive(false);
        }


        perimetryTestPanel.SetActive(panelToActivate == perimetryTestPanel);
        ishiharaTestPanel.SetActive(panelToActivate == ishiharaTestPanel);
        d15TestPanel.SetActive(panelToActivate == d15TestPanel);
        landoltCTestPanel.SetActive(panelToActivate == landoltCTestPanel);
        resultsDisplayPanel.SetActive(panelToActivate == resultsDisplayPanel);
    }

    public void ShowMainMenu()
    {
        currentAppState = AppState.MainMenu;
        SetPanelActive(mainMenuPanel);
        Debug.Log("State: Main Menu");
    }

    public void ShowSettings(string specificSettingsSection = "Patient") // Default to patient settings
    {
        settingsRootPanel.SetActive(true); // Ensure root settings panel is on
        switch (specificSettingsSection.ToLower())
        {
            case "patient":
                currentAppState = AppState.ShowingSettings_Patient;
                SetPanelActive(patientSettingsPanel);
                // TODO: Load patient profiles into UI, allow selection/creation
                // Example: patientSettingsPanel.GetComponent<PatientSettingsUI>().Populate(_currentPatient);
                break;
            case "perimetry":
                currentAppState = AppState.ShowingSettings_Perimetry;
                SetPanelActive(perimetrySettingsPanel);
                // TODO: Populate perimetrySettingsPanel UI with _currentPerimetrySettings
                // Example: perimetrySettingsPanel.GetComponent<PerimetrySettingsUI>().Populate(_currentPerimetrySettings);
                break;
            case "ishihara":
                currentAppState = AppState.ShowingSettings_Ishihara;
                SetPanelActive(ishiharaSettingsPanel);
                // TODO: Populate ishiharaSettingsPanel UI with _currentIshiharaSettings
                break;
            case "d15":
                currentAppState = AppState.ShowingSettings_D15;
                SetPanelActive(d15SettingsPanel);
                // TODO: Populate d15SettingsPanel UI with _currentD15Settings
                break;
            case "landoltc":
                currentAppState = AppState.ShowingSettings_LandoltC;
                SetPanelActive(landoltCSettingsPanel);
                // TODO: Populate landoltCSettingsPanel UI with _currentLandoltCSettings
                break;
            case "globaldisplay":
                 currentAppState = AppState.ShowingSettings_Global;
                 SetPanelActive(globalDisplaySettingsPanel);
                 // TODO: Populate global display settings UI
                 break;
            default:
                Debug.LogWarning($"Unknown settings section: {specificSettingsSection}. Defaulting to Patient settings.");
                currentAppState = AppState.ShowingSettings_Patient;
                SetPanelActive(patientSettingsPanel);
                break;
        }
        Debug.Log($"State: Showing Settings - {specificSettingsSection}");
    }

    // --- Methods to be called by UI buttons within specific settings panels ---
    public void SelectPatient(PatientProfile_CS selectedPatient) {
        _currentPatient = selectedPatient;
        // Optionally save this as the "last used" patient
        SettingsManager.SaveSettings(_currentPatient, "PatientProfile", "LastUsed");
        Debug.Log($"Patient selected: {_currentPatient.patientName}");
    }

    public void UpdatePerimetrySettings(PerimetrySettings_CS newSettings) { // Called by PerimetrySettingsUI
        _currentPerimetrySettings = newSettings;
    }
    // Similar Update methods for Ishihara, D15, LandoltC settings

    // --- Methods to Start Tests (called by "Start Test" buttons in respective settings UIs) ---
    public void TriggerStartPerimetryTest() // Called by UI button
    {
        // Settings should have been updated via UpdatePerimetrySettings or retrieved from UI directly
        if (_currentPatient == null || _currentPerimetrySettings == null) {
            Debug.LogError("Patient or Perimetry settings not set!"); return;
        }
        StartPerimetryTest(_currentPatient, _currentPerimetrySettings);
    }

    private void StartPerimetryTest(PatientProfile_CS patient, PerimetrySettings_CS settings)
    {
        currentAppState = AppState.RunningPerimetry;
        SetPanelActive(perimetryTestPanel);
        Debug.Log($"Starting Perimetry Test for {patient.patientName} with settings: {settings.settingsName}");
        // perimetryTestManager.InitializeAndStartTest(patient, settings); // Assumes manager has such a method
    }

    public void TriggerStartIshiharaTest()
    {
        if (_currentPatient == null || _currentIshiharaSettings == null) {
            Debug.LogError("Patient or Ishihara settings not set!"); return;
        }
        StartIshiharaTest(_currentPatient, _currentIshiharaSettings);
    }

    private void StartIshiharaTest(PatientProfile_CS patient, IshiharaSettings_CS settings)
    {
        currentAppState = AppState.RunningIshihara;
        SetPanelActive(ishiharaTestPanel);
        Debug.Log($"Starting Ishihara Test for {patient.patientName}");
        ishiharaTestManager.StartTest(); // Assumes manager takes settings implicitly or has them pre-assigned
    }

    public void TriggerStartD15Test()
    {
         if (_currentPatient == null || _currentD15Settings == null) {
            Debug.LogError("Patient or D15 settings not set!"); return;
        }
        StartD15Test(_currentPatient, _currentD15Settings);
    }
    private void StartD15Test(PatientProfile_CS patient, D15Settings_CS settings)
    {
        currentAppState = AppState.RunningD15;
        SetPanelActive(d15TestPanel);
        Debug.Log($"Starting D15 Test for {patient.patientName}");
        farnsworthD15TestManager.StartTest();
    }

    public void TriggerStartLandoltCTest(string eyeToTest) // UI button calls this with "Right" or "Left"
    {
        if (_currentPatient == null || _currentLandoltCSettings == null) {
            Debug.LogError("Patient or LandoltC settings not set!"); return;
        }
        StartLandoltCTest(_currentPatient, _currentLandoltCSettings, eyeToTest);
    }

    private void StartLandoltCTest(PatientProfile_CS patient, VisualAcuitySettings_CS settings, string eye)
    {
        currentAppState = AppState.RunningLandoltC;
        SetPanelActive(landoltCTestPanel);
        Debug.Log($"Starting Landolt C Test for {patient.patientName}, Eye: {eye}");
        landoltCTestManager.StartTest(eye); // Assumes manager applies settings from its own fields or gets them
    }

    public void OnTestCompleted(string testType, object resultData)
    {
        Debug.Log($"{testType} test completed.");
        currentAppState = AppState.ShowingResults;
        SetPanelActive(resultsDisplayPanel);

        // Here, you would typically cast resultData to its specific type
        // and pass it to a UI manager for the resultsDisplayPanel.
        // Also, invoke ResultsSaver.
        // Example:
        // ResultsDisplayUI resultsUI = resultsDisplayPanel.GetComponent<ResultsDisplayUI>();
        // resultsUI.DisplayResults(testType, resultData);

        DateTime testTimestamp = DateTime.Now; // Should ideally come from the resultData if recorded there

        if (resultData is PerimetryResult_CS perimetryResult) {
            ResultsSaver.SaveResultAsJson(perimetryResult, _currentPatient.patientId, "Perimetry_VisualField", testTimestamp);
            // Optionally trigger export:
            // ResultsSaver.ExportPerimetryToDesktopFormat(perimetryResult, _currentPatient, _currentPerimetrySettings, _currentPatient.patientId, testTimestamp);
        }
        else if (resultData is IshiharaTestResult ishiharaResult) { // Assuming IshiharaTestResult is the C# class
            ResultsSaver.SaveResultAsJson(ishiharaResult, _currentPatient.patientId, "Ishihara", testTimestamp);
        }
        else if (resultData is FarnsworthD15TestResult d15Result) { // Assuming FarnsworthD15TestResult is the C# class
             ResultsSaver.SaveResultAsJson(d15Result, _currentPatient.patientId, "FarnsworthD15", testTimestamp);
        }
        else if (resultData is VisualAcuityTestResult_LandoltC landoltCResult) { // Assuming VisualAcuityTestResult_LandoltC is the C# class
            ResultsSaver.SaveResultAsJson(landoltCResult, _currentPatient.patientId, $"LandoltC_{landoltCResult.eyeTested}", testTimestamp);
        }

        // After showing results, a button on resultsDisplayPanel would call ShowMainMenu() or ShowSettings()
    }
}
