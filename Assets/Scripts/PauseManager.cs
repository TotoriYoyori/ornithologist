using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseScreen; // Public variable for the object to be activated during pause
    private bool isPaused = false;

    void Update()
    {
        // Check for Esc key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Метод для переключения паузы
    public void TogglePause()
    {
        // Toggle pause state
        isPaused = !isPaused;

        // If game is paused
        if (isPaused)
        {
            Time.timeScale = 0; // Set time scale to 0 to stop game time
            Debug.Log("Game Paused");

            // Activate the pause screen object if specified
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(true);
            }
        }
        // If game is unpaused
        else
        {
            Time.timeScale = 1; // Set time scale back to 1 to resume game time
            Debug.Log("Game Unpaused");

            // Deactivate the pause screen object if specified
            if (pauseScreen != null)
            {
                pauseScreen.SetActive(false);
            }
        }
    }
}
