using UnityEngine;

public class TargetColorController : MonoBehaviour
{
    Color[] possibleColors = {
        new Color(0, 0, 0),               // Black
        new Color(0, 1, 1),               // Cyan
        new Color(1, 1, 0),               // Yellow
        new Color(1, 0, 1),               // Magenta
        new Color(0, 0.5f, 0.5f),         // Teal
        new Color(0.5f, 0, 0.5f),         // Purple
        new Color(0.5f, 0.5f, 0),         // Olive
        new Color(1f, 0.5f, 0.5f),        // Light Red
        new Color(0.5f, 1, 0.5f),         // Light Green
        new Color(0.5f, 0.5f, 1),         // Light Blue
        new Color(0, 0.33f, 0.33f),       // Dark Cyan
        new Color(0.33f, 0.33f, 0),       // Dark Yellow
        new Color(0.33f, 0, 0.33f),       // Dark Magenta
        new Color(0.33f, 0.67f, 0.33f),   // Dark Green
        new Color(0.33f, 0.33f, 0.67f),   // Dark Blue
        new Color(0.67f, 0.33f, 0.33f),   // Dark Red
        new Color(0.33f, 1, 0.67f),       // Light Greenish-Cyan
        new Color(0.33f, 0.67f, 1),       // Light Sky Blue
        new Color(1, 0.67f, 0.33f),       // Light Orange
        new Color(0.67f, 0.67f, 0.67f)    // Light Gray
    };

    public Color randomColor;
    public Transform border;

    public Color GenerateRandomColor()
    {
    int randomIndex = Random.Range(0, possibleColors.Length);
    return possibleColors[randomIndex];
    }

    public void SetupTargetColor()
    {
        AssignRandomColor();
        ResetBorder();
    }

    private void AssignRandomColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Generate a random color
            randomColor = GenerateRandomColor();
            renderer.material.color = randomColor;

            Debug.Log($"Assigned random color {randomColor} to {renderer.name}");
        }
        else
        {
            Debug.LogWarning("Target renderer is not assigned.");
        }
    }

    private void ResetBorder()
    {
        border.GetComponent<Renderer>().enabled = false;
    }

    
}