using System;
using System.Collections.Generic;

public enum D15ConfusionAxis
{
    None,
    Protan,
    Deutan,
    Tritan,
    Unclear // For patterns that don't clearly fit one axis
}

public enum D15Severity
{
    None,
    Mild,
    Moderate,
    Strong,
    Undetermined
}

[Serializable]
public class FarnsworthD15TestResult
{
    public string testDateTime;
    public List<int> userCapOrder; // List of cap numbers (1-15) in the order the user placed them
    public int errorScore;
    public D15ConfusionAxis confusionAxis;
    public D15Severity severity;
    public List<Vector2Int> crossoverPairs; // Pairs of cap numbers that form crossovers (e.g., (capA, capB) crosses (capC, capD))

    public FarnsworthD15TestResult()
    {
        this.testDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.userCapOrder = new List<int>();
        this.crossoverPairs = new List<Vector2Int>();
        this.confusionAxis = D15ConfusionAxis.None;
        this.severity = D15Severity.Undetermined;
    }
}
