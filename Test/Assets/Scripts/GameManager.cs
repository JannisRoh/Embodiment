using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] pipes;
    public GameObject[] sockets;
    public GameObject[] buttons;
    public GameObject[] progressBars;

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
            pipe.GetComponent<PipeController>().DisconnectFromSocket();
            pipe.GetComponent<PipeController>().SetupPipe();
        }

        foreach (var progressBar in progressBars)
        {
            progressBar.GetComponent<ProgressBarController>().RandomizeFillRequirements();
        }
        foreach (var button in buttons)
        {
            button.GetComponent<ButtonController>().RandomizeMaxPressDuration();
        }
    }

    public void CheckWinCondition()
    {
        foreach (var progressBar in progressBars)
        {
            if (!progressBar.GetComponent<ProgressBarController>().IsFull())
                return;
        }

        // Trigger new level or end the game
        Debug.Log("Level Completed!");
        InitializeLevel();
    }
}
