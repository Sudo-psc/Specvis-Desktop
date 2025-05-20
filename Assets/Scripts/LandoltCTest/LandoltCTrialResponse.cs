using System;

public enum LandoltCOrientation
{
    NotSet, // Default or error
    Up,
    Down,
    Left,
    Right,
    // Optional: Oblique orientations if used (e.g., UpLeft, UpRight, DownLeft, DownRight for 8 options)
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

[Serializable]
public class LandoltCTrialResponse
{
    public string acuityLevelSnellenPresented;
    public float logMARPresented;
    public float angularSizeMinutesPresented;
    public LandoltCOrientation actualOrientation;
    public LandoltCOrientation chosenOrientation;
    public bool isCorrect;
    public float responseTimeSeconds; // Optional: time taken to respond

    public LandoltCTrialResponse(string snellen, float logMAR, float angularSize, LandoltCOrientation actual, LandoltCOrientation chosen, float timeTaken = 0f)
    {
        this.acuityLevelSnellenPresented = snellen;
        this.logMARPresented = logMAR;
        this.angularSizeMinutesPresented = angularSize;
        this.actualOrientation = actual;
        this.chosenOrientation = chosen;
        this.isCorrect = (actual == chosen && actual != LandoltCOrientation.NotSet);
        this.responseTimeSeconds = timeTaken;
    }
}
