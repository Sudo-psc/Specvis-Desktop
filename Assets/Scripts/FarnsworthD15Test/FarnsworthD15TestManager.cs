using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro; // For TextMeshPro elements

public class FarnsworthD15TestManager : MonoBehaviour
{
    [Header("Cap Data & Prefabs")]
    public List<D15CapInfo> allCapInfos; // Assign all 16 D15CapInfo ScriptableObjects here
    public GameObject capPrefab; // Assign a 3D cylinder/capsule prefab for the caps

    [Header("Scene References")]
    public Transform presentationArea; // Parent transform where randomized caps appear
    public Transform arrangementTray;  // Parent transform for slots
    public Transform pilotCapSlot;     // Specific slot for the pilot cap
    public List<Transform> testCapSlots; // 15 slots for test caps, ordered 0-14

    [Header("UI Elements")]
    public TextMeshProUGUI instructionText;
    public Button startTestButton;
    public Button submitButton;

    private List<GameObject> spawnedCapsInPresentation;
    private List<GameObject> placedCapsInArrangement; // GameObjects of caps in arrangement tray slots
    private D15CapInfo pilotCapInfo;
    private List<D15CapInfo> currentTestCapsInfo; // The 15 test caps' ScriptableObject data
    
    // Tracks which D15CapInfo is in which slot (index of testCapSlots)
    private Dictionary<int, D15CapInfo> capInSlotMapping; 

    public enum D15TestState { Idle, Arranging, Finished }
    private D15TestState currentState = D15TestState.Idle;

    // For VR interaction (simplified)
    private GameObject currentlyHeldCap = null;
    private D15CapInfo currentlyHeldCapInfo = null;
    private int originalSlotIndexOfHeldCap = -1; // -1 if picked from presentation area

    // Conceptual reference
    // public ResultsSaver resultsSaver;
    // public MasterTestCoordinator masterCoordinator;


    void Start()
    {
        spawnedCapsInPresentation = new List<GameObject>();
        placedCapsInArrangement = new List<GameObject>(new GameObject[15]); // Initialize with nulls for 15 slots
        capInSlotMapping = new Dictionary<int, D15CapInfo>();

        if (startTestButton != null) startTestButton.onClick.AddListener(StartTest);
        if (submitButton != null) submitButton.onClick.AddListener(EndTest);

        //resultsSaver = FindObjectOfType<ResultsSaver>();
        //masterCoordinator = FindObjectOfType<MasterTestCoordinator>();

        SetUIState(D15TestState.Idle);
    }

    void SetUIState(D15TestState newState)
    {
        currentState = newState;
        if (startTestButton != null) startTestButton.gameObject.SetActive(newState == D15TestState.Idle);
        if (submitButton != null) submitButton.gameObject.SetActive(newState == D15TestState.Arranging);
        
        if (newState == D15TestState.Idle)
        {
            if(instructionText != null) instructionText.text = "Press 'Start Test' to begin the Farnsworth D-15 test.";
            ClearCaps();
        }
        else if (newState == D15TestState.Arranging)
        {
            if(instructionText != null) instructionText.text = "Arrange the colored caps in order of hue, starting from the pilot cap.";
        }
        else if (newState == D15TestState.Finished)
        {
            // Message updated by EndTest()
        }
    }

