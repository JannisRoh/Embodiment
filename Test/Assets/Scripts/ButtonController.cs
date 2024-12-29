using UnityEngine;
using Leap.PhysicalHands;

public class ButtonController : MonoBehaviour
{
    private PhysicalHandsButton physicalButton; // Reference to the Ultraleap PhysicalHandsButton component
    private float maxPressDuration;
    private float pressTimer;
    private bool isShaking = false;

    // Reference to the specific PipeController this button controls
    public PipeController associatedPipe;
    public SourceController associatedSource;

    public Color paintColor;
    private float paintAmount = 0.001f;

    

    private void Start()
    {
        // Get the PhysicalHandsButton component attached to this GameObject
        physicalButton = GetComponent<PhysicalHandsButton>();

        if (physicalButton == null)
        {
            Debug.LogError("PhysicalHandsButton component not found on " + gameObject.name);
        }
    }

    private void Update()
    {
        if (physicalButton != null && physicalButton.IsPressed)
        {
            pressTimer += Time.deltaTime;

            // Check if the pipe is attached to the socket
            associatedPipe.CheckAnchorStatus();
            associatedSource.Active();

            if (associatedPipe.connectedPaintTank == null || associatedPipe.connectedPaintTank.IsFull())
            {
                if (!associatedPipe.particleSystem.isPlaying) // Ensure EmitPaint() is called only once
                {
                    associatedPipe.EmitPaint();
                }
            }
            else
            {
                // Pipe is attached, stop emitting paint and handle progress
                associatedPipe.StopPaint();
                associatedPipe.connectedPaintTank.AddPaint(paintColor, paintAmount);
            }

            // Handle shaking logic
            if (pressTimer >= maxPressDuration - 1f && !isShaking && associatedPipe.connectedPaintTank)
            {
                StartCoroutine(associatedPipe.ShakeAnchorable(0.5f, 0.01f, 0.1f)); // Start shaking
                isShaking = true; // Set shaking flag
            }

            // Disconnect pipe if press duration exceeds max duration
            if (pressTimer > maxPressDuration)
            {
                if (associatedPipe != null)
                {
                    Debug.LogError("Max press time exceeded, disconnecting pipe.");
                    associatedPipe.DisconnectFromSocket();
                }
                else
                {
                    Debug.LogError("No associated pipe assigned to " + gameObject.name);
                }

                // Reset press timer and shaking flag
                pressTimer = 0;
                isShaking = false;
            }
        }
        else
        {
            // Button is released, reset logic
            pressTimer = 0;
            isShaking = false;
            associatedPipe.StopPaint();
        }
    }


        public void RandomizeMaxPressDuration()
        {
            // Randomize max press duration between 1 and 5 seconds
            maxPressDuration = Random.Range(2f, 5f);
            Debug.Log("Max press duration = " + maxPressDuration);
            //associatedPipe.StopPaint();
        }
    }
