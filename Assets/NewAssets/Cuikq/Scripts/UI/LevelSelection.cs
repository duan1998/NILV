using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject[] levelButtons;
    int currentLevel;

    void Start()
    {
        currentLevel = 0;
        ShowLevel(currentLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelManager.Instance.LoadLevel(currentLevel + 1);
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            ShowNextLevel();
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            ShowPreviousLevel();
        }
    }

    public void ShowNextLevel()
    {
        if (currentLevel < 0 || currentLevel >= levelButtons.Length - 1)
        {
            return;
        }
        currentLevel++;
        ShowLevel(currentLevel);
    }

    public void ShowPreviousLevel()
    {
        if (currentLevel <= 0 || currentLevel >= levelButtons.Length)
        {
            return;
        }
        currentLevel--;
        ShowLevel(currentLevel);
    }

    void ShowLevel(int level)
    {
        if(level < 0 || level >= levelButtons.Length)
        {
            return;
        }

        foreach (GameObject go in levelButtons)
        {
            go.SetActive(false);
        }
        levelButtons[level].SetActive(true);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }
}
