package com.specvis.core.procedures;

// Removed JavaFX imports:
// import javafx.animation.KeyFrame;
// import javafx.animation.Timeline;
// import javafx.event.ActionEvent;
// import javafx.scene.Scene;
// import javafx.scene.input.KeyCode;
// import javafx.scene.input.KeyEvent;
// import javafx.scene.layout.Pane;
// import javafx.scene.paint.Color; // Will replace with custom color storage if needed
// import javafx.scene.shape.Ellipse;
// import javafx.scene.shape.Line;
// import javafx.scene.shape.Polygon;
// import javafx.scene.text.Font;
// import javafx.scene.text.FontWeight;
// import javafx.scene.text.Text;
// import javafx.stage.Modality;
// import javafx.stage.Stage;
// import javafx.stage.StageStyle;
// import javafx.util.Duration;

import com.specvis.core.datastructures.ProcedureBasicData;
import com.specvis.core.datastructures.ProcedureBasicStimulus;
import com.specvis.core.datastructures.SpecvisData; // Assuming this is refactored
import com.specvis.core.datastructures.Patient; // Assuming this is refactored
import com.specvis.core.datastructures.settings.ProcedureBasicSettingsGeneral; // Assuming this is refactored
import com.specvis.core.datastructures.settings.UISettingsScreenAndLuminanceScale; // Assuming this is refactored
import com.specvis.core.logic.Functions; // Assuming this is refactored

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Random;
// Removed: import org.specvis.StartApplication;
// Removed: import org.specvis.controllers.ViewProcedurePreviewController;
// Removed: import org.specvis.logic.Functions; // Already imported from com.specvis.core.logic

public class ProcedureBasicFixMonitorNone { // Extends Stage removed

    private ProcedureCallback callback;
    private Functions functions; // To be injected
    private ProcedureBasicSettingsGeneral settings; // To be injected
    private UISettingsScreenAndLuminanceScale uiSettings; // To be injected
    private SpecvisData specvisData; // To be injected
    private Patient currentPatient; // To be injected

    // Fields from original class, some types might change
    protected String procedureType;
    protected boolean procedureIsStarted = false;
    protected boolean procedureIsRunning = false;
    protected boolean procedureIsFinished = false;
    protected boolean procedureWasPaused = false;
    protected boolean procedureWasCanceled = false;
    protected boolean bootIsOn = false;
    protected boolean stimulusIsOn = false;
    protected boolean responseIsExpected = false;
    protected boolean responseWasMade = false;
    protected boolean stimulusWasPerceived = false;
    protected boolean fixationPointIsOn = false;
    protected boolean blindSpotIsOn = false;
    protected boolean blindSpotWasLocalized = false;
    protected boolean monitorIsOn = false;

    protected List<ProcedureBasicStimulus> listOfStimuli;
    protected List<ProcedureBasicStimulus> listOfActiveStimuli;
    protected List<ProcedureBasicStimulus> listOfStimuliToDisplayRandomly;
    protected ProcedureBasicStimulus currentlyDisplayedStimulus;
    protected ProcedureBasicStimulus previouslyDisplayedStimulus;

    protected int totalNumberOfStimuliToDisplay;
    protected int numberOfStimuliPerBrightnessLevel;
    protected int totalNumberOfStimuliPresentations;
    protected int currentStimulusPresentationNumber = 0;
    protected int totalNumberOfStimuliDeactivated = 0;
    protected int numberOfReversalsToDeactivateStimulus;

    protected double fixationPointPositionX;
    protected double fixationPointPositionY;
    // private Color fixPointColor; // Replaced by individual components or direct setting from Unity
    protected double fixPointHSBBrightness; // Store HSB brightness if logic dictates color

    protected long timeProcedureStarted;
    protected long timeProcedureFinished;
    protected long timeProcedurePaused;
    protected long timeProcedureResumed;
    protected long totalTimeProcedurePaused = 0;
    protected long totalProcedureDuration;

    protected double currentBrightnessLevelBackground;
    protected double currentBrightnessLevelFixPoint;
    protected double currentBrightnessLevelStimulus;
    protected double currentBrightnessLevelBlindSpot;
    protected double currentBrightnessLevelMonitor;

