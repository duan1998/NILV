using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PauseMenuState
{
    Off,
    Main,
    Help
}

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject helpInfo;
    private PauseMenuState state;

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
        helpInfo.SetActive(false);
        state = PauseMenuState.Off;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case PauseMenuState.Off:
                    Pause();
                    break;
                case PauseMenuState.Help:
                    CloseHelp();
                    break;
                case PauseMenuState.Main:
                    Resume();
                    break;
            }
        }
    }
    public void ReloadScene()
    {
        LevelManager.Instance.ReloadScene();
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        state = PauseMenuState.Main;
    }
    public void ShowHelp()
    {
        helpInfo.SetActive(true);
        state = PauseMenuState.Help;
    }
    public void CloseHelp()
    {
        helpInfo.SetActive(false);
        state = PauseMenuState.Main;
    }
    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        state = PauseMenuState.Off;
    }
    public void ReturnToMain()
    {
        LevelManager.Instance.LoadMainScene();
        Time.timeScale = 1f;
    }
}
