package com.specvis.core.datastructures;

// Removed: import javafx.scene.shape.Shape;

public class ProcedureBasicStimulus {

    private int index;
    private double positionOnTheScreenInPixelsX;
    private double positionOnTheScreenInPixelsY;
    private double distanceFromFixPointOnTheFieldOfViewInDegreesX;
    private double distanceFromFixPointOnTheFieldOfViewInDegreesY;
    // Removed: private Shape shape;
    private int displayTime; // in milliseconds
    private int[] answers; // e.g., history of seen/not seen, or response times - This might be legacy, replaced by responseHistory
    private int brightnessThreshold; // Could be an abstract unit or specific scale (final determined threshold)
    private double luminanceThreshold; // cd/m^2 (final determined threshold)
    private double decibelThreshold;   // dB (final determined threshold)

    // New fields for UI-agnostic representation
    private String stimulusType; // e.g., "Ellipse", "Polygon", "Custom"
    private double stimulusSizeDegX; // Width in visual degrees
    private double stimulusSizeDegY; // Height in visual degrees (can be same as X for circle/square)
    private double stimulusInclinationDeg; // Rotation in degrees, relevant for Polygon or oriented Ellipse
    private double currentBrightnessValue; // The current brightness/luminance/decibel value being presented
    private boolean isActive; // Is this stimulus still part of the active test set?
    private int presentationCount;
    private java.util.List<Double> reversalLevels; // Brightness levels at which reversals occurred
    private java.util.List<Boolean> responseHistory; // History of perceived (true) / not perceived (false) responses
    private boolean lastAdjustmentWasUp; // Tracks the direction of the last brightness adjustment for staircase logic

    public ProcedureBasicStimulus() {
        this.isActive = true; // Default to active
        this.presentationCount = 0;
        this.reversalLevels = new java.util.ArrayList<>();
        this.responseHistory = new java.util.ArrayList<>();
        // lastAdjustmentWasUp typically initialized before first adjustment or based on initial trend
    }

    // Getters and Setters
    public int getIndex() { return index; }
    public void setIndex(int index) { this.index = index; }

    public double getPositionOnTheScreenInPixelsX() { return positionOnTheScreenInPixelsX; }
    public void setPositionOnTheScreenInPixelsX(double positionOnTheScreenInPixelsX) { this.positionOnTheScreenInPixelsX = positionOnTheScreenInPixelsX; }

    public double getPositionOnTheScreenInPixelsY() { return positionOnTheScreenInPixelsY; }
    public void setPositionOnTheScreenInPixelsY(double positionOnTheScreenInPixelsY) { this.positionOnTheScreenInPixelsY = positionOnTheScreenInPixelsY; }

    public double getDistanceFromFixPointOnTheFieldOfViewInDegreesX() { return distanceFromFixPointOnTheFieldOfViewInDegreesX; }
    public void setDistanceFromFixPointOnTheFieldOfViewInDegreesX(double distanceFromFixPointOnTheFieldOfViewInDegreesX) { this.distanceFromFixPointOnTheFieldOfViewInDegreesX = distanceFromFixPointOnTheFieldOfViewInDegreesX; }

    public double getDistanceFromFixPointOnTheFieldOfViewInDegreesY() { return distanceFromFixPointOnTheFieldOfViewInDegreesY; }
    public void setDistanceFromFixPointOnTheFieldOfViewInDegreesY(double distanceFromFixPointOnTheFieldOfViewInDegreesY) { this.distanceFromFixPointOnTheFieldOfViewInDegreesY = distanceFromFixPointOnTheFieldOfViewInDegreesY; }

    public int getDisplayTime() { return displayTime; }
    public void setDisplayTime(int displayTime) { this.displayTime = displayTime; }

    @Deprecated // Consider using responseHistory instead for detailed tracking
    public int[] getAnswers() { return answers; }
    @Deprecated
    public void setAnswers(int[] answers) { this.answers = answers; }

    public int getBrightnessThreshold() { return brightnessThreshold; }
    public void setBrightnessThreshold(int brightnessThreshold) { this.brightnessThreshold = brightnessThreshold; }

    public double getLuminanceThreshold() { return luminanceThreshold; }
    public void setLuminanceThreshold(double luminanceThreshold) { this.luminanceThreshold = luminanceThreshold; }

    public double getDecibelThreshold() { return decibelThreshold; }
    public void setDecibelThreshold(double decibelThreshold) { this.decibelThreshold = decibelThreshold; }

    public String getStimulusType() { return stimulusType; }
    public void setStimulusType(String stimulusType) { this.stimulusType = stimulusType; }

    public double getStimulusSizeDegX() { return stimulusSizeDegX; }
    public void setStimulusSizeDegX(double stimulusSizeDegX) { this.stimulusSizeDegX = stimulusSizeDegX; }

    public double getStimulusSizeDegY() { return stimulusSizeDegY; }
    public void setStimulusSizeDegY(double stimulusSizeDegY) { this.stimulusSizeDegY = stimulusSizeDegY; }

    public double getStimulusInclinationDeg() { return stimulusInclinationDeg; }
    public void setStimulusInclinationDeg(double stimulusInclinationDeg) { this.stimulusInclinationDeg = stimulusInclinationDeg; }

    public double getCurrentBrightnessValue() { return currentBrightnessValue; }
    public void setCurrentBrightnessValue(double currentBrightnessValue) { this.currentBrightnessValue = currentBrightnessValue; }

    public boolean isActive() { return isActive; }
    public void setActive(boolean active) { isActive = active; }

    public int getPresentationCount() { return presentationCount; }
    public void setPresentationCount(int presentationCount) { this.presentationCount = presentationCount; }
    public void incrementPresentationCount() { this.presentationCount++; }

    public java.util.List<Double> getReversalLevels() { return reversalLevels; }
    public void addReversalLevel(double level) { this.reversalLevels.add(level); }

    public java.util.List<Boolean> getResponseHistory() { return responseHistory; }
    public void addResponseToHistory(boolean perceived) { this.responseHistory.add(perceived); }
    public boolean getLastResponse() {
        if (responseHistory.isEmpty()) return false; // Or throw exception, or return default
        return responseHistory.get(responseHistory.size() -1);
    }

    public boolean wasLastAdjustmentUp() { return lastAdjustmentWasUp; }
    public void setLastAdjustmentWasUp(boolean lastAdjustmentWasUp) { this.lastAdjustmentWasUp = lastAdjustmentWasUp; }
}