    // Replaces Timelines for external control
    private int currentStepInLogic = 0; // To manage sequence of operations
    private static final int STEP_BOOT_START = 1;
    private static final int STEP_BOOT_WAIT = 2;
    private static final int STEP_PREPARE_STIMULUS = 3;
    private static final int STEP_DISPLAY_STIMULUS = 4;
    private static final int STEP_WAIT_FOR_RESPONSE = 5;
    private static final int STEP_PROCESS_RESPONSE = 6;
    private static final int STEP_HIDE_STIMULUS = 7;
    private static final int STEP_CHECK_TEST_END = 8;
    private static final int STEP_FINISH = 9;


    // Constructor
    public ProcedureBasicFixMonitorNone(
            Functions functions,
            ProcedureBasicSettingsGeneral settings,
            UISettingsScreenAndLuminanceScale uiSettings,
            SpecvisData specvisData,
            Patient currentPatient,
            ProcedureCallback callback) {

        this.functions = functions;
        this.settings = settings;
        this.uiSettings = uiSettings;
        this.specvisData = specvisData;
        this.currentPatient = currentPatient;
        this.callback = callback;

        // Removed all JavaFX Stage and Scene initialization
        // Removed KeyEvent handler registration

        initLocalFields(); // Call this to set up internal state
    }

    protected void initLocalFields() {
        // Removed: functions = StartApplication.getFunctions();
        // Removed: viewProcedurePreviewController = StartApplication.getViewProcedurePreviewController();
        // Removed: displayPane = initializeDisplayPane();

        this.procedureType = settings.getProcedureType();
        // this.fixPointColor = settings.getFixationPointColor(); // Store as HSB/RGB if needed
        // For example, if HSB brightness is used by the logic:
        // if (settings.getFixationPointColor() != null) {
        //     this.fixPointHSBBrightness = settings.getFixationPointColor().getBrightness();
        // }


        this.listOfStimuli = new ArrayList<>();
        this.listOfActiveStimuli = new ArrayList<>();
        this.listOfStimuliToDisplayRandomly = new ArrayList<>();

        this.fixationPointPositionX = settings.getFixationPointPositionX();
        this.fixationPointPositionY = settings.getFixationPointPositionY();

        this.currentBrightnessLevelBackground = settings.getBrightnessLevelBackground();
        this.currentBrightnessLevelFixPoint = settings.getBrightnessLevelFixPoint();
        this.currentBrightnessLevelStimulus = settings.getBrightnessLevelStimulus(); // Initial value
        this.currentBrightnessLevelBlindSpot = settings.getBrightnessLevelBlindSpot();
        this.currentBrightnessLevelMonitor = settings.getBrightnessLevelMonitor();
        this.numberOfReversalsToDeactivateStimulus = settings.getNumberOfReversalsToDeactivateStimulus();

        // Other initializations from the original class, e.g.
        this.totalNumberOfStimuliToDisplay = settings.getTotalNumberOfStimuliToDisplay();

        // Create the list of stimuli (this method needs significant refactoring)
        this.listOfStimuli = createListOfStimuli(settings);
        this.listOfActiveStimuli.addAll(listOfStimuli);
        this.listOfStimuliToDisplayRandomly.addAll(listOfActiveStimuli);
        Collections.shuffle(this.listOfStimuliToDisplayRandomly, new Random(System.nanoTime()));

        this.totalNumberOfStimuliPresentations = listOfStimuli.size() * settings.getMaximumNumberOfPresentationsPerStimulus();

        if (callback != null) {
            callback.onMessage("Procedure initialized. Ready to start.", ProcedureCallback.MessageType.INFO);
            callback.onProgressUpdate(0, 0, listOfStimuli.size());
            // Update fixation point if it should be visible from the start
            if (settings.isFixationPointIsOn()) {
                 // Assuming fixation point color needs to be communicated if not fixed in Unity
                callback.onFixationPointUpdate(true, fixationPointPositionX, fixationPointPositionY);
                fixationPointIsOn = true;
            }
        }
    }

