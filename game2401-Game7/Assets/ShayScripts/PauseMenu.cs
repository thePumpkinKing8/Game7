using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenuScreen;

    private bool _gamePaused;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private GameObject _resumeButton;

    [SerializeField]
    private GameObject _mainMenuButton;

    [SerializeField]
    private GameObject _exitButton;


    private void Awake()
    {
        _player = GameObject.Find("Player"); // Reference to the player
        _resumeButton = GameObject.Find("Resume_Button"); // Reference to the resume button
        _mainMenuButton = GameObject.Find("MainMenu_Button"); // Reference to the main menu button
        _exitButton = GameObject.Find("Quit_Button"); // Reference to the exit button
    }

    void Start()
    {
        _pauseMenuScreen.SetActive(false); // Hides the pause menu
    }

    void Update()
    {
        if (_gamePaused)
        {
          _player?.SetActive(false); // Deactivate the Player object if it exists in the scene. Keeps the player character from recieving any inputs while the game is paused.
        }
        else if (!_gamePaused)
        {
           _player?.SetActive(true); // Activate the Player object if it exists in the scene
        }
    }

    #region Button Actions

    public void OnResumePress()
    {
        GameResume(); // Resumes the game when the Resume button is pressed.
    }

    public void OnMainMenuPress()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Loads the Main Menu scene
    }

    public void OnExitPress()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Quits the game
    }

    #endregion

    public void GamePause()
    {
        _gamePaused = true;

        OpenMenu();

        Time.timeScale = 0f; // Freezes time
        
    }

    public void GameResume()
    {
        _gamePaused = false;

        CloseMenu();

        Time.timeScale = 1f; // Resumes time
    }

    private void OpenMenu()
    {
        _pauseMenuScreen.SetActive(true); // Shows the pause menu
    }

    private void CloseMenu()
    {
        _pauseMenuScreen.SetActive(false); // Hide the pause menu
    }
}
