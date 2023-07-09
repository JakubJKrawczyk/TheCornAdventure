using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int CurrentHealth = 5;
    public GameObject[] HealthPanel;

    public Animator ScreenEffectAnimator;

    public void Start()
    {
        RefreshHealth();
    }


    // AddHealth can be negative, maybe some poison mushroom or smth
    public bool AddHealth(int health)
    {
        if ((CurrentHealth == 5 && health > 0) || (CurrentHealth == 0 && health < 0))
        {
            return false;       // Health won't be removed
        }
        else
        {
            CurrentHealth += health;
            if (CurrentHealth > 5)
            {
                CurrentHealth = 5;  // because Health might be able to heal more than 1
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
            return true;    // Remove Health
        }
    }

    // RemoveHealth - for enemies
    public void RemoveHealth(int health)
    {
        CurrentHealth -= health;
        ScreenEffectAnimator.SetTrigger("Damage");
        RefreshHealth();
    }

    public void RefreshHealth()
    {
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            GameOver();
        }

        for (int i = 0; i < HealthPanel.Length; i++)
        {
            if (i < CurrentHealth)
            {
                HealthPanel[i].SetActive(true);
            }
            else
            {
                HealthPanel[i].SetActive(false);
            }
        }
    }


    public void GameOver()
    {
        Debug.Log("Health = 0 GAME OVER");
       // Time.timeScale = 0.1f;
    }
}