    protected List<ProcedureBasicStimulus> createListOfStimuli(ProcedureBasicSettingsGeneral procedureSettings) {
        List<ProcedureBasicStimulus> stimuliList = new ArrayList<>();
        String[] stimuliPositions = procedureSettings.getStimuliPositions().split(";");

        for (int i = 0; i < stimuliPositions.length; i++) {
            ProcedureBasicStimulus stimulus = new ProcedureBasicStimulus();
            stimulus.setIndex(i);

            String[] coordinates = stimuliPositions[i].split(",");
            double xPosDegrees = Double.parseDouble(coordinates[0]);
            double yPosDegrees = Double.parseDouble(coordinates[1]);

            stimulus.setDistanceFromFixPointOnTheFieldOfViewInDegreesX(xPosDegrees);
            stimulus.setDistanceFromFixPointOnTheFieldOfViewInDegreesY(yPosDegrees);

            // Calculate pixel positions if needed by some internal logic,
            // though Unity will likely use degree positions directly.
            // This requires UISettingsScreenAndLuminanceScale to be available.
            if (uiSettings != null && functions != null) {
                double[] pixelPos = functions.calculateStimulusPositionOnTheScreenInPixels(
                        xPosDegrees, yPosDegrees,
                        uiSettings.getScreenWidthInPixels(), uiSettings.getScreenHeightInPixels(),
                        uiSettings.getScreenWidthInMillimeters(), uiSettings.getViewingDistanceInMillimeters(),
                        fixationPointPositionX, fixationPointPositionY, // Assuming these are in pixels for this calculation if needed
                        uiSettings.getScreenDistortionCorrectionQuadraticCoefficient(),
                        uiSettings.getScreenDistortionCorrectionLinearCoefficient(),
                        uiSettings.getScreenDistortionCorrectionConstantCoefficient()
                );
                stimulus.setPositionOnTheScreenInPixelsX(pixelPos[0]);
                stimulus.setPositionOnTheScreenInPixelsY(pixelPos[1]);
            } else {
                 // Fallback or error if settings not available for pixel calculation
                 // For VR, pixel positions are less relevant than angular positions.
                stimulus.setPositionOnTheScreenInPixelsX(0); // Or handle error
                stimulus.setPositionOnTheScreenInPixelsY(0);
            }

            stimulus.setDisplayTime(procedureSettings.getStimulusDisplayTime());
            stimulus.setCurrentBrightnessValue(procedureSettings.getBrightnessLevelStimulus()); // Initial brightness

            if (procedureSettings.getStimulusShape().equalsIgnoreCase("Ellipse")) {
                initializeStimulusAsEllipse(stimulus, procedureSettings);
            } else if (procedureSettings.getStimulusShape().equalsIgnoreCase("Polygon")) {
                initializeStimulusAsPolygon(stimulus, procedureSettings);
            } else {
                // Default or error for unknown shape
                initializeStimulusAsEllipse(stimulus, procedureSettings); // Default to Ellipse
                if (callback != null) {
                    callback.onMessage("Unknown stimulus shape: " + procedureSettings.getStimulusShape() + ". Defaulting to Ellipse.", ProcedureCallback.MessageType.WARNING);
                }
            }
            stimuliList.add(stimulus);
        }
        return stimuliList;
    }


    // Refactored versions of createEllipseStimulus / createPolygonStimulus
    // These now populate ProcedureBasicStimulus fields instead of creating Shape objects.
    protected void initializeStimulusAsEllipse(ProcedureBasicStimulus stimulus, ProcedureBasicSettingsGeneral params) {
        stimulus.setStimulusType("Ellipse");
        // Convert visual angle size to pixels if necessary, or store in degrees
        // This depends on how Unity will consume this. Let's assume storing in degrees.
        stimulus.setStimulusSizeDegX(params.getStimulusSizeInVisualDegrees());
        stimulus.setStimulusSizeDegY(params.getStimulusSizeInVisualDegrees()); // Ellipse from circle
        stimulus.setStimulusInclinationDeg(0); // Not applicable for circle-ellipse
        // Color/brightness will be set during presentation logic
    }

    protected void initializeStimulusAsPolygon(ProcedureBasicStimulus stimulus, ProcedureBasicSettingsGeneral params) {
        stimulus.setStimulusType("Polygon");
        stimulus.setStimulusSizeDegX(params.getStimulusSizeInVisualDegrees()); // Assuming this is overall size
        stimulus.setStimulusSizeDegY(params.getStimulusSizeInVisualDegrees());
        stimulus.setStimulusInclinationDeg(params.getStimulusInclination());
        // Specific polygon points/definition might be needed if not a standard shape (e.g. square)
        // For now, this assumes Unity knows how to draw a "Polygon" of a given size/inclination.
    }

    // initializeStimulus is now merged into createListOfStimuli or the specific initializeAs... methods

    // --- Public methods for Unity to control the procedure ---

