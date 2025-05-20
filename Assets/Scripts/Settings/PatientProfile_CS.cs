using System;

[Serializable]
public class PatientProfile_CS
{
    public string patientId = "P001"; // Example: Can be generated or input
    public string patientName = "Default Patient";
    public string dateOfBirth = "01/01/1900"; // Store as string for simplicity, parse if needed
    public string gender = "Unknown";
    public string notes = "No specific notes.";

    // Add other fields as needed, e.g., medical history summary
}
