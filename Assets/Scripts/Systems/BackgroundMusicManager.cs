using UnityEngine;
using DG.Tweening; // For fading

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private AudioClip catEntranceClip;

    [SerializeField] private float volume = 0.1f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Required References")]
    [SerializeField] private AudioSource audioSource;


    //Properties
    public bool IsPlaying => audioSource.isPlaying;



    // ------------------- Lifecycle -------------------

    void Awake()
    {
        if (audioSource == null) { Debug.LogWarning($"[BackgroundMusicManager] {gameObject.name} missing audiosource ref"); return; }

        audioSource.clip = mainClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        // Start music if not already playing
        if (!audioSource.isPlaying)
            FadeIn();
    }

    void OnDisable()
    {
        if (audioSource != null)
            DOTween.Kill(audioSource);
    }


    // ------------------- Methods -------------------

    public void FadeIn()
    {
        if (audioSource == null || !audioSource.enabled || !audioSource.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("[BackgroundMusicManager] Tried to play a disabled or missing AudioSource.");
            return;
        }

        audioSource.DOKill(); // Ensure any previous fade is cancelled

        if (!audioSource.isPlaying)
            audioSource.Play();

        audioSource.DOFade(volume, fadeDuration);
    }

    public void FadeOut()
    {
        audioSource.DOKill(); // Cancel previous tweens to avoid overlap

        audioSource.DOFade(0f, fadeDuration)
                   .OnComplete(() => audioSource.Pause());
    }


    private void SwitchToClip(AudioClip newClip, float fadeOutDuration = 1f, float fadeInDuration = 1f)
    {
        if (audioSource == null || newClip == null)
            return;

        if (audioSource.clip == newClip)
            return; // already playing

        audioSource.DOKill();

        audioSource.DOFade(0f, fadeOutDuration).OnComplete(() =>
        {
            audioSource.clip = newClip;
            FadeIn();
        });
    }



    public void SwitchToMainBGM() // Called by cutscene signal
    {
        SwitchToClip(mainClip);
    }

    public void SwitchToCatBGM()  //Called by cutscene signal
    {
        SwitchToClip(catEntranceClip,1,0);
    }



    public void StopImmediately()  //probably not needed but will keep
    {
        audioSource.DOKill(); // Stop any ongoing fades immediately
        audioSource.Stop();
    }

    public void SetVolume(float volume)   //probably not needed but will keep
    {
        audioSource.DOKill(); // Kill fade if overriding manually
        audioSource.volume = volume;
    }
}
