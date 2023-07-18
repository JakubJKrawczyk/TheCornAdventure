using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    [SerializeField] private CharacterController2D playerController;
    [SerializeField] private PlatformEffector2D platformEffector;

    
    private Collider2D platformCollider;

   
    void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        platformCollider = GetComponent<Collider2D>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerController = collision.gameObject.GetComponent<CharacterController2D>();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerController == null)
            return;      
        if (playerController.isCrouching && IsPlayerOnPlatform())
        {
            AllowFallThrough(true);
        }
        else
        {
            AllowFallThrough(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerController = null;
        AllowFallThrough(false);
    }
    public bool IsPlayerOnPlatform()
    {
        if (platformCollider == null)
            return false;
        return platformCollider.IsTouchingLayers(LayerMask.GetMask("Player")); 
    }

    public void AllowFallThrough(bool allow)
    {
        platformEffector.rotationalOffset = allow ? 180 : 0;
    }
}
