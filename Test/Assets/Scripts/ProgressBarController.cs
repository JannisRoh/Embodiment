using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    private Slider progressBar;
    private float fillSpeed = 100; // The speed at which the progress bar fills

    private float requiredFillAmount; // The amount needed to fill the bar
    private float currentFillAmount = 0; // The current amount filled

    private RectTransform progressBarTransform;

    private void Start()
    {
        // Automatically get the Slider component attached to this GameObject
        progressBar = GetComponent<Slider>();
        progressBarTransform = progressBar.GetComponent<RectTransform>();
        
        RandomizeFillRequirements();
    }

    public void RandomizeFillRequirements()
    {
        ResetProgress();
        requiredFillAmount = Random.Range(10f, 100f);
        progressBar.maxValue = requiredFillAmount; // Set max value to the required fill amount
        ResizeProgressBar(progressBarTransform, requiredFillAmount); // Resize based on the required amount
    }

    private void ResizeProgressBar(RectTransform barTransform, float maxValue)
    {
        float maxWidth = 200f; // The maximum width of the progress bar
        float width = (maxValue / 100f) * maxWidth; // Calculate width based on maxValue
        barTransform.sizeDelta = new Vector2(width, barTransform.sizeDelta.y); // Update the size
    }

    public void IncreaseProgress()
    {
        currentFillAmount += fillSpeed * Time.deltaTime; // Increment the current fill amount
        progressBar.value = Mathf.Clamp(currentFillAmount, 0f, requiredFillAmount); // Update the slider value
    }

    public void ResetProgress()
    {
        currentFillAmount = 0; // Reset current fill amount
        progressBar.value = 0f; // Reset slider value
    }

    public bool IsFull()
    {
        return currentFillAmount >= requiredFillAmount; // Check if the bar is full
    }
}
