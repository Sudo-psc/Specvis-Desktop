using UnityEngine;

[CreateAssetMenu(fileName = "NewIshiharaPlate", menuName = "Specvis/Ishihara Plate Info", order = 100)]
public class IshiharaPlateInfo : ScriptableObject
{
    public string plateID; // e.g., "Plate 1", "Plate 2", or the number depicted
    [Tooltip("What the user is expected to see if they have normal color vision.")]
    public string correctResponse; // e.g., "74", "29", "pattern", "nothing"
    [Tooltip("Response typical for protanopia/protanomaly.")]
    public string commonProtanResponse;
    [Tooltip("Response typical for deutanopia/deutanomaly.")]
    public string commonDeutanResponse;
    // Optional: Add a field for the image itself if you plan to display them
    // public Sprite plateImage;
}
