package com.specvis.core.logic;

// Removed: import javafx.scene.paint.Color;
// Removed: import javafx.scene.control.Alert;
// Removed: import org.specvis.StartApplication; // Will pass SpecvisData as parameter
// Removed: import org.specvis.controllers.dialogs.ViewExceptionDialog;
import com.specvis.core.datastructures.LuminanceData; // Assuming refactored
import com.specvis.core.datastructures.LuminanceScale; // Assuming refactored
import com.specvis.core.datastructures.ProcedureBasicData; // Assuming refactored
import com.specvis.core.datastructures.SpecvisData;       // Assuming refactored
import com.specvis.core.datastructures.procedures.basic.ProcedureBasicStimulus; // Assuming refactored

import org.apache.commons.math3.fitting.PolynomialCurveFitter;
import org.apache.commons.math3.fitting.WeightedObservedPoints;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.Optional;

public class Functions {

    public Functions() {
        // Constructor
    }

    // Method to be removed:
    // public String toHexCode(Color color) {
    //     return String.format("#%02X%02X%02X",
    //             (int) (color.getRed() * 255),
    //             (int) (color.getGreen() * 255),
    //             (int) (color.getBlue() * 255));
    // }


    public LuminanceScale findExistingLuminanceScaleByID(SpecvisData specvisData, String id) throws IllegalArgumentException {
        if (specvisData == null) {
            throw new IllegalArgumentException("SpecvisData cannot be null.");
        }
        Optional<LuminanceScale> result = specvisData.getListOfLuminanceScales().stream()
                .filter(luminanceScale -> luminanceScale.getLuminanceScaleID().equals(id))
                .findFirst();
        if (result.isPresent()) {
            return result.get();
        } else {
            // Removed: Alert alert = new Alert(Alert.AlertType.ERROR); ... new ViewExceptionDialog(alert, e);
            throw new IllegalArgumentException("Luminance Scale with ID: " + id + " was not found.");
        }
    }

    public List<ProcedureBasicData> getSortedDataFromFromBasicProcedure(SpecvisData specvisData, String procedureType) throws IllegalArgumentException {
         if (specvisData == null) {
            throw new IllegalArgumentException("SpecvisData cannot be null.");
        }
        List<ProcedureBasicData> data = new ArrayList<>();
        try {
            for (ProcedureBasicData procedureData : specvisData.getListOfProceduresBasicData()) {
                if (procedureData.getProcedureType().equals(procedureType)) {
                    data.add(procedureData);
                }
            }
            Collections.sort(data, Comparator.comparing(ProcedureBasicData::getProcedureDate).thenComparing(ProcedureBasicData::getProcedureTime).reversed());

        } catch (Exception e) {
            // Removed: Alert alert = new Alert(Alert.AlertType.ERROR); ... new ViewExceptionDialog(alert, e);
            // Log the error or wrap in a custom exception if specific handling is needed
            System.err.println("Error sorting procedure data: " + e.getMessage());
            e.printStackTrace(); // Or use a logging framework
            throw new RuntimeException("Error sorting procedure data for type " + procedureType, e);
        }
        return data;
    }

    public double[] calculateStimulusPositionOnTheScreenInPixels(double xPositionInDegrees, double yPositionInDegrees,
                                                                 int screenWidthInPixels, int screenHeightInPixels,
                                                                 double screenWidthInMillimeters, double viewingDistanceInMillimeters,
                                                                 double fixationPointPositionOnTheScreenXInPixels, double fixationPointPositionOnTheScreenYInPixels,
                                                                 double screenDistortionCorrectionQuadraticCoefficient, double screenDistortionCorrectionLinearCoefficient, double screenDistortionCorrectionConstantCoefficient) {

        double xPositionInMillimeters = Math.tan(Math.toRadians(xPositionInDegrees)) * viewingDistanceInMillimeters;
        double yPositionInMillimeters = Math.tan(Math.toRadians(yPositionInDegrees)) * viewingDistanceInMillimeters;

        double xPositionOnTheScreenInPixels = fixationPointPositionOnTheScreenXInPixels + (xPositionInMillimeters * (screenWidthInPixels / screenWidthInMillimeters));
        double yPositionOnTheScreenInPixels = fixationPointPositionOnTheScreenYInPixels - (yPositionInMillimeters * (screenWidthInPixels / screenWidthInMillimeters)); // Subtraction because Y is inverted in screens

        // Apply distortion correction if coefficients are non-zero
        if (screenDistortionCorrectionQuadraticCoefficient != 0 || screenDistortionCorrectionLinearCoefficient != 0 || screenDistortionCorrectionConstantCoefficient != 0) {
            double r = Math.sqrt(Math.pow(xPositionInMillimeters, 2) + Math.pow(yPositionInMillimeters, 2)); // Distance from center in mm
            double r_corrected = screenDistortionCorrectionQuadraticCoefficient * Math.pow(r, 2) +
                                 screenDistortionCorrectionLinearCoefficient * r +
                                 screenDistortionCorrectionConstantCoefficient;
            
            double corrected_x_mm = xPositionInMillimeters * (r_corrected / r); // Apply correction factor
            double corrected_y_mm = yPositionInMillimeters * (r_corrected / r);

            xPositionOnTheScreenInPixels = fixationPointPositionOnTheScreenXInPixels + (corrected_x_mm * (screenWidthInPixels / screenWidthInMillimeters));
            yPositionOnTheScreenInPixels = fixationPointPositionOnTheScreenYInPixels - (corrected_y_mm * (screenWidthInPixels / screenWidthInMillimeters));
        }


        return new double[]{xPositionOnTheScreenInPixels, yPositionOnTheScreenInPixels};
    }


