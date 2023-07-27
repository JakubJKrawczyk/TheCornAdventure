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

    [SerializeField] private GameObject LevelsPanel;

    private Button[] LevelbuttonsList;


    private void Start()
    {

        LevelbuttonsList = LevelsPanel.GetComponentsInChildren<Button>();

        foreach (var button in LevelbuttonsList)
        {
            button.interactable= false;
        }

        for (int i = 0; i <= LevelsUnlocked; i++)
        {
            LevelbuttonsList[i].interactable = true;
            LevelbuttonsList[i].image.color= Color.white;
        }
    }

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