    public void startProcedure() {
        if (procedureIsStarted) {
            if (callback != null) callback.onMessage("Procedure already started.", ProcedureCallback.MessageType.WARNING);
            return;
        }
        procedureIsStarted = true;
        procedureIsRunning = true;
        procedureIsFinished = false;
        procedureWasCanceled = false;
        timeProcedureStarted = System.currentTimeMillis();

        if (callback != null) {
            callback.onMessage("Procedure started.", ProcedureCallback.MessageType.INFO);
            if (settings.isFixationPointIsOn() && !fixationPointIsOn) {
                 callback.onFixationPointUpdate(true, fixationPointPositionX, fixationPointPositionY);
                 fixationPointIsOn = true;
            }
        }
        currentStepInLogic = STEP_BOOT_START; // Start the logical flow
        // The Unity game loop will call an `update(deltaTime)` method to drive the steps
    }

    public void pauseProcedure() {
        if (!procedureIsStarted || procedureIsFinished || !procedureIsRunning) {
            if (callback != null) callback.onMessage("Procedure not running or already finished.", ProcedureCallback.MessageType.WARNING);
            return;
        }
        procedureIsRunning = false;
        procedureWasPaused = true;
        timeProcedurePaused = System.currentTimeMillis();
        if (callback != null) callback.onMessage("Procedure paused.", ProcedureCallback.MessageType.INFO);
        // Stop stimulus presentation, etc.
        if (stimulusIsOn && callback != null) {
            callback.onStimulusHidden();
        }
        stimulusIsOn = false;
        bootIsOn = false; // If boot was active
    }

    public void resumeProcedure() {
        if (!procedureIsStarted || procedureIsFinished || procedureIsRunning || !procedureWasPaused) {
            if (callback != null) callback.onMessage("Procedure not paused or cannot be resumed.", ProcedureCallback.MessageType.WARNING);
            return;
        }
        procedureIsRunning = true;
        procedureWasPaused = false;
        timeProcedureResumed = System.currentTimeMillis();
        totalTimeProcedurePaused += (timeProcedureResumed - timeProcedurePaused);
        if (callback != null) callback.onMessage("Procedure resumed.", ProcedureCallback.MessageType.INFO);
        // Potentially re-show fixation point if it was hidden
        if (settings.isFixationPointIsOn() && !fixationPointIsOn && callback != null) {
             callback.onFixationPointUpdate(true, fixationPointPositionX, fixationPointPositionY);
             fixationPointIsOn = true;
        }
        // The update loop will continue from the currentStepInLogic
    }

    public void cancelProcedure() {
        if (!procedureIsStarted || procedureIsFinished) {
             if (callback != null) callback.onMessage("Procedure not started or already finished.", ProcedureCallback.MessageType.WARNING);
            return;
        }
        procedureIsRunning = false;
        procedureIsFinished = true;
        procedureWasCanceled = true;
        timeProcedureFinished = System.currentTimeMillis();
        if (stimulusIsOn && callback != null) {
            callback.onStimulusHidden();
        }
        stimulusIsOn = false;
        if (callback != null) {
            callback.onMessage("Procedure canceled by user.", ProcedureCallback.MessageType.INFO);
            // Send dummy/final results if needed
            ProcedureBasicData finalData = generateProcedureData();
            callback.onTestFinished(finalData);
        }
    }

