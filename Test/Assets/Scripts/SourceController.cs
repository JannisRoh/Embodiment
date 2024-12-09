using UnityEngine;

public class SourceController : MonoBehaviour

{
    public Transform childNode;  // Assign the child node in the Inspector
    public float amplitude = 0.05f;  // Maximum scale change
    public float frequency = 2.5f; // Oscillation speed
    private Vector3 originalScale;

    private void Start()
    {
        if (childNode == null)
        {
            Debug.LogError("Child node not assigned. Please assign a child node in the Inspector.");
            return;
        }

        // Store the original scale of the child
        originalScale = childNode.localScale;
    }

    public void Active()
    {
        if (childNode == null) return;

        // Calculate the new Y scale using a sine function
        float newYScale = originalScale.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Apply the new scale
        childNode.localScale = new Vector3(originalScale.x, newYScale, originalScale.z);
    }
}

