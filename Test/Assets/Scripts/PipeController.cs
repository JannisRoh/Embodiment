using UnityEngine;
using Leap;
using System.Collections;

public class PipeController : MonoBehaviour
{
    public Anchor socket; // Assigned in the inspector
    private bool isConnected = false;
    private Vector3 originalPosition;
    private AnchorableBehaviour anchorable;
    private Vector3 originalPositionAnchorable;
    public float returnSpeed = 2.0f;
    private bool isReturning = false;

    public bool IsAnchorAttached = false;

    public void SetupPipe()
    {
        isConnected = false;
        anchorable = GetComponentInChildren<AnchorableBehaviour>();
        originalPosition = transform.position;
        if (anchorable != null)
        {
            originalPositionAnchorable = anchorable.transform.position;
            CheckAnchorStatus();
        }
        else
        {
            Debug.LogError("Anchorable component not found on child of " + gameObject.name);
        }
    }

    private void Update()
    {
        if (isReturning)
        {
            // Smoothly move the pipe back to the original position
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);

            // Smoothly move the anchorable child back to its original position, if applicable
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

    public void ConnectToSocket()
    {
        isConnected = true;
    }

    public void DisconnectFromSocket()
    {
        if (anchorable != null)
        {
            anchorable.Detach();
            IsAnchorAttached = false;
            Debug.Log("Anchorable detached from socket.");
            isConnected = false;
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
            IsAnchorAttached = anchorable.IsWithinRange(socket);
            //IsAnchorAttached = anchorable.IsAttached; // Set based on the anchorable's attachment status
            Debug.Log("Anchorable attachment status: " + IsAnchorAttached);
        }
    }

    public IEnumerator ShakeAnchorable(float duration, float shakeMagnitude, float shakeInterval)
    {
        Vector3 originalPosition = anchorable.transform.position;
        Debug.Log("Shake in PipeController");
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Calculate the target position by moving along the y-axis by shakeMagnitude
            Vector3 targetPosition = originalPosition + new Vector3(0, shakeMagnitude, 0);
            
            // Smoothly move to the target position
            float t = 0f;
            while (t < 1f)
            {
                anchorable.transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
                t += Time.deltaTime / shakeInterval; // Increase t over time
                yield return null; // Wait for the next frame
            }

            // Reset back to the original position
            t = 0f;
            while (t < 1f)
            {
                anchorable.transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
                t += Time.deltaTime / shakeInterval; // Increase t over time
                yield return null; // Wait for the next frame
            }

            elapsed += shakeInterval; // Increase elapsed time by shakeInterval
            yield return new WaitForSeconds(shakeInterval); // Wait for shakeInterval before next shake
        }

        // Reset to the original position after shaking is done
        anchorable.transform.position = originalPosition; 
    }


}