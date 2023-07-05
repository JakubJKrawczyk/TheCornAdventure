using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public float activationDistance = 0.75f;
    private Animator leverAnimator;

    private GameObject player;

    private bool Active = false;


    public GameObject[] Recievers;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        leverAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);
            Debug.Log(distance);
            if (distance <= activationDistance)
            {
                Active = !Active;
                if (Active)
                {
                    Activated();
                }
                else
                {
                    Deactivated();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(Active);
        }
    }

    private void Activated()
    {
        leverAnimator.SetTrigger("LeverMove");
        foreach (var reciever in Recievers)
        {
            reciever.SendMessage("Switch");
        }
    }

    private void Deactivated()
    {
        leverAnimator.SetTrigger("LeverMoveDown");
        foreach (var reciever in Recievers)
        {
            reciever.SendMessage("Switch");
        }
    }
}
