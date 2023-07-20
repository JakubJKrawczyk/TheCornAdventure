using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int CurrentHealth = 5;
    [Header("Dependencies")]
    [SerializeField] private GameObject HealthPanel;
    [SerializeField] private Animator ScreenEffectAnimator;
    [SerializeField] private GameObject GameOverPanel;

    //private script variables
    private List<GameObject> HealthIcons;
    private float _dmgCooldown = 1f;

    public void Start()
    {
        
        HealthIcons = new List<GameObject>();
        RefreshHealth();

        foreach (Transform icon in HealthPanel.transform)
        {
            HealthIcons.Add(icon.transform.GetChild(0).gameObject);
        }
    }

    public bool AddHealth(int health)
    {
        if ((CurrentHealth == 5 && health > 0) || (CurrentHealth == 0 && health < 0))
        {
            return false;
        }
        else
        {
            CurrentHealth += health;
            if (CurrentHealth > 5)
            {
                CurrentHealth = 5;
            }
             if (health > 0)
             {
                 ScreenEffectAnimator.SetTrigger("Healing");
             }
             else
             {
                 ScreenEffectAnimator.SetTrigger("Damage");
             }
            RefreshHealth();
            return true;
        }
    }
    public void RemoveHealth(int health)
    {
        CurrentHealth -= health;
        Debug.Log("Player took " + health + " damage.");
         ScreenEffectAnimator.SetTrigger("Damage");
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            GameOver();
        }
        RefreshHealth();
    }

    public void RefreshHealth()
    {
        for (int i = 0; i < HealthIcons.Count; i++)
        {
            if (i < CurrentHealth)
            {
                HealthIcons[i].SetActive(true);
            }
            else
            {
                HealthIcons[i].SetActive(false);
            }
        }
    }
    public int GetHealth()
    {
        return CurrentHealth;
    }
    public void SetHealth(int health)
    {
        CurrentHealth = health;
        RefreshHealth();
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }
}
