using UnityEngine;
using UnityEngine.EventSystems; // Required for event system interfaces

// Basic VR grabbable script. In a real project, this would use OVRGrabber/OVRGrabbable or similar from Oculus SDK.
public class D15GrabbableCap : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private FarnsworthD15TestManager manager;
    private D15CapInfo capInfo;
    private MeshRenderer meshRenderer;
    private Color originalColor;
    private bool isHighlighted = false;
    public bool isPlaced = false; // Set to true when in an arrangement slot

    // Simple state for click-to-grab-and-place
    private static D15GrabbableCap currentlyTryingToGrab = null;

    public void Initialize(FarnsworthD15TestManager manager, D15CapInfo capInfo)
    {
        this.manager = manager;
        this.capInfo = capInfo;
        this.meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (this.meshRenderer == null) this.meshRenderer = GetComponent<MeshRenderer>();
        if (this.meshRenderer != null) this.originalColor = this.meshRenderer.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPlaced && manager.currentState == FarnsworthD15TestManager.D15TestState.Arranging)
        {
            // Highlight effect (e.g., change color slightly, or use an outline shader)
            if (meshRenderer != null) meshRenderer.material.color = originalColor * 1.3f; // Simple brighten
            isHighlighted = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHighlighted)
        {
            if (meshRenderer != null) meshRenderer.material.color = originalColor;
            isHighlighted = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager.currentState != FarnsworthD15TestManager.D15TestState.Arranging) return;

        if (isPlaced) // Cap is already in an arrangement slot, pick it up
        {
            if (currentlyTryingToGrab == null) // No other cap is being "held" by click
            {
                // Pick up from slot
                isPlaced = false; // Mark as not placed anymore
                if (meshRenderer != null) this.originalColor = this.meshRenderer.material.color; // Re-cache color as it might have been set by manager
                manager.OnCapPickedUp(this.gameObject, this.capInfo);
                currentlyTryingToGrab = this; // This cap is now "held"
                // Visual feedback: make it slightly larger or attach to a conceptual "hand pointer"
                transform.localScale *= 1.1f; 
                if(manager.instructionText != null) manager.instructionText.text = $"Holding Cap {capInfo.capNumber}. Click an empty slot to place.";
            }
            else if (currentlyTryingToGrab == this) // Clicking the "held" cap again (optional: deselect/drop)
            {
                // Simple drop: manager needs a method to handle "dropping" a cap not over a slot
                // manager.OnCapDropped(this.gameObject, this.capInfo);
                // For now, this does nothing or returns it to original slot if that logic is in manager
                transform.localScale /= 1.1f; // Revert visual feedback
                currentlyTryingToGrab = null;
            }
        }
        else // Cap is in presentation area or "held"
        {
            if (currentlyTryingToGrab == null) // Not holding any cap, try to pick this one
            {
                 isPlaced = false; 
                manager.OnCapPickedUp(this.gameObject, this.capInfo);
                currentlyTryingToGrab = this;
                transform.localScale *= 1.1f;
                if(manager.instructionText != null) manager.instructionText.text = $"Holding Cap {capInfo.capNumber}. Click an empty slot to place.";
            }
            // If currentlyTryingToGrab is not null and not this, it means another cap is "held".
            // We can't pick up another. The user must first place the held cap.
        }
    }

    // This method would be called by a "Slot" script when it's clicked
    // For simplicity, assuming slots are just transforms and manager handles placement logic directly
    // if a "held" cap exists.
    public static void AttemptPlaceOnSlot(Transform slotTransform) {
        if (currentlyTryingToGrab != null) {
            currentlyTryingToGrab.transform.localScale /= 1.1f; // Revert visual feedback
            currentlyTryingToGrab.manager.OnCapTryPlace(slotTransform);
            // If successfully placed, manager will set currentlyTryingToGrab = null
            // If not, OnCapTryPlace should handle returning the cap or re-enabling its "held" state.
            // For this simplified model, manager's OnCapTryPlace will clear currentlyTryingToGrab if successful.
            if(currentlyTryingToGrab.isPlaced) { // Check if manager set it to placed
                 currentlyTryingToGrab = null;
            } else {
                // It was not placed, re-enable visual feedback
                 if(currentlyTryingToGrab != null) currentlyTryingToGrab.transform.localScale *= 1.1f; 
            }
        }
    }
     void OnDestroy() {
        if (currentlyTryingToGrab == this) {
            currentlyTryingToGrab = null;
        }
    }
}
