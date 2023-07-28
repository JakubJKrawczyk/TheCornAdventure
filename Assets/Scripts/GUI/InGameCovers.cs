using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCovers : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCover;
    [SerializeField] private GameObject PauseCover;
    [SerializeField] private GameObject PauseSettingsCover;

    private string nextSceneName;
    private bool isPaused = false;
    private AsyncOperation _asyncO;
    private Scene activeScene;
    private void Awake()
    {
        activeScene = SceneManager.GetActiveScene();
    }

    [System.Obsolete]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameOverCover.activeInHierarchy)
        {
            if(isPaused)
            {
                if (PauseSettingsCover.activeInHierarchy)
                {
                    PauseSettingsCover.SetActive(false);
                }
                else
                {
                    ResumeGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
        if (GameOverCover.activeInHierarchy)
        {
            Time.timeScale = 0;
        }


        if (_asyncO != null)
        {
            Debug.Log(_asyncO.progress);
        }
        if (_asyncO is not null && _asyncO.isDone)
        {
            Debug.Log("Skoñczy³em ³adowaæ scenê");
            Scene nextScene = SceneManager.GetSceneByName(nextSceneName);
            if (nextScene.IsValid())
            {
                Debug.Log("Zmieniam scenê");
                SceneManager.SetActiveScene(nextScene);
                _asyncO.allowSceneActivation = true;
                SceneManager.UnloadScene(activeScene.buildIndex);
                nextSceneName = null;

            }
        }

    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!GameOverCover.activeInHierarchy)
        {
            if (!hasFocus)
            {
             //   PauseGame();
            }
            else
            {
             //   ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
            PauseCover.SetActive(true);
            PauseCover.GetComponent<Animator>().Play("ButtonsPanelShowUp");
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            PauseCover.SetActive(false);
            PauseSettingsCover.SetActive(false);
        }
    }

    public void LastCheckPoint()
    {
        //todo: checkpoints
        Time.timeScale = 1f;
        
        

    }

   
       
    
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Debug.Log("Wywo³ano Load next scene");
        nextSceneName = SceneManager.GetActiveScene().name;
        _asyncO = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        _asyncO.allowSceneActivation = false;
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Wywo³ano Load next scene");
        nextSceneName = "MainMenu";
        _asyncO = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        _asyncO.allowSceneActivation = false;
    }
}
