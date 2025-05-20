package com.specvis.core.datastructures.settings;

// Removed: import javafx.scene.paint.Color;
// Removed: import org.specvis.logic.Functions;

import java.util.UUID;

public class UISettingsScreenAndLuminanceScale {

    private String settingsId;
    private String settingsName;

    // Screen Parameters
    private int screenWidthInPixels;
    private int screenHeightInPixels;
    private double screenWidthInMillimeters;
    private double screenHeightInMillimeters;
    private double viewingDistanceInMillimeters;
    private String screenColorHex; // Background color for UI elements, if used by core logic
    private double screenDistortionCorrectionQuadraticCoefficient;
    private double screenDistortionCorrectionLinearCoefficient;
    private double screenDistortionCorrectionConstantCoefficient;

    // Luminance Scale Parameters
    private String luminanceScaleId; // Link to a LuminanceScale object
    private String luminanceOrBrightnessScaleIsUsed; // "Luminance", "Brightness"
    private double maximumLuminanceOrBrightnessOfTheScreen; // cd/m2 or abstract unit
    private double minimumLuminanceOrBrightnessOfTheScreen; // cd/m2 or abstract unit
    private String luminanceScaleUnit; // e.g., "cd/m^2", "dB"

    public UISettingsScreenAndLuminanceScale(String settingsName) {
        this.settingsId = UUID.randomUUID().toString();
        this.settingsName = settingsName;
        // Initialize with default values
        this.screenWidthInPixels = 1920;
        this.screenHeightInPixels = 1080;
        this.screenWidthInMillimeters = 500;
        this.screenHeightInMillimeters = 300;
        this.viewingDistanceInMillimeters = 570; // Approx 57cm for 1 deg = 1 cm
        this.screenColorHex = "#808080"; // Default gray
        this.luminanceOrBrightnessScaleIsUsed = "Luminance";
        this.maximumLuminanceOrBrightnessOfTheScreen = 100; // cd/m2
        this.minimumLuminanceOrBrightnessOfTheScreen = 0.1; // cd/m2
        this.luminanceScaleUnit = "cd/m^2";
    }

    // Getters and Setters
    public String getSettingsId() { return settingsId; }
    public void setSettingsId(String settingsId) { this.settingsId = settingsId; }
    public String getSettingsName() { return settingsName; }
    public void setSettingsName(String settingsName) { this.settingsName = settingsName; }
    public int getScreenWidthInPixels() { return screenWidthInPixels; }
    public void setScreenWidthInPixels(int screenWidthInPixels) { this.screenWidthInPixels = screenWidthInPixels; }
    public int getScreenHeightInPixels() { return screenHeightInPixels; }
    public void setScreenHeightInPixels(int screenHeightInPixels) { this.screenHeightInPixels = screenHeightInPixels; }
    public double getScreenWidthInMillimeters() { return screenWidthInMillimeters; }
    public void setScreenWidthInMillimeters(double screenWidthInMillimeters) { this.screenWidthInMillimeters = screenWidthInMillimeters; }
    public double getScreenHeightInMillimeters() { return screenHeightInMillimeters; }
    public void setScreenHeightInMillimeters(double screenHeightInMillimeters) { this.screenHeightInMillimeters = screenHeightInMillimeters; }
    public double getViewingDistanceInMillimeters() { return viewingDistanceInMillimeters; }
    public void setViewingDistanceInMillimeters(double viewingDistanceInMillimeters) { this.viewingDistanceInMillimeters = viewingDistanceInMillimeters; }
    public String getScreenColorHex() { return screenColorHex; }
    public void setScreenColorHex(String screenColorHex) { this.screenColorHex = screenColorHex; }
    public double getScreenDistortionCorrectionQuadraticCoefficient() { return screenDistortionCorrectionQuadraticCoefficient; }
    public void setScreenDistortionCorrectionQuadraticCoefficient(double screenDistortionCorrectionQuadraticCoefficient) { this.screenDistortionCorrectionQuadraticCoefficient = screenDistortionCorrectionQuadraticCoefficient; }
    public double getScreenDistortionCorrectionLinearCoefficient() { return screenDistortionCorrectionLinearCoefficient; }
    public void setScreenDistortionCorrectionLinearCoefficient(double screenDistortionCorrectionLinearCoefficient) { this.screenDistortionCorrectionLinearCoefficient = screenDistortionCorrectionLinearCoefficient; }
    public double getScreenDistortionCorrectionConstantCoefficient() { return screenDistortionCorrectionConstantCoefficient; }
    public void setScreenDistortionCorrectionConstantCoefficient(double screenDistortionCorrectionConstantCoefficient) { this.screenDistortionCorrectionConstantCoefficient = screenDistortionCorrectionConstantCoefficient; }
    public String getLuminanceScaleId() { return luminanceScaleId; }
    public void setLuminanceScaleId(String luminanceScaleId) { this.luminanceScaleId = luminanceScaleId; }
    public String getLuminanceOrBrightnessScaleIsUsed() { return luminanceOrBrightnessScaleIsUsed; }
    public void setLuminanceOrBrightnessScaleIsUsed(String luminanceOrBrightnessScaleIsUsed) { this.luminanceOrBrightnessScaleIsUsed = luminanceOrBrightnessScaleIsUsed; }
    public double getMaximumLuminanceOrBrightnessOfTheScreen() { return maximumLuminanceOrBrightnessOfTheScreen; }
    public void setMaximumLuminanceOrBrightnessOfTheScreen(double maximumLuminanceOrBrightnessOfTheScreen) { this.maximumLuminanceOrBrightnessOfTheScreen = maximumLuminanceOrBrightnessOfTheScreen; }
    public double getMinimumLuminanceOrBrightnessOfTheScreen() { return minimumLuminanceOrBrightnessOfTheScreen; }
    public void setMinimumLuminanceOrBrightnessOfTheScreen(double minimumLuminanceOrBrightnessOfTheScreen) { this.minimumLuminanceOrBrightnessOfTheScreen = minimumLuminanceOrBrightnessOfTheScreen; }
    public String getLuminanceScaleUnit() { return luminanceScaleUnit; }
    public void setLuminanceScaleUnit(String luminanceScaleUnit) { this.luminanceScaleUnit = luminanceScaleUnit; }
}