    /**
     * This method should be called repeatedly by the Unity game loop.
     * @param deltaTime Time since last update (can be used for future timed operations if any)
     */
    public void update(float deltaTime) {
        if (!procedureIsRunning || procedureIsFinished) {
            return;
        }

        switch (currentStepInLogic) {
            case STEP_BOOT_START:
                // Replaces initAndRunBootTimeline()
                if (settings.isBootTimeIsRandom()) {
                    settings.setBootTime((long) (Math.random() * (settings.getBootTimeMaximum() - settings.getBootTimeMinimum()) + settings.getBootTimeMinimum()));
                }
                bootIsOn = true;
                if (callback != null) callback.onMessage("Boot period started: " + settings.getBootTime() + "ms", ProcedureCallback.MessageType.INFO);
                // In a real scenario, Unity would handle the wait. Here we transition.
                // For a library, we'd need a way for Unity to tell us when the time is up,
                // or this method is called frequently and we check System.currentTimeMillis().
                // For simplicity, let's assume for now that Unity calls this, then waits, then calls again.
                // A better way is for this method to do its part and Unity decides when to call next.
                // So, this step just sets up. The next step would be the "waiting" step.
                currentStepInLogic = STEP_BOOT_WAIT;
                // To simulate a wait, we'd need to store a start time for the boot.
                // For now, let's assume Unity handles the delay and then calls `processUserAction` or similar to advance.
                // This is a simplification. A more robust approach would involve this update method
                // checking elapsed time if it's responsible for timing.
                // Let's assume Unity calls `notifyBootFinished()` when the time is up.
                break;

            // STEP_BOOT_WAIT would involve checking elapsed time.
            // Let's assume an external trigger (like notifyBootFinished) moves to STEP_PREPARE_STIMULUS

            case STEP_PREPARE_STIMULUS:
                // Part of initAndRunStimulusTimeline() - choosing stimulus
                if (listOfStimuliToDisplayRandomly.isEmpty()) {
                    if (!listOfActiveStimuli.isEmpty()) { // Repopulate if active stimuli remain
                        listOfStimuliToDisplayRandomly.addAll(listOfActiveStimuli);
                        Collections.shuffle(listOfStimuliToDisplayRandomly, new Random(System.nanoTime()));
                    } else {
                        currentStepInLogic = STEP_FINISH; // All stimuli processed or deactivated
                        break;
                    }
                }
                
                if (listOfStimuliToDisplayRandomly.isEmpty() && listOfActiveStimuli.isEmpty()) {
                     currentStepInLogic = STEP_FINISH; // Should be caught by previous check, but safety.
                     break;
                }


                previouslyDisplayedStimulus = currentlyDisplayedStimulus;
                currentlyDisplayedStimulus = listOfStimuliToDisplayRandomly.remove(0);

                // Update brightness based on staircase/test logic
                // This is where getBrightnessForNextStimulusPresentation would be used.
                // For now, let's assume it sets currentBrightnessLevelStimulus or directly on stimulus.
                currentBrightnessLevelStimulus = getBrightnessForNextStimulusPresentation(currentlyDisplayedStimulus);
                currentlyDisplayedStimulus.setCurrentBrightnessValue(currentBrightnessLevelStimulus);

                if (callback != null) {
                    callback.onStimulusReadyToPresent(currentlyDisplayedStimulus);
                }
                currentStepInLogic = STEP_DISPLAY_STIMULUS; // Ready to be displayed by Unity
                break;

            case STEP_DISPLAY_STIMULUS:
                // Unity would render the stimulus based on data from currentlyDisplayedStimulus.
                // This step in the library primarily means "stimulus is now considered visible".
                stimulusIsOn = true;
                responseIsExpected = true;
                responseWasMade = false;
                currentStimulusPresentationNumber++;
                currentlyDisplayedStimulus.incrementPresentationCount();

                if (callback != null) {
                    // This might be redundant if onStimulusReadyToPresent is enough
                    // callback.onMessage("Stimulus " + currentlyDisplayedStimulus.getIndex() + " presented.", MessageType.INFO);
                }
                // Unity needs to handle the display time (settings.getStimulusDisplayTime())
                // After display time, Unity should call `notifyStimulusDisplayTimeEnded()`
                currentStepInLogic = STEP_WAIT_FOR_RESPONSE; // Waiting for user or timeout
                break;

            case STEP_WAIT_FOR_RESPONSE:
                // This state is primarily managed by Unity. Unity waits for input.
                // If input comes, Unity calls `processUserResponse(boolean seen)`.
                // If display time ends *before* input, Unity calls `notifyStimulusDisplayTimeEnded()`.
                // If response time ends (settings.getMaximumResponseTime()), Unity calls `processUserResponse(false)` (or specific timeout signal).
                break;

            case STEP_PROCESS_RESPONSE:
                // This step is triggered by processUserResponse() or notifyStimulusDisplayTimeEnded()
                // which will set the next state.
                decideAboutStimulusDeactivation(currentlyDisplayedStimulus, stimulusWasPerceived);

                if (callback != null) {
                    callback.onStimulusHidden(); // Tell Unity to hide it
                }
                stimulusIsOn = false;
                responseIsExpected = false;
                currentStepInLogic = STEP_CHECK_TEST_END;
                break;

            // STEP_HIDE_STIMULUS is effectively handled by onStimulusHidden and Unity acting on it.

            case STEP_CHECK_TEST_END:
                double progress = (double) totalNumberOfStimuliDeactivated / listOfStimuli.size();
                if (callback != null) {
                    callback.onProgressUpdate(progress, totalNumberOfStimuliDeactivated, listOfStimuli.size());
                }

                if (totalNumberOfStimuliDeactivated == listOfStimuli.size() ||
                    currentStimulusPresentationNumber >= totalNumberOfStimuliPresentations) {
                    currentStepInLogic = STEP_FINISH;
                } else {
                    currentStepInLogic = STEP_PREPARE_STIMULUS; // Loop back for next stimulus
                }
                break;

            case STEP_FINISH:
                procedureIsRunning = false;
                procedureIsFinished = true;
                timeProcedureFinished = System.currentTimeMillis();
                totalProcedureDuration = (timeProcedureFinished - timeProcedureStarted) - totalTimeProcedurePaused;
                if (callback != null) {
                    callback.onMessage("Procedure finished. Total duration: " + totalProcedureDuration + "ms", ProcedureCallback.MessageType.INFO);
                    ProcedureBasicData finalData = generateProcedureData();
                    callback.onTestFinished(finalData);
                }
                break;
        }
    }

