using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("GameObjects storages")]
    public List<GameObject> Enemies;
    public List<SceneAsset> ScenesInAssets = new List<SceneAsset>();
    public GameObject Player;
    public GameObject PlayerA;
    public Image Cinematic;
    public Image UI;

    AsyncOperation _asyncO;
    string nextSceneName;
    Scene activeScene;
    private void Awake()
    {
        activeScene = SceneManager.GetActiveScene();

    }

    public void ChangeDisplay(int index)
    {
        PlayerPrefs.SetInt("UnitySelectMonitor", index);
    }
    
    public void SwitchUI()
    {
        Cinematic.gameObject.SetActive(!Cinematic.isActiveAndEnabled);
        UI.gameObject.SetActive(!UI.isActiveAndEnabled);
    }
    public void SwitchPlayer()
    {
        Player.SetActive(!Player.activeSelf);
        PlayerA.SetActive(!PlayerA.activeSelf);
        if(Player.activeSelf)
        {
            Player.GetComponent<PlayerMovement2D>().AllowMovement();
        }
        else
        {
            Player.GetComponent<PlayerMovement2D>().DenyMove();

        }
    }

    public void LoadNextScene()
    {
        Debug.Log("Wywo³ano Load next scene");
        int indexOfCurrentScene = ScenesInAssets.IndexOf(ScenesInAssets.First(s => s.name == SceneManager.GetActiveScene().name));
        if(ScenesInAssets.Count > indexOfCurrentScene +1) {
            nextSceneName = ScenesInAssets[indexOfCurrentScene + 1].name;
            _asyncO = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
            _asyncO.allowSceneActivation = false;
        }
    }



    private void Update()
    {
        if(_asyncO != null)
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
                SceneManager.UnloadSceneAsync(activeScene.buildIndex);
                nextSceneName = null;
                
            }
        }
    }

}
