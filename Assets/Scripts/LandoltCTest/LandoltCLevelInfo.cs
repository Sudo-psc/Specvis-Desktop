using UnityEngine;

[CreateAssetMenu(fileName = "LandoltCLevel", menuName = "Specvis/Landolt C Level Info", order = 2)]
public class LandoltCLevelInfo : ScriptableObject
{
    [Tooltip("e.g., '20/200', '6/60'")]
    public string acuityValueSnellen;

    [Tooltip("LogMAR equivalent, e.g., 1.0 for 20/200, 0.0 for 20/20")]
    public float logMARValue;

    [Tooltip("Overall angular size of the Landolt C character in arcminutes")]
    public float angularSizeMinutes;

    [Tooltip("Angular size of the gap in arcminutes (should be angularSizeMinutes / 5)")]
    public float gapSizeMinutes;

    void OnValidate()
    {
        // Ensure gap size is always 1/5th of the overall size
        if (angularSizeMinutes > 0)
        {
            gapSizeMinutes = angularSizeMinutes / 5f;
        }
    }
}
