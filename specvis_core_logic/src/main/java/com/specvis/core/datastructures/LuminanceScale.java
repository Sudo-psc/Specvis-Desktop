package com.specvis.core.datastructures;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class LuminanceScale {

    private String luminanceScaleID;
    private String luminanceScaleName;
    private String luminanceScaleUnit; // "cd/m^2", "dB", etc.
    private int polynomialDegree;
    private List<LuminanceData> listOfLuminanceData;

    public LuminanceScale(String luminanceScaleName, String luminanceScaleUnit, int polynomialDegree) {
        this.luminanceScaleID = UUID.randomUUID().toString();
        this.luminanceScaleName = luminanceScaleName;
        this.luminanceScaleUnit = luminanceScaleUnit;
        this.polynomialDegree = polynomialDegree;
        this.listOfLuminanceData = new ArrayList<>();
    }

    // Getters and Setters
    public String getLuminanceScaleID() {
        return luminanceScaleID;
    }

    public void setLuminanceScaleID(String luminanceScaleID) {
        this.luminanceScaleID = luminanceScaleID;
    }

    public String getLuminanceScaleName() {
        return luminanceScaleName;
    }

    public void setLuminanceScaleName(String luminanceScaleName) {
        this.luminanceScaleName = luminanceScaleName;
    }

    public String getLuminanceScaleUnit() {
        return luminanceScaleUnit;
    }

    public void setLuminanceScaleUnit(String luminanceScaleUnit) {
        this.luminanceScaleUnit = luminanceScaleUnit;
    }

    public int getPolynomialDegree() {
        return polynomialDegree;
    }

    public void setPolynomialDegree(int polynomialDegree) {
        this.polynomialDegree = polynomialDegree;
    }

    public List<LuminanceData> getListOfLuminanceData() {
        return listOfLuminanceData;
    }

    public void setListOfLuminanceData(List<LuminanceData> listOfLuminanceData) {
        this.listOfLuminanceData = listOfLuminanceData;
    }

    public void addLuminanceDataPoint(LuminanceData dataPoint) {
        this.listOfLuminanceData.add(dataPoint);
    }

    public void addLuminanceDataPoint(double inputValue, double measuredLuminance) {
        this.listOfLuminanceData.add(new LuminanceData(inputValue, measuredLuminance));
    }

    @Override
    public String toString() {
        return "LuminanceScale{" +
                "luminanceScaleName='" + luminanceScaleName + '\'' +
                ", unit='" + luminanceScaleUnit + '\'' +
                ", degree=" + polynomialDegree +
                ", dataPoints=" + listOfLuminanceData.size() +
                '}';
    }
}
