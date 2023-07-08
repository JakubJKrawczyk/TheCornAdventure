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

    public void AddHealth(int health)
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
    }
    public void RemoveHealth(int health)
    {
        CurrentHealth -= health;
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
        
    }
}
