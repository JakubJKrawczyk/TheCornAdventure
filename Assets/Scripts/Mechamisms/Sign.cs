using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour
{
   [SerializeField] private string hint;
    [SerializeField] private string PlayerTag;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Canvas hintCanvas;
    private void Awake()
    {

        hintText.text = hint;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            hintCanvas.gameObject.SetActive(true);
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTag))
        {
            hintCanvas.gameObject.SetActive(false);
            
        }
    }
}