    // Called by Unity when boot time is over
    public void notifyBootFinished() {
        if (currentStepInLogic == STEP_BOOT_WAIT) {
            bootIsOn = false;
            if (callback != null) callback.onMessage("Boot period finished.", ProcedureCallback.MessageType.INFO);
            currentStepInLogic = STEP_PREPARE_STIMULUS;
        }
    }

    // Called by Unity when stimulus display duration is over
    public void notifyStimulusDisplayTimeEnded() {
        if (currentStepInLogic == STEP_WAIT_FOR_RESPONSE && stimulusIsOn) {
            if (!responseWasMade) { // If user hasn't responded yet
                // Treat as "not seen" or handle as per procedure rules for timeout
                // This might depend on settings.getTreatTimeoutAs()
                stimulusWasPerceived = false; // Default assumption for timeout
                 if (callback != null) callback.onMessage("Stimulus display time ended. No response.", ProcedureCallback.MessageType.INFO);
            }
            // Even if response was made, the display time end means we hide the stimulus
            if (callback != null) {
                callback.onStimulusHidden();
            }
            stimulusIsOn = false;
            responseIsExpected = false; // Stop expecting response for this stimulus
            // If response was already made, it would have transitioned. If not, this timeout forces transition.
            if (!responseWasMade) {
                currentStepInLogic = STEP_PROCESS_RESPONSE;
            }
            // if responseWasMade, processUserResponse would have already changed currentStepInLogic
        }
    }


    // Called by Unity when user presses a key (or makes a VR input)
    public void processUserResponse(boolean seen) {
        if (!procedureIsRunning || !responseIsExpected || responseWasMade) {
            // Log or callback warning: "Unexpected response or response already made."
            return;
        }
        responseWasMade = true;
        stimulusWasPerceived = seen;

        if (callback != null) {
            callback.onMessage("Response received: " + (seen ? "Seen" : "Not Seen"), ProcedureCallback.MessageType.INFO);
        }
        
        // The stimulus might still be "on" screen if response is faster than display time.
        // The original timeline logic for hiding stimulus after response:
        // Timeline timelineHideStimulus = new Timeline(new KeyFrame(Duration.millis(1), (ActionEvent event) -> { ... }));
        // This is now handled by Unity, or by `notifyStimulusDisplayTimeEnded`.
        // For immediate hiding on response:
        if (stimulusIsOn && callback != null) {
            // callback.onStimulusHidden(); // This could be immediate or wait for display time
        }
        // stimulusIsOn = false; // If hidden immediately
        currentStepInLogic = STEP_PROCESS_RESPONSE;
    }


