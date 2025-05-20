package com.specvis.core.procedures;

import com.specvis.core.datastructures.ProcedureBasicStimulus;
import com.specvis.core.datastructures.ProcedureBasicData; // Assuming this will be refactored

public interface ProcedureCallback {
    void onStimulusReadyToPresent(ProcedureBasicStimulus stimulusData);
    void onStimulusHidden();
    void onProgressUpdate(double progress, int stimuliCompleted, int totalStimuli);
    void onTestFinished(ProcedureBasicData results); // Or a more generic result type
    void onMessage(String message, MessageType type); // e.g., INFO, WARNING, ERROR
    void onFixationPointUpdate(boolean visible, double x, double y); // x,y in degrees or pixels as needed

    enum MessageType {
        INFO,
        WARNING,
        ERROR
    }
}
