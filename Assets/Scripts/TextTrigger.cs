using UnityEngine;
using DG.Tweening; // DOTween

public class TextTrigger : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private bool isTriggered;
    [SerializeField] private bool isUntriggered;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("References")]
    [SerializeField] private CanvasGroup textToTrigger;  // Drag your FloatingText CanvasGroup here



    // ------------------- Lifecycle -------------------

    void Awake()
    {
        if (isTriggered && textToTrigger != null)
            textToTrigger.alpha = 0f; // Ensure hidden at start
    }

    void OnDisable()
    {
        if (textToTrigger != null)
            DOTween.Kill(textToTrigger);
    }


    // ------------------- Methods -------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered)
        {
            if (other.CompareTag("Player") && textToTrigger != null)
            {
                textToTrigger.DOKill(); // Stop any existing tweens
                textToTrigger.DOFade(1f, fadeDuration); // Fade in
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (isUntriggered)
        {
            if (other.CompareTag("Player") && textToTrigger != null)
            {
                textToTrigger.DOKill();
                textToTrigger.DOFade(0f, fadeDuration); // Fade out
            }
        }
    }
}
