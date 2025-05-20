using System;
using System.Collections.Generic;

[Serializable]
public class VisualAcuityTestResult_LandoltC
{
    public string testDateTime;
    public string eyeTested; // "Right" or "Left"
    public string finalAcuitySnellen;
    public float finalAcuityLogMAR;
    public List<LandoltCTrialResponse> responses;
    public string testProcedureNotes; // e.g., "Staircase: 3 correct for smaller, 1 incorrect for larger"

    public VisualAcuityTestResult_LandoltC(string eye)
    {
        this.testDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.eyeTested = eye;
        this.responses = new List<LandoltCTrialResponse>();
        this.finalAcuitySnellen = "Not Determined";
        this.finalAcuityLogMAR = -99f; // Indicate not determined
        this.testProcedureNotes = "";
    }

    public void AddResponse(LandoltCTrialResponse response)
    {
        responses.Add(response);
    }

    public void SetFinalAcuity(string snellen, float logMAR)
    {
        this.finalAcuitySnellen = snellen;
        this.finalAcuityLogMAR = logMAR;
    }
}
