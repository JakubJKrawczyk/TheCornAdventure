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

    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject LevelsPanel;

    private Button[] buttonsList;
    private Button[] LevelbuttonsList;
    [SerializeField] private GameObject[] PanelsToShow;


    private void Start()
    {
        buttonsList = buttonsPanel.GetComponentsInChildren<Button>();

        LevelbuttonsList = LevelsPanel.GetComponentsInChildren<Button>();

        foreach (var button in LevelbuttonsList)
        {
            button.interactable= false;
        }

        for (int i = 0; i <= LevelsUnlocked; i++)
        {
            LevelbuttonsList[i].interactable = true;
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

    public void ButtonClicked(int id)
    {
        float delay = (100f / 60f) - 0.5f;

        StartCoroutine(ShowOptionsWithDelay(delay, id));
    }

    private IEnumerator ShowOptionsWithDelay(float delay, int toshow)
    {
        yield return new WaitForSeconds(delay);

        ShowOptions(toshow);
    }

    public void ShowOptions(int toshow)
    {
        PanelsToShow[toshow].SetActive(true);
    }
}
