using UnityEngine;
using System; // For Action

// Assuming PerimetryTestManager_VisualField is the C# class managing the perimetry test
// This could also be a more generic VRTestCoordinator or similar.
public class UnityProcedureCallback : AndroidJavaProxy
{
    // Actions to be invoked in Unity when Java calls back
    public static Action<string> OnStimulusReady_Static;
    public static Action<string> OnTestFinished_Static;
    public static Action<double> OnProgressUpdate_Static;
    public static Action<string, int> OnMessage_Static; // Message and type (int, map to enum in C#)
    public static Action<bool, double, double> OnFixationPointUpdate_Static; // Visible, X, Y
    // Add other callbacks as defined in the Java ProcedureCallback interface

    // If you prefer instance-based callbacks:
    // private PerimetryTestManager_VisualField _testManagerInstance;
    // public UnityProcedureCallback(PerimetryTestManager_VisualField managerInstance) : base("com.specvis.core.procedures.ProcedureCallback")
    // {
    //     _testManagerInstance = managerInstance;
    // }

    // Static callback version (simpler if only one perimetry test runs at a time)
    public UnityProcedureCallback() : base("com.specvis.core.procedures.ProcedureCallback") // Make sure this matches your Java interface path
    {
    }

    // Method names must exactly match the Java interface method names
    // Parameter types must also match (Java types map to C# types, e.g., String -> string, double -> double)

    // From original ProcedureCallback.java:
    // void onStimulusReadyToPresent(ProcedureBasicStimulus stimulusData);
    // void onStimulusHidden();
    // void onProgressUpdate(double progress, int stimuliCompleted, int totalStimuli);
    // void onTestFinished(ProcedureBasicData results); // Or a more generic result type
    // void onMessage(String message, MessageType type); // e.g., INFO, WARNING, ERROR
    // void onFixationPointUpdate(boolean visible, double x, double y); // x,y in degrees or pixels as needed

    // For this example, we'll use the simplified callbacks from the task description
    // and assume JSON strings are passed for complex objects.

    void onStimulusReady(String stimulusDetailsJson)
    {
        Debug.Log($"UnityProcedureCallback: onStimulusReady called with JSON: {stimulusDetailsJson}");
        OnStimulusReady_Static?.Invoke(stimulusDetailsJson);
        // Example with instance: _testManagerInstance?.HandleJavaStimulusReady(stimulusDetailsJson);
    }

    void onStimulusHidden() // Assuming this was also in your Java interface
    {
        Debug.Log("UnityProcedureCallback: onStimulusHidden called");
        // Example: OnStimulusHidden_Static?.Invoke();
        // This might trigger the C# side to hide the stimulus GameObject.
    }
    
    void onTestFinished(String resultsJson)
    {
        Debug.Log($"UnityProcedureCallback: onTestFinished called with JSON: {resultsJson}");
        OnTestFinished_Static?.Invoke(resultsJson);
        // Example with instance: _testManagerInstance?.HandleJavaTestFinished(resultsJson);
    }

    void onProgressUpdate(double progress, int stimuliCompleted, int totalStimuli) // Matched original callback
    {
        Debug.Log($"UnityProcedureCallback: onProgressUpdate called with progress: {progress}, completed: {stimuliCompleted}/{totalStimuli}");
        // If using the simplified one from task: OnProgressUpdate_Static?.Invoke(progress);
        // For the more detailed one:
        // Could pass a small class or struct, or just the double for simplicity if others aren't always needed
        OnProgressUpdate_Static?.Invoke(progress); // Passing only the double for this example
        // Or: PerimetryTestManager_VisualField.Instance.HandleJavaProgressUpdateDetailed(progress, stimuliCompleted, totalStimuli);
    }
    
    void onMessage(String message, int messageType) // Assuming MessageType enum maps to int
    {
        Debug.Log($"UnityProcedureCallback: onMessage called: '{message}', Type: {messageType}");
        OnMessage_Static?.Invoke(message, messageType);
        // Example with instance: _testManagerInstance?.HandleJavaMessage(message, (CoreMessageType)messageType);
    }

    void onFixationPointUpdate(bool visible, double x, double y)
    {
        Debug.Log($"UnityProcedureCallback: onFixationPointUpdate: visible={visible}, x={x}, y={y}");
        OnFixationPointUpdate_Static?.Invoke(visible, x, y);
    }
}

// Example enum for message types if you want to map the int from Java
// public enum CoreMessageType { INFO = 0, WARNING = 1, ERROR = 2 }
