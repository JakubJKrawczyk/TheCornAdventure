using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCovers : MonoBehaviour
{
    [SerializeField] private GameObject GameOverCover;
    [SerializeField] private GameObject PauseCover;
    [SerializeField] private GameObject PauseSettingsCover;
    private CheckpointManager checkpointManager;

    private bool isPaused = false;

    private void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }
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
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!GameOverCover.activeInHierarchy)
        {
            if (!hasFocus)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
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
        checkpointManager.LoadLastCheckpoint();
        Time.timeScale = 1f;
        ResumeGame();

    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
