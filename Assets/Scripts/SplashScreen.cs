using UnityEngine;
using TMPro;
using DG.Tweening;

public class SplashScreen : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayTime = 2.5f;

    [Header("Required Reference")]
    [SerializeField] private TextMeshProUGUI splashText;
    [SerializeField] private CanvasGroup splashCanvas;



    // ------------------- Lifecycle -------------------

    void Awake()
    {
        if (GameManager.Instance.SplashShown)  // skip splash if not first start
        {
            gameObject.SetActive(false);
            return;
        }


        GameManager.Instance.SplashShown = true;

        if (splashCanvas == null)
            splashCanvas = GetComponent<CanvasGroup>();

        splashCanvas.alpha = 1f;
        splashText.alpha = 0f;

        splashText.DOFade(1, fadeDuration);

        DOTween.Sequence()
            .AppendInterval(displayTime)
            .Append(splashCanvas.DOFade(0f, fadeDuration))
            .OnComplete(() => gameObject.SetActive(false));
    }



    private void OnDisable()
    {
        if (splashText != null)
            DOTween.Kill(splashText);

        if (splashCanvas != null)
            DOTween.Kill(splashCanvas);
    }
}
