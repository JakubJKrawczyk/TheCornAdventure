using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int LevelsUnlocked = 3;
    public int LastLevel = 0;

    public SceneAsset[] Levels;

    AsyncOperation _asyncO;
    string nextSceneName;
    Scene activeScene;

    [SerializeField] private GameObject LevelsPanel;

    private Button[] LevelbuttonsList;


    private void Start()
    {
        activeScene = SceneManager.GetActiveScene();
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
        SceneManager.LoadScene(Levels[level].name);


        nextSceneName = Levels[level].name;
        _asyncO = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        _asyncO.allowSceneActivation = false;
    }

    private void Update()
    {
        if ( _asyncO is not null && _asyncO.isDone)
        {
            Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
            if (nextScene.IsValid())
            {
                SceneManager.SetActiveScene(nextScene);
                _asyncO.allowSceneActivation = true;
                SceneManager.UnloadSceneAsync(activeScene.buildIndex);
                nextSceneName = null;

            }
        }
    }
    public void Exit()
    {
        Debug.Log("========EXIT========");
        Application.Quit();
    }
}
