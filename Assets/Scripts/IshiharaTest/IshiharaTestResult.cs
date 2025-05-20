using System;
using System.Collections.Generic;

[Serializable]
public class IshiharaTestResult
{
    public string testDateTime;
    public List<IshiharaUserResponse> userResponses;
    public int totalPlates;
    public int correctCount;
    public string finalScore; // e.g., "8/10"
    public string preliminaryDiagnosis; // e.g., "Normal Color Vision", "Possible Red-Green Deficiency"

    public IshiharaTestResult(List<IshiharaUserResponse> responses, int totalPlates, int correctCount)
    {
        this.testDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.userResponses = responses;
        this.totalPlates = totalPlates;
        this.correctCount = correctCount;
        this.finalScore = $"{correctCount}/{totalPlates}";
        this.preliminaryDiagnosis = "To be determined"; // Logic for this can be added later
    }
}
