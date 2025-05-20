using System;

[Serializable]
public class PatientInfo_CS
{
    public string patientId = "DefaultPatientID";
    public string patientName = "Default Patient Name";
    public string dateOfBirth = "01/01/1900"; // Example format
    public string gender = "Unknown";
    public string notes = "No additional notes."; // General notes about the patient

    // Add any other fields that might be relevant for session_info.txt
    // For example, if visual acuity from a general exam is stored:
    // public string generalVisualAcuityLeft = "N/A";
    // public string generalVisualAcuityRight = "N/A";
}
