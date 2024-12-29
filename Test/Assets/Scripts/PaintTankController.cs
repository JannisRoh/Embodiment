using UnityEngine;
using System.Collections;


public class PaintTankController : MonoBehaviour
{
    public Transform referencePoint; // Reference to the interior cylinder
    public Transform innerCylinder;
    public float maxFillHeight = 1.0f; // Maximum height of the cylinder
    public float currentFillHeight = 0.0f; // Current fill level
    public Color currentColor = Color.clear; // Current color of the tank
    private float totalPaintAmount = 0.0f; // Total amount of paint added
    public float drainSpeed = 1f;
    private Coroutine drainCoroutine;

    public TargetColorController panel;

    private void Update()
    {
        ColorCheckIndicator();
    }


    public void AddPaint(Color newColor, float paintAmount)
    {
            // Calculate the weighted color mix
            float newTotal = totalPaintAmount + paintAmount;
            currentColor = Color.Lerp(currentColor, newColor, paintAmount / newTotal);
            totalPaintAmount = newTotal;

            // Update the inner cylinder size and color
            currentFillHeight = Mathf.Clamp(currentFillHeight + paintAmount, 0, maxFillHeight);
            UpdateCylinderVisuals();
    }

    private void UpdateCylinderVisuals()
    {
        // Update the inner cylinder's scale and color
        if (referencePoint != null)
        {
            Vector3 newScale = referencePoint.localScale;
            newScale.y = currentFillHeight;
            referencePoint.localScale = newScale;

            Renderer renderer = innerCylinder.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = currentColor;
                Debug.Log($"current color is {currentColor}");
            }
        }
    }

    public bool IsFull()
    {
        return currentFillHeight >= maxFillHeight; // Check if the bar is full
    }

    public bool ColorCheck()
    {
        if (Mathf.Abs(currentColor.r - panel.randomColor.r) <= 0.1f &&
        Mathf.Abs(currentColor.g - panel.randomColor.g) <= 0.1f &&
        Mathf.Abs(currentColor.b - panel.randomColor.b) <= 0.1f)
    {
        return true;
    }
    else
    {
        return false;
    }
    }

    public void ColorCheckIndicator()
    {
        Renderer renderer = panel.border.GetComponent<Renderer>();
        if (!IsFull())
        {
            // Make the border invisible
            //renderer.enabled = false;
        }
        else
        {
            if (ColorCheck())
            {
                renderer.enabled = true;
                renderer.material.color = new Color(0.67f, 1f, 0.67f); // Light green
            }
            else
            {
                StartDraining();
            }
        }
    }
    public void StartDraining()
    {
        // Ensure only one coroutine runs at a time
        if (drainCoroutine != null)
        {
            StopCoroutine(drainCoroutine);
        }
        drainCoroutine = StartCoroutine(GraduallyReduceFill());
    }

    private IEnumerator GraduallyReduceFill()
    {
        Renderer borderRenderer = panel.border.GetComponent<Renderer>();

        if (borderRenderer != null)
        {
            // Make the border visible and red during draining
            borderRenderer.enabled = true;
            borderRenderer.material.color = Color.red;

            while (currentFillHeight > 0)
            {
                currentFillHeight = Mathf.Max(currentFillHeight - drainSpeed * Time.deltaTime, 0);
                UpdateCylinderVisuals();
                yield return null; // Wait for the next frame
            }

            // Once the tank is empty, make the border invisible
            borderRenderer.enabled = false;

            // Clear the tank color
            currentColor = Color.clear;
            totalPaintAmount = 0; // Reset the total paint amount
            UpdateCylinderVisuals();
            Debug.Log("Tank is empty, color cleared.");
        }
        else
        {
            Debug.LogError("Panel border renderer is missing.");
        }

        // End the coroutine
        drainCoroutine = null;
    }
}