    public void StartTest()
    {
        if (allCapInfos == null || allCapInfos.Count < 16)
        {
            Debug.LogError("Not enough D15CapInfo objects assigned!");
            if(instructionText != null) instructionText.text = "Error: Cap data missing.";
            return;
        }
        SetUIState(D15TestState.Arranging);
        ClearCaps(); // Clear previous test if any

        pilotCapInfo = allCapInfos.FirstOrDefault(c => c.capNumber == 0);
        currentTestCapsInfo = allCapInfos.Where(c => c.capNumber != 0).OrderBy(c => Random.value).ToList(); // Shuffle

        // Place Pilot Cap
        if (pilotCapInfo != null && pilotCapSlot != null)
        {
            GameObject pilotInstance = InstantiateCap(pilotCapInfo, pilotCapSlot.position, pilotCapSlot.rotation, pilotCapSlot);
            // Make pilot cap non-interactive if needed, or handle specially
            // For simplicity, it's just placed.
        }

        // Display randomized test caps in presentation area
        for (int i = 0; i < currentTestCapsInfo.Count; i++)
        {
            // Example: Arrange in a grid or circle within presentationArea
            Vector3 position = presentationArea.position + new Vector3((i % 5 - 2) * 0.2f, 0, (i / 5) * 0.2f); 
            GameObject capInstance = InstantiateCap(currentTestCapsInfo[i], position, Quaternion.identity, presentationArea);
            spawnedCapsInPresentation.Add(capInstance);
            
            // Add VR grabbable component and attach info
            var grabbable = capInstance.AddComponent<D15GrabbableCap>(); // Simplified grabbable script
            grabbable.Initialize(this, currentTestCapsInfo[i]);
        }
    }

    GameObject InstantiateCap(D15CapInfo capInfo, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject capInstance = Instantiate(capPrefab, position, rotation, parent);
        capInstance.name = $"Cap_{capInfo.capNumber}";
        var renderer = capInstance.GetComponentInChildren<MeshRenderer>(); // Assuming model has renderer on child
        if (renderer == null) renderer = capInstance.GetComponent<MeshRenderer>(); // Or on parent

        if (renderer != null)
        {
            renderer.material.color = capInfo.standardColorRGB;
        }
        else
        {
            Debug.LogWarning($"No renderer found on cap prefab instance for Cap {capInfo.capNumber}");
        }
        // Add a TextMeshPro component to display cap number on top/bottom (optional)
        TextMeshPro tmp = capInstance.GetComponentInChildren<TextMeshPro>();
        if (tmp != null) {
             tmp.text = capInfo.capNumber.ToString();
        }
        return capInstance;
    }
    
    public void OnCapPickedUp(GameObject capObject, D15CapInfo capInfo) {
        currentlyHeldCap = capObject;
        currentlyHeldCapInfo = capInfo;

        // Check if it was picked from an arrangement slot
        for(int i=0; i < testCapSlots.Count; i++) {
            if (capInSlotMapping.TryGetValue(i, out D15CapInfo infoInSlot) && infoInSlot == capInfo) {
                originalSlotIndexOfHeldCap = i;
                placedCapsInArrangement[i] = null; // Remove from array
                capInSlotMapping.Remove(i);       // Remove from dictionary
                break;
            }
        }
        if (spawnedCapsInPresentation.Contains(capObject)) {
            spawnedCapsInPresentation.Remove(capObject);
            originalSlotIndexOfHeldCap = -1; // Indicates picked from presentation area
        }
        // Parent to hand/controller (handled by VR interaction system)
        Debug.Log($"Picked up Cap {capInfo.capNumber}");
    }

    public void OnCapTryPlace(Transform potentialSlot) {
        if (currentlyHeldCap == null || currentlyHeldCapInfo == null) return;

        int slotIndex = -1;
        for (int i = 0; i < testCapSlots.Count; i++) {
            if (testCapSlots[i] == potentialSlot) {
                slotIndex = i;
                break;
            }
        }

        if (slotIndex != -1) { // It's a valid test cap slot
            if (placedCapsInArrangement[slotIndex] == null) { // Slot is empty
                PlaceCapInSlot(currentlyHeldCap, currentlyHeldCapInfo, slotIndex);
                currentlyHeldCap = null;
                currentlyHeldCapInfo = null;
            } else {
                // Slot is occupied - option 1: return to original pos, option 2: swap (more complex)
                // For simplicity, returning to original position or presentation area
                ReturnHeldCapToOrigin(); 
                Debug.Log("Slot occupied. Cap returned.");
            }
        } else {
            // Not a valid slot, return cap
            ReturnHeldCapToOrigin();
            Debug.Log("Not a valid slot. Cap returned.");
        }
    }

