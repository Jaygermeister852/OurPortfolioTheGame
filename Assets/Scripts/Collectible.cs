using UnityEngine;
using DG.Tweening; // DOTween for fade

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private CollectibleID collectibleID;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("SFX")]
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private float volume = 1f;


    //Cache
    GameManager gameManager;
    private Collider2D col;




    // ------------------- Lifecycle -------------------
    private void Awake()
    {
        gameManager = GameManager.Instance;
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        // Hide if already collected
        if (gameManager.CollectibleManager.IsCollected(collectibleID))
            gameObject.SetActive(false);
    }

    void OnDisable()
    {
        // Safety: kill any tweens on the SpriteRenderer if object is disabled mid-fade
        if (spriteRenderer != null)
            DOTween.Kill(spriteRenderer);
    }




    // ------------------- Methods -------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Register collection
        gameManager.CollectibleManager.Collect(collectibleID);

        // Disable collider to prevent multiple triggers
        if (col != null)
            col.enabled = false;

        // Fade out safely
        if (spriteRenderer != null)
        {
            spriteRenderer.DOKill(); // stop any old tweens
            spriteRenderer.DOFade(0f, fadeDuration)
                          .OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            // fallback if no sprite
            gameObject.SetActive(false);
        }


        gameManager.SFXManager.PlayPersistent(pickupClip, volume); //Plays the audio clip

        Debug.Log($"Collected {collectibleID}! {gameManager.CollectibleManager.CollectedCount}/{gameManager.CollectibleManager.TotalCount}");
    }


}
