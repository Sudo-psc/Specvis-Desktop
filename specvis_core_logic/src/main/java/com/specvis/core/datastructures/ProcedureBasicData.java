package com.specvis.core.datastructures;

import java.util.List;
import java.util.ArrayList;

public class ProcedureBasicData {

    private String procedureType;
    private String procedureDate;
    private String procedureTime;
    private String patientId; // Link to Patient
    private String procedureSettingsId; // Link to settings used

    private boolean fixationPointWasOn;
    private double fixationPointPositionX; // In degrees
    private double fixationPointPositionY; // In degrees
    private double brightnessLevelBackground;
    private double brightnessLevelFixPoint;

    private boolean blindSpotWasLocalized;
    private double blindSpotPositionX; // In degrees
    private double blindSpotPositionY; // In degrees
    private double blindSpotSize;      // In degrees

    private int totalNumberOfStimuli;
    private int totalNumberOfStimuliDeactivated;
    private int totalNumberOfStimuliNotDeactivated;

    private long totalProcedureDuration; // Milliseconds
    private long totalTimeProcedurePaused; // Milliseconds
    private boolean procedureWasCanceled;

    private boolean screenSettingsAreLoaded;
    private String usedScreenSettings; // Description or ID of screen settings
    private boolean luminanceScaleIsLoaded;
    private String usedLuminanceScale; // Description or ID of luminance scale

    private List<ProcedureBasicStimulus> listOfStimuli; // Holds the state of all stimuli at the end
    private double meanOfTheThresholds; // Calculated from deactivated stimuli

    public ProcedureBasicData() {
        this.listOfStimuli = new ArrayList<>();
    }

    // Getters and Setters
    public String getProcedureType() { return procedureType; }
    public void setProcedureType(String procedureType) { this.procedureType = procedureType; }

    public String getProcedureDate() { return procedureDate; }
    public void setProcedureDate(String procedureDate) { this.procedureDate = procedureDate; }

    public String getProcedureTime() { return procedureTime; }
    public void setProcedureTime(String procedureTime) { this.procedureTime = procedureTime; }

    public String getPatientId() { return patientId; }
    public void setPatientId(String patientId) { this.patientId = patientId; }

    public String getProcedureSettingsId() { return procedureSettingsId; }
    public void setProcedureSettingsId(String procedureSettingsId) { this.procedureSettingsId = procedureSettingsId; }

    public boolean isFixationPointWasOn() { return fixationPointWasOn; }
    public void setFixationPointWasOn(boolean fixationPointWasOn) { this.fixationPointWasOn = fixationPointWasOn; }

    public double getFixationPointPositionX() { return fixationPointPositionX; }
    public void setFixationPointPositionX(double fixationPointPositionX) { this.fixationPointPositionX = fixationPointPositionX; }

    public double getFixationPointPositionY() { return fixationPointPositionY; }
    public void setFixationPointPositionY(double fixationPointPositionY) { this.fixationPointPositionY = fixationPointPositionY; }

    public double getBrightnessLevelBackground() { return brightnessLevelBackground; }
    public void setBrightnessLevelBackground(double brightnessLevelBackground) { this.brightnessLevelBackground = brightnessLevelBackground; }

    public double getBrightnessLevelFixPoint() { return brightnessLevelFixPoint; }
    public void setBrightnessLevelFixPoint(double brightnessLevelFixPoint) { this.brightnessLevelFixPoint = brightnessLevelFixPoint; }

    public boolean isBlindSpotWasLocalized() { return blindSpotWasLocalized; }
    public void setBlindSpotWasLocalized(boolean blindSpotWasLocalized) { this.blindSpotWasLocalized = blindSpotWasLocalized; }

    public double getBlindSpotPositionX() { return blindSpotPositionX; }
    public void setBlindSpotPositionX(double blindSpotPositionX) { this.blindSpotPositionX = blindSpotPositionX; }

    public double getBlindSpotPositionY() { return blindSpotPositionY; }
    public void setBlindSpotPositionY(double blindSpotPositionY) { this.blindSpotPositionY = blindSpotPositionY; }

    public double getBlindSpotSize() { return blindSpotSize; }
    public void setBlindSpotSize(double blindSpotSize) { this.blindSpotSize = blindSpotSize; }

    public int getTotalNumberOfStimuli() { return totalNumberOfStimuli; }
    public void setTotalNumberOfStimuli(int totalNumberOfStimuli) { this.totalNumberOfStimuli = totalNumberOfStimuli; }

    public int getTotalNumberOfStimuliDeactivated() { return totalNumberOfStimuliDeactivated; }
    public void setTotalNumberOfStimuliDeactivated(int totalNumberOfStimuliDeactivated) { this.totalNumberOfStimuliDeactivated = totalNumberOfStimuliDeactivated; }

    public int getTotalNumberOfStimuliNotDeactivated() { return totalNumberOfStimuliNotDeactivated; }
    public void setTotalNumberOfStimuliNotDeactivated(int totalNumberOfStimuliNotDeactivated) { this.totalNumberOfStimuliNotDeactivated = totalNumberOfStimuliNotDeactivated; }

    public long getTotalProcedureDuration() { return totalProcedureDuration; }
    public void setTotalProcedureDuration(long totalProcedureDuration) { this.totalProcedureDuration = totalProcedureDuration; }

    public long getTotalTimeProcedurePaused() { return totalTimeProcedurePaused; }
    public void setTotalTimeProcedurePaused(long totalTimeProcedurePaused) { this.totalTimeProcedurePaused = totalTimeProcedurePaused; }

    public boolean isProcedureWasCanceled() { return procedureWasCanceled; }
    public void setProcedureWasCanceled(boolean procedureWasCanceled) { this.procedureWasCanceled = procedureWasCanceled; }

    public boolean isScreenSettingsAreLoaded() { return screenSettingsAreLoaded; }
    public void setScreenSettingsAreLoaded(boolean screenSettingsAreLoaded) { this.screenSettingsAreLoaded = screenSettingsAreLoaded; }

    public String getUsedScreenSettings() { return usedScreenSettings; }
    public void setUsedScreenSettings(String usedScreenSettings) { this.usedScreenSettings = usedScreenSettings; }

    public boolean isLuminanceScaleIsLoaded() { return luminanceScaleIsLoaded; }
    public void setLuminanceScaleIsLoaded(boolean luminanceScaleIsLoaded) { this.luminanceScaleIsLoaded = luminanceScaleIsLoaded; }

    public String getUsedLuminanceScale() { return usedLuminanceScale; }
    public void setUsedLuminanceScale(String usedLuminanceScale) { this.usedLuminanceScale = usedLuminanceScale; }

    public List<ProcedureBasicStimulus> getListOfStimuli() { return listOfStimuli; }
    public void setListOfStimuli(List<ProcedureBasicStimulus> listOfStimuli) { this.listOfStimuli = listOfStimuli; }

    public double getMeanOfTheThresholds() { return meanOfTheThresholds; }
    public void setMeanOfTheThresholds(double meanOfTheThresholds) { this.meanOfTheThresholds = meanOfTheThresholds; }
}