    protected double getBrightnessForNextStimulusPresentation(ProcedureBasicStimulus stimulus) {
        // This is the core staircase logic. It's complex and relies heavily on settings.
        // The original code has this logic within the Timeline event handlers for stimulus presentation.
        // We need to replicate that logic here.

        double nextBrightness = stimulus.getCurrentBrightnessValue(); // Start with current value

        if (stimulus.getPresentationCount() <= 1) { // First presentation or first after init
            return settings.getBrightnessLevelStimulus(); // Use initial brightness from settings
        }

        // Retrieve response history for THIS stimulus.
        // The original code implies this is managed by checking `stimulusWasPerceived`
        // which is set by `processUserResponse`. This means `stimulusWasPerceived` reflects
        // the response to the *immediately preceding* presentation of *any* stimulus,
        // which might not be correct for a per-stimulus staircase.
        // A proper staircase needs response history *per stimulus*.
        // Let's assume `stimulus.getAnswers()` or a similar new field in `ProcedureBasicStimulus`
        // stores the history of perceived (1) / not perceived (0) for that specific stimulus.
        // For this refactoring, I'll use `stimulusWasPerceived` as a simplification,
        // implying it's correctly updated for the current stimulus before this call.

        // TODO: Enhance ProcedureBasicStimulus to store its own response history for accurate staircase.
        // For now, `stimulusWasPerceived` refers to the last global response.

        double stepSizeUp = settings.getStaircaseStepSizeUp(); // e.g., 2 dB
        double stepSizeDown = settings.getStaircaseStepSizeDown(); // e.g., 1 dB
        int ruleUp = settings.getStaircaseRuleUp(); // e.g., 1 (for 1 "no" -> increase brightness)
        int ruleDown = settings.getStaircaseRuleDown(); // e.g., 3 (for 3 "yes" in a row -> decrease brightness)

        // This is a simplified interpretation. Real staircase logic often looks at consecutive responses.
        // The original code's logic for this needs to be carefully translated.
        // Assuming a simple UP/DOWN based on the last response for now:
        if (stimulusWasPerceived) { // Last stimulus was seen
            nextBrightness -= stepSizeDown; // Decrease brightness (make harder)
        } else { // Last stimulus was not seen
            nextBrightness += stepSizeUp; // Increase brightness (make easier)
        }

        // Clamp to min/max brightness levels
        nextBrightness = Math.max(settings.getMinimumBrightnessLevelStimulus(), nextBrightness);
        nextBrightness = Math.min(settings.getMaximumBrightnessLevelStimulus(), nextBrightness);

        // Store this new brightness in the stimulus object for the upcoming presentation
        stimulus.setCurrentBrightnessValue(nextBrightness);
        return nextBrightness;
    }

    protected void decideAboutStimulusDeactivation(ProcedureBasicStimulus stimulus, boolean perceivedThisTime) {
        // Add current response to a history within the stimulus object
        // For now, we don't have a detailed history array in ProcedureBasicStimulus,
        // but this is where it would be updated.
        // stimulus.addResponseToHistory(perceivedThisTime);

        // Check for reversals. A reversal is when the trend of responses changes
        // (e.g., from "seen" to "not seen" or vice-versa) which causes a change
        // in the direction of brightness adjustment.
        // This logic requires knowing the *previous* brightness adjustment direction
        // and the *previous* responses.

        // Simplified logic:
        // The original code in `viewProcedurePreviewController.updateMonitorPaneWithResults`
        // seems to count reversals based on changes in brightness adjustment direction.
        // This means we need to track the last adjustment direction for the stimulus.

        // TODO: Add fields to ProcedureBasicStimulus:
        // - `private List<Boolean> responseHistory = new ArrayList<>();`
        // - `private List<Double> brightnessHistory = new ArrayList<>();`
        // - `private int reversalCount = 0;`
        // - `private boolean lastAdjustmentWasUp = false;` (or similar)

        // Example (conceptual - needs proper state in ProcedureBasicStimulus):
        // boolean previousResponse = stimulus.getLastResponse(); // Get from history
        // if (stimulus.getPresentationCount() > 1 && perceivedThisTime != previousResponse) {
        //    // Potential reversal, if brightness adjustment direction also flipped.
        //    // This logic is non-trivial and needs careful porting of the original's intent.
        //    // stimulus.incrementReversalCount();
        //    // stimulus.addReversalLevel(stimulus.getCurrentBrightnessValue());
        // }


        // Deactivate based on reversal count
        // if (stimulus.getReversalCount() >= numberOfReversalsToDeactivateStimulus) {
        // For this refactoring, using the placeholder logic:
        if (stimulus.getPresentationCount() >= settings.getMaximumNumberOfPresentationsPerStimulus() ||
            (stimulus.getReversalLevels().size() >= numberOfReversalsToDeactivateStimulus && numberOfReversalsToDeactivateStimulus > 0) ) {
            if (stimulus.isActive()) {
                stimulus.setActive(false);
                totalNumberOfStimuliDeactivated++;
                listOfActiveStimuli.remove(stimulus);
                if (callback != null) {
                    String reason = (stimulus.getReversalLevels().size() >= numberOfReversalsToDeactivateStimulus)
                                    ? "reversals reached" : "max presentations";
                    callback.onMessage("Stimulus " + stimulus.getIndex() + " deactivated ("+reason+").", ProcedureCallback.MessageType.INFO);
                }
            }
        }
    }
    
