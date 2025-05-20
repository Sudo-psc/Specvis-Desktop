package com.specvis.core.datastructures.settings;

// Removed: import javafx.scene.paint.Color;
// Removed: import org.specvis.logic.Functions; // If used, should be injected or handled differently

import java.util.UUID;

public class ProcedureBasicSettingsGeneral {

    private String settingsId;
    private String settingsName;
    private String procedureType; // E.g., "BASIC_FIX_MONITOR_NONE"

    // Stimulus Parameters
    private String stimulusShape; // "Ellipse", "Polygon"
    private double stimulusSizeInVisualDegrees;
    private int stimulusDisplayTime; // ms
    private double stimulusInclination; // degrees, for Polygon
    // Removed: private Color stimulusColor; // Store as components or string
    private String stimulusColorHex; // Example: "#FF0000" for red
    private double brightnessLevelStimulus; // Initial brightness
    private double minimumBrightnessLevelStimulus;
    private double maximumBrightnessLevelStimulus;

    // Fixation Point Parameters
    private boolean fixationPointIsOn;
    // Removed: private Color fixationPointColor;
    private String fixationPointColorHex;
    private double fixationPointPositionX; // degrees or pixels, define consistently
    private double fixationPointPositionY; // degrees or pixels
    private double brightnessLevelFixPoint;

    // Background Parameters
    // Removed: private Color backgroundColor;
    private String backgroundColorHex;
    private double brightnessLevelBackground;

    // Blind Spot Parameters (if applicable to this general settings)
    private boolean blindSpotIsOn;
    // Removed: private Color blindSpotColor;
    private String blindSpotColorHex;
    private double brightnessLevelBlindSpot;

    // Monitor Parameters (if applicable)
    private double brightnessLevelMonitor;

    // Procedure Control Parameters
    private int totalNumberOfStimuliToDisplay; // Might be derived from stimuliPositions
    private String stimuliPositions; // Semicolon-separated list of "x,y" in degrees, e.g., "5,0;-5,0;0,5;0,-5"
    private int maximumNumberOfPresentationsPerStimulus;
    private int numberOfReversalsToDeactivateStimulus;
    private String staircaseAlgorithmType; // e.g., "BASIC_UP_DOWN", "TRANSFORMED_UP_DOWN"
    private double staircaseStepSizeUp;
    private double staircaseStepSizeDown;
    private int staircaseRuleUp;   // e.g., number of "no" responses to go up
    private int staircaseRuleDown; // e.g., number of "yes" responses to go down

    private boolean bootTimeIsRandom;
    private long bootTime; // ms
    private long bootTimeMinimum; // ms
    private long bootTimeMaximum; // ms
    private long maximumResponseTime; // ms

    public ProcedureBasicSettingsGeneral(String settingsName, String procedureType) {
        this.settingsId = UUID.randomUUID().toString();
        this.settingsName = settingsName;
        this.procedureType = procedureType;
        // Initialize with default values
        this.stimulusShape = "Ellipse";
        this.stimulusSizeInVisualDegrees = 1.0;
        this.stimulusDisplayTime = 200;
        this.stimulusColorHex = "#FFFFFF"; // White
        this.brightnessLevelStimulus = 0.5; // Normalized or abstract unit
        this.minimumBrightnessLevelStimulus = 0.0;
        this.maximumBrightnessLevelStimulus = 1.0;

        this.fixationPointIsOn = true;
        this.fixationPointColorHex = "#FFFFFF"; // White
        this.fixationPointPositionX = 0.0;
        this.fixationPointPositionY = 0.0;
        this.brightnessLevelFixPoint = 0.7;

        this.backgroundColorHex = "#000000"; // Black
        this.brightnessLevelBackground = 0.1;
        
        this.totalNumberOfStimuliToDisplay = 4; // Default, should match stimuliPositions if used
        this.stimuliPositions = "5,0;-5,0;0,5;0,-5"; // Default 4 positions
        this.maximumNumberOfPresentationsPerStimulus = 10;
        this.numberOfReversalsToDeactivateStimulus = 2;
        this.staircaseStepSizeUp = 0.1;
        this.staircaseStepSizeDown = 0.1;
        this.bootTime = 1000;
        this.maximumResponseTime = 2000;
    }

