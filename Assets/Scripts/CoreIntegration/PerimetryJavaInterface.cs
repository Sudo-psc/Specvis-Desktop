using UnityEngine;

public class PerimetryJavaInterface
{
    private AndroidJavaObject _javaProcedureInstance;
    private const string JavaClassName = "com.specvis.core.procedures.PerimetryProcedureRunner"; // Example class name

    public bool IsInitialized { get; private set; } = false;

    public PerimetryJavaInterface()
    {
        // It's often better to initialize the Java object when needed,
        // rather than in the constructor, to handle potential Android context issues.
    }

    public bool InitializeProcedure(string settingsJson, AndroidJavaProxy callbackProxy)
    {
        if (_javaProcedureInstance == null)
        {
            try
            {
                AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                // Pass currentActivity if the Java class constructor needs it.
                // For this example, assuming default constructor or one that can get context if needed.
                _javaProcedureInstance = new AndroidJavaObject(JavaClassName /*, currentActivity (if needed) */);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to instantiate Java class {JavaClassName}: {e.Message}");
                IsInitialized = false;
                return false;
            }
        }

        if (_javaProcedureInstance != null)
        {
            try
            {
                _javaProcedureInstance.Call("initialize", settingsJson, callbackProxy);
                IsInitialized = true;
                Debug.Log("PerimetryJavaInterface: Java procedure initialized successfully.");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to call 'initialize' on Java object: {e.Message}");
                IsInitialized = false;
                return false;
            }
        }
        IsInitialized = false;
        return false;
    }

    public void StartProcedure()
    {
        if (!IsInitialized || _javaProcedureInstance == null)
        {
            Debug.LogError("PerimetryJavaInterface: Cannot start, procedure not initialized.");
            return;
        }
        _javaProcedureInstance.Call("startTest");
        Debug.Log("PerimetryJavaInterface: Called startTest() on Java object.");
    }

    public void SendResponse(bool seen)
    {
        if (!IsInitialized || _javaProcedureInstance == null)
        {
            Debug.LogError("PerimetryJavaInterface: Cannot send response, procedure not initialized.");
            return;
        }
        _javaProcedureInstance.Call("processUserResponse", seen);
        // Debug.Log($"PerimetryJavaInterface: Sent response ({seen}) to Java object."); // Can be too verbose
    }

    public void PauseProcedure()
    {
        if (!IsInitialized || _javaProcedureInstance == null)
        {
            Debug.LogError("PerimetryJavaInterface: Cannot pause, procedure not initialized.");
            return;
        }
        _javaProcedureInstance.Call("pauseTest");
        Debug.Log("PerimetryJavaInterface: Called pauseTest() on Java object.");
    }

    public void ResumeProcedure()
    {
        if (!IsInitialized || _javaProcedureInstance == null)
        {
            Debug.LogError("PerimetryJavaInterface: Cannot resume, procedure not initialized.");
            return;
        }
        _javaProcedureInstance.Call("resumeTest");
        Debug.Log("PerimetryJavaInterface: Called resumeTest() on Java object.");
    }

    public void CancelProcedure()
    {
        if (!IsInitialized || _javaProcedureInstance == null)
        {
            Debug.LogError("PerimetryJavaInterface: Cannot cancel, procedure not initialized.");
            return;
        }
        _javaProcedureInstance.Call("cancelTest");
        Debug.Log("PerimetryJavaInterface: Called cancelTest() on Java object.");
    }

    public void Release()
    {
        if (_javaProcedureInstance != null)
        {
            // Optional: Call a cleanup method on the Java side if it exists
            // _javaProcedureInstance.Call("dispose");
            _javaProcedureInstance.Dispose();
            _javaProcedureInstance = null;
            IsInitialized = false;
            Debug.Log("PerimetryJavaInterface: Java object released.");
        }
    }
}