    void PlaceCapInSlot(GameObject capObject, D15CapInfo capInfo, int slotIndex) {
        capObject.transform.SetParent(testCapSlots[slotIndex]);
        capObject.transform.localPosition = Vector3.zero; // Center in slot
        capObject.transform.localRotation = Quaternion.identity;
        
        placedCapsInArrangement[slotIndex] = capObject;
        capInSlotMapping[slotIndex] = capInfo;

        // Disable further interaction with placed cap or change its layer (handled by D15GrabbableCap)
        var grabbable = capObject.GetComponent<D15GrabbableCap>();
        if (grabbable != null) grabbable.isPlaced = true;

        Debug.Log($"Placed Cap {capInfo.capNumber} in Slot {slotIndex + 1}");
    }

    void ReturnHeldCapToOrigin() {
        if (currentlyHeldCap == null) return;

        if (originalSlotIndexOfHeldCap != -1) { // Was picked from an arrangement slot
            PlaceCapInSlot(currentlyHeldCap, currentlyHeldCapInfo, originalSlotIndexOfHeldCap);
        } else { // Was picked from presentation area
            // For simplicity, just drop it or re-add to presentation list.
            // A more robust system would return it to a defined "holding" area or its original presentation spot.
            spawnedCapsInPresentation.Add(currentlyHeldCap); // Re-add to list if needed
            currentlyHeldCap.transform.SetParent(presentationArea); // Or a specific return area
            // Needs a position logic here if returning to presentation area
            Debug.Log("Cap returned to presentation area (simplified).");
        }
        currentlyHeldCap = null;
        currentlyHeldCapInfo = null;
    }


    void ClearCaps()
    {
        foreach (GameObject cap in spawnedCapsInPresentation) Destroy(cap);
        spawnedCapsInPresentation.Clear();

        foreach (GameObject cap in placedCapsInArrangement) {
            if (cap != null) Destroy(cap);
        }
        // Correctly clear the list for the next test
        for(int i=0; i<placedCapsInArrangement.Count; ++i) placedCapsInArrangement[i] = null;
        
        capInSlotMapping.Clear();

        // Destroy pilot cap if it was instantiated, or disable it
        if (pilotCapSlot != null && pilotCapSlot.childCount > 0)
        {
            Destroy(pilotCapSlot.GetChild(0).gameObject);
        }
    }

    public void EndTest()
    {
        if (currentState != D15TestState.Arranging) return;

        List<int> userOrderedCapNumbers = new List<int>();
        for (int i = 0; i < testCapSlots.Count; i++)
        {
            if (capInSlotMapping.TryGetValue(i, out D15CapInfo capInfo))
            {
                userOrderedCapNumbers.Add(capInfo.capNumber);
            }
            else
            {
                userOrderedCapNumbers.Add(-1); // Indicates an empty slot, should ideally be handled
                Debug.LogWarning($"Slot {i+1} is empty. Test submitted with incomplete arrangement.");
            }
        }
        
        if (userOrderedCapNumbers.Count < 15 && instructionText != null) {
             instructionText.text = $"Please arrange all 15 caps before submitting. {userOrderedCapNumbers.Count}/15 placed.";
             return; // Don't end test if not all caps placed
        }


        FarnsworthD15TestResult result = new FarnsworthD15TestResult();
        result.userCapOrder = userOrderedCapNumbers;

        // Call scoring logic
        D15Scoring.CalculateScore(result, allCapInfos); // Static method call

        SetUIState(D15TestState.Finished);
        if(instructionText != null) instructionText.text = $"Test Complete!\nError Score: {result.errorScore}\nAxis: {result.confusionAxis}\nSeverity: {result.severity}";

        // Log results
        string resultsJson = JsonUtility.ToJson(result, true);
        Debug.Log("Farnsworth D-15 Test Results:\n" + resultsJson);

        // Conceptual: Save results and notify master coordinator
        // if (resultsSaver != null) resultsSaver.SaveFarnsworthD15Results(result);
        // if (masterCoordinator != null) masterCoordinator.OnTestModuleCompleted(this.GetType().Name, result);
    }
}
