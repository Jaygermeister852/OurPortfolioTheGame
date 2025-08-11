using UnityEngine;
using DG.Tweening;

public class BusinessCard : MonoBehaviour, IInteractable
{
    [Header("Parameters")]
    [SerializeField] private float cardFadeDuration = 0.5f;
    [SerializeField] private float promptFadeDuration = 0.5f;
    [SerializeField] private float overlayFadeDuration = 0.3f;

    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private CanvasGroup inputPrompt;   // optional
    [SerializeField] private CanvasGroup overlay;       // the UI you want to show/hide

    //Cache
    private bool isOpen = false;


    // ------------------- Lifecycle -------------------

    private void Awake()
    {
        if (inputPrompt != null)
            inputPrompt.alpha = 0;

        if (overlay != null)
        {
            // start hidden + non-interactive
            overlay.alpha = 0f;
            overlay.blocksRaycasts = false;
            overlay.interactable = false;
        }


        if (GameManager.Instance.CompletionCutsceneTriggered)
        {
            ShowCard();
        }
        else 
        {
            HideCard();
        }
    }

    private void OnDisable()
    {
        DOTween.Kill(gameObject);

        if (inputPrompt != null)
            DOTween.Kill(inputPrompt);

        if (overlay != null)
            DOTween.Kill(overlay);

        // Safety: unfreeze player if we get disabled while open
        if (isOpen)
            UnfreezePlayer();
    }


    // ------------------- Revealing logic -------------------

    public void ShowCard() //Called by timeline
    {
        sprite.DOKill();
        sprite.DOFade(1f, cardFadeDuration);

        inputPrompt.gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);
    }

    private void HideCard()  //Should only be used on awake
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        inputPrompt.gameObject.SetActive(false);
        overlay.gameObject.SetActive(false);
    }

    // ------------------- IInteractable -------------------

    public void Interact(InteractType type)
    {
        if (type != InteractType.Primary) return;

        if (isOpen)
            CloseOverlay();
        else
            OpenOverlay();
    }

    public void ShowPrompt(bool show)
    {
        if (inputPrompt == null) return;

        inputPrompt.DOKill();
        inputPrompt.DOFade(show ? 1f : 0f, promptFadeDuration);
    }



    // ------------------- Methods -------------------

    private void OpenOverlay()
    {
        if (overlay == null) return;

        isOpen = true;
        FreezePlayer();

        inputPrompt.gameObject.SetActive(false);

        overlay.DOKill();
        overlay.blocksRaycasts = true;
        overlay.interactable = true;
        overlay.DOFade(1f, overlayFadeDuration);
    }

    private void CloseOverlay()
    {
        if (overlay == null) return;

        isOpen = false;
        UnfreezePlayer();

        inputPrompt.gameObject.SetActive(true);

        overlay.DOKill();
        overlay.DOFade(0f, overlayFadeDuration)
               .OnComplete(() =>
               {
                   overlay.blocksRaycasts = false;
                   overlay.interactable = false;
               });
    }



    private void FreezePlayer()
    {
        var player = GameManager.Instance?.PlayerInstance;
        if (player != null)
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.SetFrozen(true);
        }
    }


    private void UnfreezePlayer()
    {
        var player = GameManager.Instance?.PlayerInstance;
        if (player != null)
        {
            var pc = player.GetComponent<PlayerController>();
            if (pc != null) pc.SetFrozen(false);
        }
    }
}