    // Getters and Setters for all fields
    public String getSettingsId() { return settingsId; }
    public void setSettingsId(String settingsId) { this.settingsId = settingsId; }
    public String getSettingsName() { return settingsName; }
    public void setSettingsName(String settingsName) { this.settingsName = settingsName; }
    public String getProcedureType() { return procedureType; }
    public void setProcedureType(String procedureType) { this.procedureType = procedureType; }
    public String getStimulusShape() { return stimulusShape; }
    public void setStimulusShape(String stimulusShape) { this.stimulusShape = stimulusShape; }
    public double getStimulusSizeInVisualDegrees() { return stimulusSizeInVisualDegrees; }
    public void setStimulusSizeInVisualDegrees(double stimulusSizeInVisualDegrees) { this.stimulusSizeInVisualDegrees = stimulusSizeInVisualDegrees; }
    public int getStimulusDisplayTime() { return stimulusDisplayTime; }
    public void setStimulusDisplayTime(int stimulusDisplayTime) { this.stimulusDisplayTime = stimulusDisplayTime; }
    public double getStimulusInclination() { return stimulusInclination; }
    public void setStimulusInclination(double stimulusInclination) { this.stimulusInclination = stimulusInclination; }
    public String getStimulusColorHex() { return stimulusColorHex; }
    public void setStimulusColorHex(String stimulusColorHex) { this.stimulusColorHex = stimulusColorHex; }
    public double getBrightnessLevelStimulus() { return brightnessLevelStimulus; }
    public void setBrightnessLevelStimulus(double brightnessLevelStimulus) { this.brightnessLevelStimulus = brightnessLevelStimulus; }
    public double getMinimumBrightnessLevelStimulus() { return minimumBrightnessLevelStimulus; }
    public void setMinimumBrightnessLevelStimulus(double minimumBrightnessLevelStimulus) { this.minimumBrightnessLevelStimulus = minimumBrightnessLevelStimulus; }
    public double getMaximumBrightnessLevelStimulus() { return maximumBrightnessLevelStimulus; }
    public void setMaximumBrightnessLevelStimulus(double maximumBrightnessLevelStimulus) { this.maximumBrightnessLevelStimulus = maximumBrightnessLevelStimulus; }
    public boolean isFixationPointIsOn() { return fixationPointIsOn; }
    public void setFixationPointIsOn(boolean fixationPointIsOn) { this.fixationPointIsOn = fixationPointIsOn; }
    public String getFixationPointColorHex() { return fixationPointColorHex; }
    public void setFixationPointColorHex(String fixationPointColorHex) { this.fixationPointColorHex = fixationPointColorHex; }
    public double getFixationPointPositionX() { return fixationPointPositionX; }
    public void setFixationPointPositionX(double fixationPointPositionX) { this.fixationPointPositionX = fixationPointPositionX; }
    public double getFixationPointPositionY() { return fixationPointPositionY; }
    public void setFixationPointPositionY(double fixationPointPositionY) { this.fixationPointPositionY = fixationPointPositionY; }
    public double getBrightnessLevelFixPoint() { return brightnessLevelFixPoint; }
    public void setBrightnessLevelFixPoint(double brightnessLevelFixPoint) { this.brightnessLevelFixPoint = brightnessLevelFixPoint; }
    public String getBackgroundColorHex() { return backgroundColorHex; }
    public void setBackgroundColorHex(String backgroundColorHex) { this.backgroundColorHex = backgroundColorHex; }
    public double getBrightnessLevelBackground() { return brightnessLevelBackground; }
    public void setBrightnessLevelBackground(double brightnessLevelBackground) { this.brightnessLevelBackground = brightnessLevelBackground; }
    public boolean isBlindSpotIsOn() { return blindSpotIsOn; }
    public void setBlindSpotIsOn(boolean blindSpotIsOn) { this.blindSpotIsOn = blindSpotIsOn; }
    public String getBlindSpotColorHex() { return blindSpotColorHex; }
    public void setBlindSpotColorHex(String blindSpotColorHex) { this.blindSpotColorHex = blindSpotColorHex; }
    public double getBrightnessLevelBlindSpot() { return brightnessLevelBlindSpot; }
    public void setBrightnessLevelBlindSpot(double brightnessLevelBlindSpot) { this.brightnessLevelBlindSpot = brightnessLevelBlindSpot; }
    public double getBrightnessLevelMonitor() { return brightnessLevelMonitor; }
    public void setBrightnessLevelMonitor(double brightnessLevelMonitor) { this.brightnessLevelMonitor = brightnessLevelMonitor; }
    public int getTotalNumberOfStimuliToDisplay() { return totalNumberOfStimuliToDisplay; }
    public void setTotalNumberOfStimuliToDisplay(int totalNumberOfStimuliToDisplay) { this.totalNumberOfStimuliToDisplay = totalNumberOfStimuliToDisplay; }
    public String getStimuliPositions() { return stimuliPositions; }
    public void setStimuliPositions(String stimuliPositions) { this.stimuliPositions = stimuliPositions; }
    public int getMaximumNumberOfPresentationsPerStimulus() { return maximumNumberOfPresentationsPerStimulus; }
    public void setMaximumNumberOfPresentationsPerStimulus(int maximumNumberOfPresentationsPerStimulus) { this.maximumNumberOfPresentationsPerStimulus = maximumNumberOfPresentationsPerStimulus; }
    public int getNumberOfReversalsToDeactivateStimulus() { return numberOfReversalsToDeactivateStimulus; }
    public void setNumberOfReversalsToDeactivateStimulus(int numberOfReversalsToDeactivateStimulus) { this.numberOfReversalsToDeactivateStimulus = numberOfReversalsToDeactivateStimulus; }
    public String getStaircaseAlgorithmType() { return staircaseAlgorithmType; }
    public void setStaircaseAlgorithmType(String staircaseAlgorithmType) { this.staircaseAlgorithmType = staircaseAlgorithmType; }
    public double getStaircaseStepSizeUp() { return staircaseStepSizeUp; }
    public void setStaircaseStepSizeUp(double staircaseStepSizeUp) { this.staircaseStepSizeUp = staircaseStepSizeUp; }
    public double getStaircaseStepSizeDown() { return staircaseStepSizeDown; }
    public void setStaircaseStepSizeDown(double staircaseStepSizeDown) { this.staircaseStepSizeDown = staircaseStepSizeDown; }
    public int getStaircaseRuleUp() { return staircaseRuleUp; }
    public void setStaircaseRuleUp(int staircaseRuleUp) { this.staircaseRuleUp = staircaseRuleUp; }
    public int getStaircaseRuleDown() { return staircaseRuleDown; }
    public void setStaircaseRuleDown(int staircaseRuleDown) { this.staircaseRuleDown = staircaseRuleDown; }
    public boolean isBootTimeIsRandom() { return bootTimeIsRandom; }
    public void setBootTimeIsRandom(boolean bootTimeIsRandom) { this.bootTimeIsRandom = bootTimeIsRandom; }
    public long getBootTime() { return bootTime; }
    public void setBootTime(long bootTime) { this.bootTime = bootTime; }
    public long getBootTimeMinimum() { return bootTimeMinimum; }
    public void setBootTimeMinimum(long bootTimeMinimum) { this.bootTimeMinimum = bootTimeMinimum; }
    public long getBootTimeMaximum() { return bootTimeMaximum; }
    public void setBootTimeMaximum(long bootTimeMaximum) { this.bootTimeMaximum = bootTimeMaximum; }
    public long getMaximumResponseTime() { return maximumResponseTime; }
    public void setMaximumResponseTime(long maximumResponseTime) { this.maximumResponseTime = maximumResponseTime; }
}
