using UnityEngine;

public class BigCat : MonoBehaviour
{
    [Header("Mandatory References")]
    [SerializeField] private Animator animator;

    [Header("SFX")]
    [SerializeField] private AudioClip purrclip;
    [SerializeField] private float volume;


    // ------------------- Lifecycle -------------------

    private void Awake()
    {
        if (GameManager.Instance.CatRevealTriggered)    //This keeps it revealed on scene load
        {
            KeepRevealed();
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnCatRevealed += KeepRevealed;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCatRevealed -= KeepRevealed;
    }


    // ------------------- Methods -------------------

    private void KeepRevealed()
    {
        animator.SetBool("IsRevealed", true);
    }

    public void PlayPurr()
    {
        GameManager.Instance.SFXManager.PlayPersistent(purrclip, volume);
    }
}
