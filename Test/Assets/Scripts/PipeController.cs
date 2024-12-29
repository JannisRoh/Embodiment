using UnityEngine;
using Leap;
using System.Collections;
using GogoGaga.OptimizedRopesAndCables;

public class PipeController : MonoBehaviour
{
    public PaintTankController[] paintTanks; // Array to store paint tanks
    private Anchor[] tankAnchors; // Array to store their anchors
    public PaintTankController connectedPaintTank;

    private Vector3 originalPosition;
    private AnchorableBehaviour anchorable;
    private Vector3 originalPositionAnchorable;
    public float returnSpeed = 2.0f;
    private bool isReturning = false;

    public bool IsAnchorAttached = false;

    public ParticleSystem particleSystem;

    private Rope pipeRope;
    private float originalRopeLength = 0.3f;


    private void Awake()
    {
        // Store the true original positions on object creation
        originalPosition = transform.position;
        anchorable = GetComponentInChildren<AnchorableBehaviour>();
        if (anchorable != null)
        {
            originalPositionAnchorable = anchorable.transform.position;
        }
        else
        {
            Debug.LogError("Anchorable component not found on child of " + gameObject.name);
        }

        pipeRope = GetComponentInChildren<Rope>();
        if (pipeRope != null)
        {
            originalRopeLength = pipeRope.ropeLength;
        }
    }

    public void SetupPipe()
    {
        DisconnectFromSocket();
        tankAnchors = new Anchor[paintTanks.Length];
        
        if (anchorable != null)
        {
            CheckAnchorStatus();
        }
        else
        {
            Debug.LogError("Anchorable component not found on child of " + gameObject.name);
        }
        for (int i = 0; i < paintTanks.Length; i++)
        {
            if (paintTanks[i] != null)
            {
                Anchor anchor = paintTanks[i].GetComponentInChildren<Anchor>();
                if (anchor != null)
                {
                    tankAnchors[i] = anchor;
                    Debug.Log($"Anchor found for Paint Tank {i + 1}: {anchor.name}");
                }
                else
                {
                    Debug.LogWarning($"No Anchor found for Paint Tank {i + 1}");
                }
            }
        }
    }

    private void Update()
    {
        StretchRope();
        if (isReturning)
        {
            if (anchorable != null)
            {
                anchorable.transform.position = Vector3.Lerp(anchorable.transform.position, originalPositionAnchorable, returnSpeed * Time.deltaTime);
            }

            // Check if both the pipe and its anchorable child are close enough to stop moving
            bool pipeReachedPosition = Vector3.Distance(transform.position, originalPosition) < 0.01f;
            bool anchorableReachedPosition = anchorable != null && Vector3.Distance(anchorable.transform.position, originalPositionAnchorable) < 0.01f;

            if (pipeReachedPosition && anchorableReachedPosition)
            {
                transform.position = originalPosition;
                if (anchorable != null) anchorable.transform.position = originalPositionAnchorable;
                
                isReturning = false; // Stop the return process only if both have reached their positions
                Debug.Log("Pipe and anchorable returned to original position");
            }
        }
    }

    public void DisconnectFromSocket()
    {
        if (anchorable != null)
        {
            anchorable.Detach();
            IsAnchorAttached = false;
            Debug.Log("Anchorable detached from socket.");
        }
        else
        {
            Debug.LogError("Anchorable component not assigned.");
        }

        isReturning = true;
        Debug.Log("Pipe disconnected and returning to original position");
    }


    public void CheckAnchorStatus()
    {
        if (anchorable != null)
        {
            // Loop through all tank anchors to check if within range
            for (int i = 0; i < tankAnchors.Length; i++)
            {
                if (tankAnchors[i] != null && anchorable.IsWithinRange(tankAnchors[i]))
                {
                    connectedPaintTank = paintTanks[i]; // Assign the PaintTankController
                    //Debug.Log($"Connected to Paint Tank: {connectedPaintTank.name}");
                    return; // Exit the method early once a connection is found
                }
            }

            // If no connection is found, set connectedPaintTank to null
            connectedPaintTank = null;
            //Debug.Log("Not connected to any paint tank");
        }
    }

    public void EmitPaint()
    {
        Debug.Log("Calling EmitPaint()");
        if (particleSystem != null)
        {
            Debug.Log("paint is emitted");
            particleSystem.Play();
        }
        else
        {
            Debug.LogError("Particle System reference is null.");
        }
    }

    public void StopPaint()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
            //Debug.Log("Particle system stopped.");
        }
    }


    public IEnumerator ShakeAnchorable(float duration, float shakeMagnitude, float shakeInterval)
    {
        Vector3 originalPosition = anchorable.transform.position;
        Debug.Log("Shake in PipeController");
        float elapsed = 0.0f;
        bool moveUp = true; // Toggle direction

        while (elapsed < duration)
        {
            // Calculate the target position based on the current direction
            Vector3 targetPosition = originalPosition + new Vector3(0, moveUp ? shakeMagnitude : -shakeMagnitude, 0);

            // Smoothly move to the target position
            float t = 0f;
            while (t < 1f)
            {
                anchorable.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
                t += Time.deltaTime / shakeInterval; // Increase t over time
                yield return null; // Wait for the next frame
            }

            // Toggle direction for the next shake
            moveUp = !moveUp;

            // Wait for shakeInterval before next shake
            elapsed += shakeInterval;
            yield return new WaitForSeconds(shakeInterval);
        }

        // Reset to the original position after shaking is done
        anchorable.transform.position = originalPosition;
    }

    private void StretchRope()
    {
        // Calculate the distance between the current position and the original position of the anchorable
        float distance = Vector3.Distance(anchorable.transform.position, originalPositionAnchorable);

        // Adjust the rope length based on the distance
        pipeRope.ropeLength = originalRopeLength + distance;

        //Debug.Log($"Rope stretched to new length: {pipeRope.ropeLength}");
    }




}