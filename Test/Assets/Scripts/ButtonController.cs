using UnityEngine;
using Leap.PhysicalHands;

public class ButtonController : MonoBehaviour
{
    private PhysicalHandsButton physicalButton; // Reference to the Ultraleap PhysicalHandsButton component
    private float maxPressDuration;
    private float pressTimer;

    // Reference to the specific PipeController this button controls
    public PipeController associatedPipe;
    public ProgressBarController associatedProgressBar;
    public SourceController associatedSource;

    private bool isShaking = false;

    private void Start()
    {
        // Get the PhysicalHandsButton component attached to this GameObject
        physicalButton = GetComponent<PhysicalHandsButton>();

        if (physicalButton == null)
        {
            Debug.LogError("PhysicalHandsButton component not found on " + gameObject.name);
        }

        RandomizeMaxPressDuration(); // Set initial random max press duration
    }

    private void Update()
    {
        // Check if the button is pressed using physicalButton.IsPressed
        if (physicalButton != null && physicalButton.IsPressed)
        {
            pressTimer += Time.deltaTime;
            associatedSource.Active();
            associatedPipe.CheckAnchorStatus();

            if (associatedPipe.IsAnchorAttached == false)
            {
                associatedPipe.EmitPaint();
            }

            if (associatedPipe != null && associatedPipe.IsAnchorAttached)
            {
                associatedPipe.StopPaint();
                // Call the function to increase progress
                Debug.Log("Button is pressed and pipe in socket");
                associatedProgressBar.IncreaseProgress();
            }
            
            //Debug.Log("Press timer = " + pressTimer);
            if (pressTimer >= maxPressDuration - 1f && !isShaking && associatedPipe.IsAnchorAttached)
            {
                if (associatedPipe != null)
                {
                    StartCoroutine(associatedPipe.ShakeAnchorable(0.5f, 0.01f, 0.1f)); // Start shaking
                    isShaking = true; // Set shaking flag to true
                }
                Debug.Log("Shake in ButtonController");
            }

            // Check if the press duration exceeds the max duration
            if (pressTimer > maxPressDuration)
            {
                // Trigger DisconnectFromSocket on the associated pipe
                if (associatedPipe != null)
                {
                    Debug.LogError("Max press time exceeded");
                    associatedPipe.DisconnectFromSocket();
                }
                else
                {
                    Debug.LogError("No associated pipe assigned to " + gameObject.name);
                }

                pressTimer = 0; // Reset the timer
                isShaking = false;
            }
        }
        else
        {
            // Reset the timer if the button is released
            pressTimer = 0;
            isShaking = false;
        }
    }

    public void RandomizeMaxPressDuration()
    {
        // Randomize max press duration between 1 and 5 seconds
        maxPressDuration = Random.Range(1f, 5f);
        Debug.Log("Max press duration = " + maxPressDuration);
        associatedPipe.StopPaint();
    }
}
