using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] pipes;
    public GameObject[] buttons;
    public GameObject[] panels;
    public GameObject[] tanks;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        // Check if there is already an instance of GameManager
        if (Instance == null)
        {
            Instance = this; // Set the instance to this instance
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate GameManager instances
        }

        // Make sure GameManager is not destroyed when loading new scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeLevel();
    }

    private void Update()
    {
        CheckWinCondition();
    }

    private void InitializeLevel()
    {
        // Randomize progress bar requirements and button-press limits
        foreach (var pipe in pipes)
        {
            pipe.GetComponent<PipeController>().SetupPipe();
        }

        foreach (var button in buttons)
        {
            button.GetComponent<ButtonController>().RandomizeMaxPressDuration();
        }
        foreach (var panel in panels)
        {
            panel.GetComponent<TargetColorController>().SetupTargetColor();
        }
    }

    public void CheckWinCondition()
    {
        foreach (var tank in tanks)
        {
            var paintTankController = tank.GetComponent<PaintTankController>();
            
            if (paintTankController == null)
            {
                //Debug.LogError($"GameObject {tank.name} does not have a PaintTankController component.");
                continue; // Skip to the next tank
            }

            Debug.Log($"Checking tank {tank.name}: currentFillHeight = {paintTankController.currentFillHeight}, maxFillHeight = {paintTankController.maxFillHeight}");
            
            if (!paintTankController.IsFull())
            {
                //Debug.Log($"Tank {tank.name} is not full. (currentFillHeight: {paintTankController.currentFillHeight})");
                return; // Exit the function if a tank is not full
            }

            if (!paintTankController.ColorCheck())
            {
                //Debug.Log($"Tank {tank.name} color does not match the target. (currentColor: {paintTankController.currentColor}, targetColor: {paintTankController.panel.randomColor})");
                return; // Exit the function if a tank's color doesn't match
            }

            //Debug.Log($"Tank {tank.name} passed all checks (Full and Color match).");
        }

        Debug.Log("All tanks passed the checks. Initializing the next level.");
        InitializeLevel(); // If all tanks pass the checks, initialize the next level
    }

}
