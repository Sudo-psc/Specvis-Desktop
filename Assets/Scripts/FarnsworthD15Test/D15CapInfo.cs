using UnityEngine;

[CreateAssetMenu(fileName = "D15Cap", menuName = "Specvis/Farnsworth D15 Cap Info", order = 1)]
public class D15CapInfo : ScriptableObject
{
    public int capNumber; // 0 for Pilot, 1-15 for Test Caps
    public Color standardColorRGB; // sRGB color for display
    public int correctOrder; // Its position in the correct hue sequence (0 for Pilot, 1-15 for others in sequence)
    public string munsellNotation; // e.g., "5R 4/12" (for reference)
}
