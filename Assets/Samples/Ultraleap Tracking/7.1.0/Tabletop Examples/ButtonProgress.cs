using UnityEngine;
using UnityEngine.UI;
using Leap.PhysicalHands;

public class MultiButtonProgressWithRandomizer : MonoBehaviour
{
    // Reference the 5 Physical Hands Buttons
    public PhysicalHandsButton cyanButton;
    public PhysicalHandsButton yellowButton;
    public PhysicalHandsButton magentaButton;
    public PhysicalHandsButton blackButton;
    public PhysicalHandsButton whiteButton;

    // Reference the 5 Progress Bars (Sliders)
    public Slider cyanProgressBar;
    public Slider yellowProgressBar;
    public Slider magentaProgressBar;
    public Slider blackProgressBar;
    public Slider whiteProgressBar;

    // Reference the RectTransform for each slider to modify their width
    public RectTransform cyanBarTransform;
    public RectTransform yellowBarTransform;
    public RectTransform magentaBarTransform;
    public RectTransform blackBarTransform;
    public RectTransform whiteBarTransform;

    public float fillSpeed = 0.5f;  // Speed of progress bar fill
    private float cyanProgress = 0f;
    private float yellowProgress = 0f;
    private float magentaProgress = 0f;
    private float blackProgress = 0f;
    private float whiteProgress = 0f;

    private float cyanMaxValue;
    private float yellowMaxValue;
    private float magentaMaxValue;
    private float blackMaxValue;
    private float whiteMaxValue;

    void Start()
    {
        // Set initial random max values and resize progress bars
        RandomizeMaxValues();
    }

    void Update()
    {
        // Update the progress for the cyan button
        if (cyanButton.IsPressed)
        {
            cyanProgress += fillSpeed * Time.deltaTime;
            cyanProgressBar.value = Mathf.Clamp(cyanProgress, 0f, cyanMaxValue);
        }

        // Update the progress for the yellow button
        if (yellowButton.IsPressed)
        {
            yellowProgress += fillSpeed * Time.deltaTime;
            yellowProgressBar.value = Mathf.Clamp(yellowProgress, 0f, yellowMaxValue);
        }

        // Update the progress for the magenta button
        if (magentaButton.IsPressed)
        {
            magentaProgress += fillSpeed * Time.deltaTime;
            magentaProgressBar.value = Mathf.Clamp(magentaProgress, 0f, magentaMaxValue);
        }

        // Update the progress for the black button
        if (blackButton.IsPressed)
        {
            blackProgress += fillSpeed * Time.deltaTime;
            blackProgressBar.value = Mathf.Clamp(blackProgress, 0f, blackMaxValue);
        }

        // Update the progress for the white button
        if (whiteButton.IsPressed)
        {
            whiteProgress += fillSpeed * Time.deltaTime;
            whiteProgressBar.value = Mathf.Clamp(whiteProgress, 0f, whiteMaxValue);
        }

        // Check if all sliders have reached their max value
        if (AllSlidersMaxed())
        {
            // Randomize max values and reset progress
            RandomizeMaxValues();
            ResetProgress();
        }
    }

    // Randomize the max value of each slider between 10 and 100
    private void RandomizeMaxValues()
    {
        cyanMaxValue = Random.Range(10f, 100f);
        yellowMaxValue = Random.Range(10f, 100f);
        magentaMaxValue = Random.Range(10f, 100f);
        blackMaxValue = Random.Range(10f, 100f);
        whiteMaxValue = Random.Range(10f, 100f);

        // Update the max values of the sliders
        cyanProgressBar.maxValue = cyanMaxValue;
        yellowProgressBar.maxValue = yellowMaxValue;
        magentaProgressBar.maxValue = magentaMaxValue;
        blackProgressBar.maxValue = blackMaxValue;
        whiteProgressBar.maxValue = whiteMaxValue;

        // Resize the progress bars based on the max values
        ResizeProgressBar(cyanBarTransform, cyanMaxValue);
        ResizeProgressBar(yellowBarTransform, yellowMaxValue);
        ResizeProgressBar(magentaBarTransform, magentaMaxValue);
        ResizeProgressBar(blackBarTransform, blackMaxValue);
        ResizeProgressBar(whiteBarTransform, whiteMaxValue);
    }

    // Resize the progress bar according to the max value
    private void ResizeProgressBar(RectTransform barTransform, float maxValue)
    {
        float maxWidth = 200f;
        float width = (maxValue / 100f) * maxWidth;
        barTransform.sizeDelta = new Vector2(width, barTransform.sizeDelta.y);
    }

    // Reset the progress of all sliders
    private void ResetProgress()
    {
        cyanProgress = 0f;
        yellowProgress = 0f;
        magentaProgress = 0f;
        blackProgress = 0f;
        whiteProgress = 0f;

        cyanProgressBar.value = 0f;
        yellowProgressBar.value = 0f;
        magentaProgressBar.value = 0f;
        blackProgressBar.value = 0f;
        whiteProgressBar.value = 0f;
    }

    // Check if all sliders have reached their max value
    private bool AllSlidersMaxed()
    {
        return cyanProgress >= cyanMaxValue &&
               yellowProgress >= yellowMaxValue &&
               magentaProgress >= magentaMaxValue &&
               blackProgress >= blackMaxValue &&
               whiteProgress >= whiteMaxValue;
    }
}
