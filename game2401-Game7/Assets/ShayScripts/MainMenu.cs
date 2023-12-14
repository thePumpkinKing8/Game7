using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool _controlsOpen = false;

    [SerializeField]
    private GameObject _playButton;

    [SerializeField]
    private GameObject _controlsButton;

    [SerializeField]
    private GameObject _exitButton;

    [SerializeField]
    private GameObject _controlsScreen;

    void Awake()
    {
        _playButton = GameObject.Find("Play_Button"); // Reference to the play button
        _controlsButton = GameObject.Find("Controls_Button"); // Reference to the controls button
        _exitButton = GameObject.Find("Exit_Button"); // Reference to the exit button
        _controlsScreen = GameObject.Find("ControlsScreen"); // Reference to the controls screen
    }

    private void Start()
    {
        _controlsScreen.SetActive(false); // Hides the controls screen
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _controlsOpen == true) // After pressing escape, if the controls screen is open...
        {
            {
                _controlsScreen.SetActive(false); // Hides the controls screen
            }
        }
    }

    public void OnPlayPress()
    {
        SceneManager.LoadScene("Level1"); // Loads the first level
    }

    public void OnControlsPress()
    {
        _controlsScreen.SetActive(true); // Shows the controls screen
        _controlsOpen = true; // Flags the controls screen as being open
    }

    public void OnExitPress()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Quits the game
    }
}
