using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private float fadeDuration = 0.5f;

    //Cache
    private CollectibleManager collectibleManager;
    private CanvasGroup canvasGroup;
    private bool isHidden = true;



    // ------------------- Lifecycle -------------------

    private void Awake()
    {
        collectibleManager = GameManager.Instance.CollectibleManager;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (GameManager.Instance.CollectibleManager.CollectedCount <= 0)
        {
            canvasGroup.alpha = 0f; //Hides it by default
            isHidden = true;
        }
    }

    void OnEnable()
    {
        if (collectibleManager != null)
        {
            collectibleManager.OnCollectibleCountChanged += ShowUI;
            collectibleManager.OnCollectibleCountChanged += UpdateUI;
            UpdateUI(); // Initialize once
        }
    }

    void OnDisable()
    {
        if (canvasGroup != null)
            DOTween.Kill(canvasGroup);

        if (collectibleManager != null)
        {
            collectibleManager.OnCollectibleCountChanged -= ShowUI;
            collectibleManager.OnCollectibleCountChanged -= UpdateUI;
        }
    }


    // ------------------- Methods -------------------

    void ShowUI() //Subbed to OnCollectibleCountChanged
    {
        if (isHidden)
        {
            canvasGroup.DOFade(1f, fadeDuration);
            isHidden = false;
        }
    }


    void UpdateUI()
    {
        int collected = collectibleManager.CollectedCount;
        int total = collectibleManager.TotalCount;
        counterText.text = $"{collected}/{total}  ideas collected";
    }

}
