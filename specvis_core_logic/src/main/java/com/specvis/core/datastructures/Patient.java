package com.specvis.core.datastructures;

import java.util.ArrayList;
import java.util.List;

public class Patient {

    private String patientName;
    private String patientId;
    private String dateOfBirth; // Consider using java.util.Date or String
    private String gender;
    // Add other relevant patient fields: medical history, notes, etc.

    // List of procedure IDs or results associated with this patient
    private List<String> procedureHistoryIds;

    public Patient(String patientName, String patientId, String dateOfBirth, String gender) {
        this.patientName = patientName;
        this.patientId = patientId;
        this.dateOfBirth = dateOfBirth;
        this.gender = gender;
        this.procedureHistoryIds = new ArrayList<>();
    }

    // Getters and Setters
    public String getPatientName() {
        return patientName;
    }

    public void setPatientName(String patientName) {
        this.patientName = patientName;
    }

    public String getPatientId() {
        return patientId;
    }

    public void setPatientId(String patientId) {
        this.patientId = patientId;
    }

    public String getDateOfBirth() {
        return dateOfBirth;
    }

    public void setDateOfBirth(String dateOfBirth) {
        this.dateOfBirth = dateOfBirth;
    }

    public String getGender() {
        return gender;
    }

    public void setGender(String gender) {
        this.gender = gender;
    }

    public List<String> getProcedureHistoryIds() {
        return procedureHistoryIds;
    }

    public void setProcedureHistoryIds(List<String> procedureHistoryIds) {
        this.procedureHistoryIds = procedureHistoryIds;
    }

    public void addProcedureToHistory(String procedureId) {
        this.procedureHistoryIds.add(procedureId);
    }

    @Override
    public String toString() {
        return "Patient{" +
                "patientName='" + patientName + '\'' +
                ", patientId='" + patientId + '\'' +
                '}';
    }
}
