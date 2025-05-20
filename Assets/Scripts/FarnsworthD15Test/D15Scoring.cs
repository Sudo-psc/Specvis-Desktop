using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class D15Scoring
{
    // Correct order of cap numbers (1-15) after the pilot (cap 0)
    private static readonly List<int> CorrectSequence = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

    public static void CalculateScore(FarnsworthD15TestResult result, List<D15CapInfo> allCapInfos)
    {
        if (result == null || result.userCapOrder == null || result.userCapOrder.Count != 15)
        {
            Debug.LogError("D15Scoring: Invalid result data provided.");
            result.errorScore = -1; // Indicate error
            result.confusionAxis = D15ConfusionAxis.Unclear;
            result.severity = D15Severity.Undetermined;
            return;
        }

        // 1. Calculate Total Error Score (TES)
        // The error score is the sum of the absolute differences between the numbers of adjacent caps
        // in the patient's arrangement, plus the differences between the pilot cap (0) and the first cap,
        // and the last cap and the pilot cap (0) again to form a circle.
        // However, standard D-15 scoring often refers to the sum of differences from the *correct successor*.
        // Let's use a common simplified method: sum of absolute differences to correct *value* for that position.
        // A more direct way: sum of differences in *order value* between adjacent caps.
        // The traditional method is simpler: Sum of | UserOrder[i].CorrectOrder - UserOrder[i-1].CorrectOrder | - 2*N_caps
        // For simplicity here, we'll use a method based on Vingrys and King-Smith (1988) Total Error Score (TES)
        // which is based on comparing each cap to its ideal predecessor and successor.

        // First, map cap numbers to their D15CapInfo objects for easy lookup of `correctOrder`
        Dictionary<int, D15CapInfo> capInfoMap = allCapInfos.ToDictionary(ci => ci.capNumber, ci => ci);
        D15CapInfo pilotCap = capInfoMap.ContainsKey(0) ? capInfoMap[0] : null;

        if (pilotCap == null) {
            Debug.LogError("Pilot cap info not found for scoring.");
            result.errorScore = -1;
            return;
        }

        int tes = 0;
        List<D15CapInfo> userArrangementWithPilot = new List<D15CapInfo>();
        userArrangementWithPilot.Add(pilotCap); // Start with pilot
        foreach (int capNum in result.userCapOrder) {
            if (capInfoMap.TryGetValue(capNum, out D15CapInfo cap)) {
                userArrangementWithPilot.Add(cap);
            } else {
                 Debug.LogWarning($"Cap number {capNum} in user order not found in allCapInfos. Scoring might be inaccurate.");
                 // Add a dummy cap or skip? For now, let's make it problematic for score.
                 tes += 30; // Penalize heavily for missing cap data
            }
        }
        userArrangementWithPilot.Add(pilotCap); // End with pilot to form a circle

        for (int i = 1; i < userArrangementWithPilot.Count -1; i++) // Iterate through placed caps (excluding end pilot)
        {
            D15CapInfo currentCap = userArrangementWithPilot[i];
            D15CapInfo prevCap = userArrangementWithPilot[i-1];
            
            // Difference to ideal successor (cap number + 1, or 1 if current is 15)
            int idealSuccessorOrder = (currentCap.correctOrder % 15) + 1; 
            
            // Find the cap the user actually placed next
            D15CapInfo actualSuccessorCap = userArrangementWithPilot[i+1];

            // Calculate difference in `correctOrder` values
            // This is one way; another is direct comparison of cap numbers if they are sequential
            tes += Mathf.Abs(currentCap.correctOrder - prevCap.correctOrder);
            if (i == userArrangementWithPilot.Count - 2) { // last placed cap compared to pilot
                 tes += Mathf.Abs(currentCap.correctOrder - actualSuccessorCap.correctOrder); // actualSuccessor is pilot here
            }
        }
        // The above TES is a sum of adjacent differences.
        // A simpler error score for D15 is often just counting how many caps are out of place
        // compared to the perfect sequence.
        // For this implementation, let's use a more standard crossover-based analysis.

        // Identifying Crossovers for Axis Diagnosis (Simplified)
        // A crossover occurs if the line connecting cap A-B intersects C-D on a D15 plot.
        // This means the order of hue has been significantly mixed.
        // We look for pairs where userOrder[i].correctOrder > userOrder[i+1].correctOrder by a large margin
        // or specific "confusion links".

        result.crossoverPairs.Clear();
        // Link each cap in user's order to the next, forming a circle (cap N links to cap 1)
        for (int i = 0; i < result.userCapOrder.Count; i++)
        {
            int cap1Num = result.userCapOrder[i];
            int cap2Num = result.userCapOrder[(i + 1) % result.userCapOrder.Count]; // Next cap, wraps around

            if (!capInfoMap.ContainsKey(cap1Num) || !capInfoMap.ContainsKey(cap2Num)) continue;

            D15CapInfo cap1Info = capInfoMap[cap1Num];
            D15CapInfo cap2Info = capInfoMap[cap2Num];

            // Check for specific confusion lines indicative of Protan, Deutan, Tritan
            // This requires a predefined list of "confusion links" based on cap correctOrder values.
            // Example: Cap 7 (GY) and Cap 8 (Y) are adjacent. If user places Cap 15 (RP) between them, that's a major error.

            // Simplified Crossover Detection:
            // If connecting cap A (order X) to cap B (order Y) "crosses over" the pilot (order 0)
            // or creates a large jump in hue angle.
            // Example: If cap_i.correctOrder is far from cap_{i+1}.correctOrder
            // This is a very simplified error score based on adjacent differences.
            // A true D15 score is more nuanced.
            int diff = Mathf.Abs(cap1Info.correctOrder - cap2Info.correctOrder);
            if (diff > 2) { // Arbitrary threshold for a 'significant' jump indicating a potential error/crossover area
                tes += diff; // Add to error score
                result.crossoverPairs.Add(new Vector2Int(cap1Info.capNumber, cap2Info.capNumber));
            } else {
                 tes += diff; // Smaller difference still contributes
            }
        }
        // This TES is just one way to calculate. A common method is to sum the two smallest errors for each cap.
        // For now, this 'tes' is a sum of absolute differences in correctOrder between adjacent user-placed caps.
        result.errorScore = tes;


        // Determine Confusion Axis (Highly Simplified based on known D15 patterns)
        // This requires a lookup table or more complex pattern analysis of crossovers.
        // Example patterns:
        // Protan: Crossovers parallel to caps 15-7, 1-8 (approx.)
        // Deutan: Crossovers parallel to caps 1-9, 2-10 (approx.)
        // Tritan: Crossovers parallel to caps 3-11, 4-12 (approx.)

        // A very basic heuristic for demonstration:
        int protanErrors = 0; int deutanErrors = 0; int tritanErrors = 0;
        foreach(var pair in result.crossoverPairs) {
            int c1Order = capInfoMap[pair.x].correctOrder;
            int c2Order = capInfoMap[pair.y].correctOrder;

            // Protan-like confusions (e.g., confusing deep reds/purples with blue-greens)
            if ((IsInRange(c1Order, 13, 15) || IsInRange(c1Order, 1, 2)) && IsInRange(c2Order, 6, 9)) protanErrors++;
            if ((IsInRange(c2Order, 13, 15) || IsInRange(c2Order, 1, 2)) && IsInRange(c1Order, 6, 9)) protanErrors++;

            // Deutan-like confusions (e.g., confusing greens with purples/reds)
            if (IsInRange(c1Order, 1, 3) && IsInRange(c2Order, 8, 11)) deutanErrors++;
            if (IsInRange(c2Order, 1, 3) && IsInRange(c1Order, 8, 11)) deutanErrors++;
            
            // Tritan-like confusions (e.g., confusing blues with yellow-greens)
            if (IsInRange(c1Order, 3, 5) && IsInRange(c2Order, 10, 13)) tritanErrors++;
            if (IsInRange(c2Order, 3, 5) && IsInRange(c1Order, 10, 13)) tritanErrors++;
        }

        if (protanErrors > deutanErrors && protanErrors > tritanErrors && protanErrors > 1) result.confusionAxis = D15ConfusionAxis.Protan;
        else if (deutanErrors > protanErrors && deutanErrors > tritanErrors && deutanErrors > 1) result.confusionAxis = D15ConfusionAxis.Deutan;
        else if (tritanErrors > protanErrors && tritanErrors > deutanErrors && tritanErrors > 1) result.confusionAxis = D15ConfusionAxis.Tritan;
        else if (result.errorScore > 10) result.confusionAxis = D15ConfusionAxis.Unclear; // Arbitrary: if error score is high but no clear axis
        else result.confusionAxis = D15ConfusionAxis.None;


        // Determine Severity (Highly Simplified)
        if (result.confusionAxis == D15ConfusionAxis.None)
        {
            if (result.errorScore <= 2) result.severity = D15Severity.None; // Typical for normal
            else if (result.errorScore <=10) result.severity = D15Severity.Mild; // Minor errors
            else result.severity = D15Severity.Moderate; // More significant errors but no clear axis
        }
        else // If there's a confusion axis
        {
            if (result.errorScore <= 15) result.severity = D15Severity.Mild;
            else if (result.errorScore <= 30) result.severity = D15Severity.Moderate;
            else result.severity = D15Severity.Strong;
        }
        if (result.userCapOrder.Any(capNum => capNum == -1)) { // Check for any empty slots
            result.severity = D15Severity.Undetermined;
            result.confusionAxis = D15ConfusionAxis.Unclear;
            if (result.errorScore < 0) result.errorScore = 99; // Ensure error score reflects issue
            else result.errorScore += 50; // Penalize for incomplete test
        }
    }

    private static bool IsInRange(int number, int min, int max) {
        return number >= min && number <= max;
    }

    // --- Input for scoring ---
    // result.userCapOrder: List<int> of cap numbers (1-15) as ordered by the user.
    // allCapInfos: List<D15CapInfo> to map cap numbers back to their correctOrder.

    // --- Outputs from scoring (to be populated in result object) ---
    // result.errorScore: An integer representing the magnitude of errors.
    // result.confusionAxis: Enum (Protan, Deutan, Tritan, None, Unclear).
    // result.severity: Enum (Mild, Moderate, Strong, None, Undetermined).
    // result.crossoverPairs: List of Vector2Int, where each Vector2Int is (capNumber1, capNumber2)
    //                        representing a connection in the user's sequence that forms part of a crossover.
    //                        This is used for plotting and detailed analysis.

    // --- Basic Logic Flow for D15Scoring.CalculateScore() ---
    // 1. Validate input: Ensure `userCapOrder` is not null and has 15 caps.
    // 2. Create a temporary list representing the full circle of caps: Pilot + userCapOrder + Pilot.
    // 3. Calculate the "Total Error Score" (TES) or a similar error metric.
    //    - A common method involves summing the differences between the `correctOrder` of adjacent caps in the user's sequence.
    //    - Example: For user sequence A-B-C, errors could be |A.correctOrder - Pilot.correctOrder| + |B.correctOrder - A.correctOrder| + |C.correctOrder - B.correctOrder| + |Pilot.correctOrder - C.correctOrder|.
    //    - Simpler: Sum of |UserOrder[i].correctOrder - CorrectSequence[i]| - but this doesn't capture hue circle.
    //    - Vingrys and King-Smith (1988) method is more robust.
    // 4. Identify Crossover Pairs:
    //    - Iterate through the user's sequence (A-B-C...).
    //    - A crossover occurs when the line connecting cap X to cap Y (in the user's sequence) on a standard D15 diagram crosses over another line segment Z-W from the user's sequence, or crosses over a "confusion line" for a specific color deficiency type.
    //    - This is often determined by comparing the `correctOrder` values. For instance, if cap A is correctly #3 and cap B is correctly #10, but the user places them A-B, they've skipped many intermediate hues.
    //    - Store these pairs (e.g., `new Vector2Int(capX.capNumber, capY.capNumber)`).
    // 5. Determine Confusion Axis:
    //    - Analyze the identified crossover pairs.
    //    - Protan axis confusions typically occur along a specific direction (e.g., caps 15-7, 1-8).
    //    - Deutan axis confusions along another (e.g., caps 1-9, 2-10).
    //    - Tritan axis confusions along a third (e.g., caps 3-11, 4-12).
    //    - This usually involves comparing the `correctOrder` of the caps in each crossover pair to predefined confusion lines or angles on the D15 diagram.
    //    - If crossovers align predominantly with one axis, that's the result. If mixed or none, it's "Unclear" or "None".
    // 6. Determine Severity:
    //    - Based on the `errorScore` and the presence/type of `confusionAxis`.
    //    - Higher error scores generally mean stronger severity.
    //    - Specific scoring tables or criteria (e.g., from Bowman) relate error scores and crossover types to severity.
    // 7. Populate the `FarnsworthD15TestResult` object with all calculated values.
}
