using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spikes : MonoBehaviour
{
    [SerializeField] private int SpikesDamage;
    [SerializeField] private string PlayerTag;
    [SerializeField] private int DamageRate;
    private Collider2D playerCollider;
    private bool hasContactContantWithPlayer = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            hasContactContantWithPlayer = true;
            playerCollider = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            hasContactContantWithPlayer = false;
            playerCollider = null;
        }
    }
    private void FixedUpdate()
    {
        if(hasContactContantWithPlayer)
        {
            StartCoroutine(MakeDamage(playerCollider));
        }
    }

    IEnumerator MakeDamage(Collider2D player)
    {
        
            player.gameObject.GetComponent<HealthController>().RemoveHealth(SpikesDamage);
            yield return new WaitForSeconds(DamageRate);
        
    }

}