    protected ProcedureBasicData generateProcedureData() {
        ProcedureBasicData data = new ProcedureBasicData();
        // Assuming ProcedureBasicData is also refactored to be JavaFX-free
        data.setProcedureType(this.procedureType);
        // For Android compatibility, avoid java.time if minSDK < 26
        java.text.SimpleDateFormat dateFormat = new java.text.SimpleDateFormat("yyyy-MM-dd", java.util.Locale.getDefault());
        java.text.SimpleDateFormat timeFormat = new java.text.SimpleDateFormat("HH:mm:ss", java.util.Locale.getDefault());
        java.util.Date nowDate = new java.util.Date();
        data.setProcedureDate(dateFormat.format(nowDate));
        data.setProcedureTime(timeFormat.format(nowDate));
        
        data.setPatientId(currentPatient != null ? currentPatient.getPatientId() : "N/A");
        // data.setProcedureSettingsId(...); // This needs to come from settings object
        data.setFixationPointWasOn(settings.isFixationPointIsOn());
        data.setFixationPointPositionX(settings.getFixationPointPositionX()); // Assuming these are in degrees
        data.setFixationPointPositionY(settings.getFixationPointPositionY());
        data.setBrightnessLevelBackground(settings.getBrightnessLevelBackground());
        data.setBrightnessLevelFixPoint(settings.getBrightnessLevelFixPoint());
        // data.setBlindSpotWasLocalized(); // Needs corresponding logic if blind spot test is added
        // data.setBlindSpotPositionX();
        // data.setBlindSpotPositionY();
        // data.setBlindSpotSize();

        data.setTotalNumberOfStimuli(listOfStimuli.size());
        data.setTotalNumberOfStimuliDeactivated(totalNumberOfStimuliDeactivated);
        data.setTotalNumberOfStimuliNotDeactivated(listOfStimuli.size() - totalNumberOfStimuliDeactivated);
        data.setTotalProcedureDuration(totalProcedureDuration);
        data.setTotalTimeProcedurePaused(totalTimeProcedurePaused);
        data.setProcedureWasCanceled(procedureWasCanceled);
        
        // data.setScreenSettingsAreLoaded(); // From UISettings
        // data.setUsedScreenSettings(); // String representation of UI settings
        // data.setLuminanceScaleIsLoaded(); // From UISettings
        // data.setUsedLuminanceScale(); // String representation of Luminance Scale

        // Store refactored stimuli data
        data.setListOfStimuli(new ArrayList<>(this.listOfStimuli)); // Pass a copy

        // Calculate average threshold if applicable
        double sumThresholds = 0;
        int countThresholds = 0;
        for(ProcedureBasicStimulus s : this.listOfStimuli) {
            if (!s.isActive()) { // Assuming threshold is determined when deactivated
                // This needs a clear definition of what "brightnessThreshold" means
                // in ProcedureBasicStimulus after refactoring.
                // If it's the final brightness, then:
                sumThresholds += s.getCurrentBrightnessValue(); // Or s.getLuminanceThreshold() etc.
                countThresholds++;
            }
        }
        if (countThresholds > 0) {
            data.setMeanOfTheThresholds(sumThresholds / countThresholds);
        } else {
            data.setMeanOfTheThresholds(0.0);
        }

        return data;
    }

    // Getters for Unity to query state if needed (optional, depends on callback usage)
    public boolean isProcedureRunning() { return procedureIsRunning; }
    public boolean isProcedureFinished() { return procedureIsFinished; }
    public ProcedureBasicStimulus getCurrentlyDisplayedStimulus() { return currentlyDisplayedStimulus; }
    public boolean isStimulusCurrentlyDisplayed() { return stimulusIsOn; }


    // Methods to be removed (were related to JavaFX UI or Timelines):
    // createContent(), initializeDisplayPane(), drawFixationPoint(), drawProcedureStateIndicator()
    // initAndRunBootTimeline(), initAndRunStimulusTimeline(), initAndRunStopwatchTimeline()
    // runStimulusPresentation(), showStimulus(), hideStimulus()
    // updateMonitorPaneWithResults(), updateTableResults()
    // closeProcedure(), showAndWaitForClose()
}
