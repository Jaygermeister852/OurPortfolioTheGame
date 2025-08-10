using UnityEngine;
using UnityEngine.Video;
using DG.Tweening; // DOTween


public class VideoControlPanel : MonoBehaviour, IInteractable
{
    [Header("Parameters")]
    [SerializeField] private float promptFadeDuration = 0.5f;

    [Header("Required References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CanvasGroup inputPrompt;

    [SerializeField] private GameObject initialOverlay;
    [SerializeField] private GameObject pauseOverlay;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite pausedSprite;
    [SerializeField] private Sprite unpausedSprite;

    [Header("SFX")]
    [SerializeField] private AudioClip uiClickClip;    // Played when interacting with UI (video panel etc.)
    [SerializeField] private float volume = 0.5f;

    //Cache
    private GameManager gameManager;

    // ------------------- Lifecycle -------------------
    private void Awake()
    {
        gameManager = GameManager.Instance;

        if (inputPrompt != null)
        {
            inputPrompt.alpha = 0; // Hide by default
        }
    }


    void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.started += OnVideoStarted;
            videoPlayer.loopPointReached += OnVideoEnded;
        }
    }


    void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.started -= OnVideoStarted;
            videoPlayer.loopPointReached -= OnVideoEnded;
        }


        DOTween.Kill(gameObject); // Kill all tweens on this object

        if (inputPrompt != null)
        { 
            DOTween.Kill(inputPrompt);
        }
    }



    // ------------------- Methods -------------------
    public void Interact(InteractType type)
    {
        gameManager.SFXManager.PlayInterruptible(uiClickClip, volume);

        switch (type)
        {
            case InteractType.Primary:
                if (videoPlayer.isPlaying)
                {
                    videoPlayer.Pause();
                    pauseOverlay.SetActive(true);
                    spriteRenderer.sprite = pausedSprite;

                    gameManager.BackgroundMusicManager.FadeIn(); // Resume BG music
                }
                else
                {
                    videoPlayer.Play();
                    pauseOverlay.SetActive(false);
                    initialOverlay.SetActive(false);
                    spriteRenderer.sprite = unpausedSprite;

                    gameManager.BackgroundMusicManager.FadeOut(); // Pause BG music
                }
                break;


            case InteractType.Secondary:
                videoPlayer.time = 0;
                spriteRenderer.sprite = unpausedSprite;
                break;
        }
    }


    // ------------------- Music Control -------------------

    private void OnVideoStarted(VideoPlayer vp) //Subbed to video events
    {
        gameManager.BackgroundMusicManager.FadeOut();
    }

    private void OnVideoEnded(VideoPlayer vp)
    {
        gameManager.BackgroundMusicManager.FadeIn();

        spriteRenderer.sprite = pausedSprite;  //Switches control panel sprite

        initialOverlay.SetActive(true);  //Brings back overlay
    }



    // ------------------- Input Prompt -------------------

    public void ShowPrompt(bool show)
    {
        if (inputPrompt != null && show)
        {
            inputPrompt.DOKill(); // Stop any existing tweens
            inputPrompt.DOFade(1f, promptFadeDuration); // Fade in
        }


        if (inputPrompt != null && !show)
        {
            inputPrompt.DOKill(); // Stop any existing tweens
            inputPrompt.DOFade(0f, promptFadeDuration); // Fade in
        }
    }
}
