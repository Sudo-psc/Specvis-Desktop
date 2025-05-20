using System;
using System.Collections.Generic;
using UnityEngine; // For List

// Assuming D15CapInfo is defined elsewhere and loaded by FarnsworthD15TestManager
// if specific cap sets were to be chosen.

[Serializable]
public class D15Settings_CS
{
    public string settingsName = "Default D-15 Settings";
    public bool enableTimer = false;
    public float timeLimitSeconds = 120f; // 2 minutes example

    // For D-15, the cap set is usually fixed (Pilot + 15 test caps).
    // If variations were supported (e.g., D-15 Desaturated),
    // you might specify which set of D15CapInfo ScriptableObjects to use.
    // For now, assuming the FarnsworthD15TestManager loads a default set.
    // public string capDataSetName; // e.g., "Standard D-15", "Desaturated D-15"
}
