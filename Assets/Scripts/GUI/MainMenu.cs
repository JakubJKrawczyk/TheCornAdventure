using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int LevelsUnlocked = 3;
    public int LastLevel = 0;

    public string[] LevelNames;

    public void Continue()
    {
        LevelSelected(LastLevel);
    }

    public void LevelSelected(int level)
    {
        SceneManager.LoadScene(LevelNames[level]);
    }

    public void Exit()
    {
        Debug.Log("========EXIT========");
        Application.Quit();
    }
}
