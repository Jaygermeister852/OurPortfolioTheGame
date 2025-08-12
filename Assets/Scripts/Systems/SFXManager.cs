using UnityEngine;
using DG.Tweening; // Safety if you want to fade later (like volume, pitch)

public class SFXManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private AudioSource interruptibleSource; 


    // ------------------- Lifecycle -------------------

    private void OnDisable()
    {
        if (interruptibleSource != null)
            DOTween.Kill(interruptibleSource);
    }


    // ------------------- Interruptible SFX -------------------


    public void PlayInterruptible(AudioClip clip, float volume = 1)      // Used only for jump + walk — these can interrupt each other.
    {
        if (clip == null || interruptibleSource == null) { Debug.LogWarning("[SFXManager] SFX not found."); return; }

        interruptibleSource.Stop();
        interruptibleSource.clip = clip;
        interruptibleSource.volume = volume;
        interruptibleSource.Play();
    }


    // ------------------- One-Shot SFX -------------------

    public void PlayPersistent(AudioClip clip, float volume = 1)
    {
        if (clip == null) return;

        //Creates a temporary game object, assigns an audiosource to it, and adds clip.
        GameObject temp = new GameObject($"PersistentSFX_{clip.name}");
        AudioSource source = temp.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;
        source.Play();

        //Makes sure it's only destroyed when playback finished
        DontDestroyOnLoad(temp);
        Destroy(temp, clip.length);
    }
}
