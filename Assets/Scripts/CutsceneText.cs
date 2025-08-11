using UnityEngine;
using DG.Tweening;


public class CutsceneText : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool isFinalText = false;

    [Header("Required References")]
    [SerializeField] private CanvasGroup canvasGroup;



    // ------------------- Lifecycle -------------------

    private void Awake()
    {
        canvasGroup.alpha = 0f;

        if (isFinalText && GameManager.Instance.CompletionCutsceneTriggered)
        {
            canvasGroup.alpha = 1f;
        }
    }

    void OnDisable()
    {
        if (canvasGroup != null)
            DOTween.Kill(canvasGroup);
    }


    // ------------------- Methods -------------------

    public void FadeIn()
    {
        canvasGroup.DOKill();

        // Fade in when activated by Timeline
        canvasGroup.DOFade(1f, fadeDuration);
    }

    public void FadeOut()
    {
        canvasGroup.DOKill();

        // Fade in when activated by Timeline
        canvasGroup.DOFade(0f, fadeDuration);
    }
}
