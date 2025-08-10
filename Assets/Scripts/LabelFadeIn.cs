using UnityEngine;
using DG.Tweening;

public class LabelFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (GameManager.Instance.DoorRevealTriggered) //Keep visible if cutscene triggered
        {
            canvasGroup.alpha = 1;
        }
        else //start invisible
        {
            canvasGroup.alpha = 0;
        }
    }


    void OnDisable()
    {
        if (canvasGroup != null)
            DOTween.Kill(canvasGroup);
    }



    public void FadeIn()
    {
        // Fade in when activated by Timeline
        canvasGroup.DOFade(1f, fadeDuration);
    }
}
