package com.specvis.core.datastructures;

import com.specvis.core.datastructures.settings.ProcedureBasicSettingsGeneral;
import com.specvis.core.datastructures.settings.UISettingsScreenAndLuminanceScale;
import com.specvis.core.procedures.ProcedureBasicFixMonitorNone; // Example, may not be stored directly

import java.util.ArrayList;
import java.util.List;

// Assuming this class was mostly a data container.
// Ensure no JavaFX specific fields or methods.
public class SpecvisData {

    private List<Patient> listOfPatients;
    private List<ProcedureBasicData> listOfProceduresBasicData;
    private List<LuminanceScale> listOfLuminanceScales;
    private List<ProcedureBasicSettingsGeneral> listOfProcedureBasicSettingsGeneral;
    private UISettingsScreenAndLuminanceScale uiSettingsScreenAndLuminanceScale;
    // other global settings or data lists

    public SpecvisData() {
        this.listOfPatients = new ArrayList<>();
        this.listOfProceduresBasicData = new ArrayList<>();
        this.listOfLuminanceScales = new ArrayList<>();
        this.listOfProcedureBasicSettingsGeneral = new ArrayList<>();
        // Initialize other lists and objects
    }

    // Getters and Setters
    public List<Patient> getListOfPatients() {
        return listOfPatients;
    }

    public void setListOfPatients(List<Patient> listOfPatients) {
        this.listOfPatients = listOfPatients;
    }

    public List<ProcedureBasicData> getListOfProceduresBasicData() {
        return listOfProceduresBasicData;
    }

    public void setListOfProceduresBasicData(List<ProcedureBasicData> listOfProceduresBasicData) {
        this.listOfProceduresBasicData = listOfProceduresBasicData;
    }

    public List<LuminanceScale> getListOfLuminanceScales() {
        return listOfLuminanceScales;
    }

    public void setListOfLuminanceScales(List<LuminanceScale> listOfLuminanceScales) {
        this.listOfLuminanceScales = listOfLuminanceScales;
    }

    public List<ProcedureBasicSettingsGeneral> getListOfProcedureBasicSettingsGeneral() {
        return listOfProcedureBasicSettingsGeneral;
    }

    public void setListOfProcedureBasicSettingsGeneral(List<ProcedureBasicSettingsGeneral> listOfProcedureBasicSettingsGeneral) {
        this.listOfProcedureBasicSettingsGeneral = listOfProcedureBasicSettingsGeneral;
    }

    public UISettingsScreenAndLuminanceScale getUiSettingsScreenAndLuminanceScale() {
        return uiSettingsScreenAndLuminanceScale;
    }

    public void setUiSettingsScreenAndLuminanceScale(UISettingsScreenAndLuminanceScale uiSettingsScreenAndLuminanceScale) {
        this.uiSettingsScreenAndLuminanceScale = uiSettingsScreenAndLuminanceScale;
    }

    // Example methods to manage data
    public void addPatient(Patient patient) {
        this.listOfPatients.add(patient);
    }

    public void addProcedureBasicData(ProcedureBasicData data) {
        this.listOfProceduresBasicData.add(data);
    }
    
    public void addLuminanceScale(LuminanceScale scale) {
        this.listOfLuminanceScales.add(scale);
    }
}
