package com.specvis.core.datastructures;

public class LuminanceData {

    private double inputValue; // e.g., 0-255 for RGB, or abstract value
    private double measuredLuminance; // cd/m^2

    public LuminanceData(double inputValue, double measuredLuminance) {
        this.inputValue = inputValue;
        this.measuredLuminance = measuredLuminance;
    }

    // Getters and Setters
    public double getInputValue() {
        return inputValue;
    }

    public void setInputValue(double inputValue) {
        this.inputValue = inputValue;
    }

    public double getMeasuredLuminance() {
        return measuredLuminance;
    }

    public void setMeasuredLuminance(double measuredLuminance) {
        this.measuredLuminance = measuredLuminance;
    }

    @Override
    public String toString() {
        return "LuminanceData{" +
                "inputValue=" + inputValue +
                ", measuredLuminance=" + measuredLuminance +
                '}';
    }
}