    public double getLuminanceFromLuminanceScaleFit(double inputValue, LuminanceScale luminanceScale) throws IllegalArgumentException {
        if (luminanceScale == null || luminanceScale.getListOfLuminanceData().isEmpty()) {
            throw new IllegalArgumentException("Luminance scale data is missing or empty.");
        }

        final WeightedObservedPoints obs = new WeightedObservedPoints();
        for (LuminanceData dataPoint : luminanceScale.getListOfLuminanceData()) {
            obs.add(dataPoint.getInputValue(), dataPoint.getMeasuredLuminance());
        }

        final PolynomialCurveFitter fitter = PolynomialCurveFitter.create(luminanceScale.getPolynomialDegree());
        final double[] coeff = fitter.fit(obs.toList());

        double luminance = 0;
        for (int i = 0; i < coeff.length; i++) {
            luminance += coeff[i] * Math.pow(inputValue, i);
        }
        return Math.max(0, luminance); // Luminance cannot be negative
    }

    public double getInputValueFromLuminanceScaleFit(double desiredLuminance, LuminanceScale luminanceScale) throws IllegalArgumentException {
         if (luminanceScale == null || luminanceScale.getListOfLuminanceData().isEmpty()) {
            throw new IllegalArgumentException("Luminance scale data is missing or empty.");
        }

        // This is more complex as it's solving for x in y = P(x).
        // For simplicity, we can iterate and find the closest input value.
        // A more robust solution might involve a numerical solver or inverse polynomial if degree is low.

        final WeightedObservedPoints obs = new WeightedObservedPoints();
        for (LuminanceData dataPoint : luminanceScale.getListOfLuminanceData()) {
            obs.add(dataPoint.getInputValue(), dataPoint.getMeasuredLuminance());
        }
        final PolynomialCurveFitter fitter = PolynomialCurveFitter.create(luminanceScale.getPolynomialDegree());
        final double[] coeff = fitter.fit(obs.toList());

        double closestInputValue = -1;
        double minDifference = Double.MAX_VALUE;

        // Iterate through a reasonable range of input values (e.g., 0-255 for RGB, or scale's min/max)
        // This range should ideally be defined by the LuminanceScale's characteristics.
        // Assuming input values are typically in a range like 0-1 or 0-255.
        // For this example, let's check common input range for digital values (e.g. 0-255 for 8-bit color channels)
        // or 0.0-1.0 for normalized values. The original application context for input values is needed here.
        // Let's assume input values are in a range defined by the provided data points.
        double minInput = Double.MAX_VALUE;
        double maxInput = Double.MIN_VALUE;
        for(LuminanceData dp : luminanceScale.getListOfLuminanceData()){
            if(dp.getInputValue() < minInput) minInput = dp.getInputValue();
            if(dp.getInputValue() > maxInput) maxInput = dp.getInputValue();
        }
        // If range is still default, use a common one like 0-255
        if (minInput == Double.MAX_VALUE) minInput = 0;
        if (maxInput == Double.MIN_VALUE) maxInput = (minInput == 0) ? 255 : minInput * 2; // Heuristic


        for (double testInputValue = minInput; testInputValue <= maxInput; testInputValue += 0.1) { // Adjust step for precision
            double calculatedLuminance = 0;
            for (int i = 0; i < coeff.length; i++) {
                calculatedLuminance += coeff[i] * Math.pow(testInputValue, i);
            }
            calculatedLuminance = Math.max(0, calculatedLuminance);

            double difference = Math.abs(calculatedLuminance - desiredLuminance);
            if (difference < minDifference) {
                minDifference = difference;
                closestInputValue = testInputValue;
            }
        }
        
        if (closestInputValue == -1 && !luminanceScale.getListOfLuminanceData().isEmpty()){
            // fallback to the input value of the data point with closest luminance
            for(LuminanceData dataPoint : luminanceScale.getListOfLuminanceData()){
                double difference = Math.abs(dataPoint.getMeasuredLuminance() - desiredLuminance);
                if(difference < minDifference){
                    minDifference = difference;
                    closestInputValue = dataPoint.getInputValue();
                }
            }
        }

        if (closestInputValue == -1) {
            throw new RuntimeException("Could not find suitable input value for desired luminance: " + desiredLuminance);
        }
        return closestInputValue;
    }

    // Method to calculate visual angle from pixel size (inverse of part of calculateStimulusPositionOnTheScreenInPixels)
    public double calculateVisualAngleFromPixels(int sizeInPixels, int screenSizeInPixels, double screenSizeInMillimeters, double viewingDistanceInMillimeters) {
        if (viewingDistanceInMillimeters <= 0) {
            throw new IllegalArgumentException("Viewing distance must be positive.");
        }
        double sizeInMillimeters = ((double)sizeInPixels / screenSizeInPixels) * screenSizeInMillimeters;
        double visualAngleRadians = Math.atan(sizeInMillimeters / viewingDistanceInMillimeters);
        return Math.toDegrees(visualAngleRadians);
    }
}
