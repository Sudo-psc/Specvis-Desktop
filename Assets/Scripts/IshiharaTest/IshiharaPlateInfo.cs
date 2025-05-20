using UnityEngine;

[CreateAssetMenu(fileName = "IshiharaPlate", menuName = "Specvis/Ishihara Plate Info", order = 0)]
public class IshiharaPlateInfo : ScriptableObject
{
    public string plateID; // e.g., "Plate 1", "Plate 2"
    public string description; // For internal reference, e.g., "Standard 74"
    public string correctResponse; // e.g., "74", "29", "pattern"
    public string commonProtanResponse; // Response typical for protanopia/protanomaly
    public string commonDeutanResponse; // Response typical for deutanopia/deutanomaly
    // In a real scenario, might also include:
    // public Sprite plateImage; // To display the actual plate
    // public PlateType plateType; // e.g., Vanishing, Transformation, HiddenDigit, Classification
}

// public enum PlateType { Vanishing, Transformation, HiddenDigit, Classification, Diagnostic }
// Not strictly needed for this task, but good for future expansion.
