using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class BigCat : MonoBehaviour
{
    [Header("Mandatory References")]
    [SerializeField] private Animator animator;

    [Header("SFX")]
    [SerializeField] private AudioClip purrclip;
    [SerializeField] private float volume;

    [Header("Completion Cutscene Ideas")]
    [SerializeField] private SpriteRenderer[] ideas;
    [SerializeField] private float fadeDuration = 0.5f;
    private int ideasRevealedCount = 0;


    // ------------------- Lifecycle -------------------

    private void Awake()
    {
        if (GameManager.Instance.CatRevealTriggered)    //This keeps it revealed on scene load
        {
            KeepRevealed();
        }


        //Ideas sprite logic
        if (GameManager.Instance.CompletionCutsceneTriggered)    //This keeps all ideas revealed on scene load
        {
            ShowAllIdeas();
        }
        else
        {
            HideAllIdeas();
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnCatRevealed += KeepRevealed;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCatRevealed -= KeepRevealed;


        // Kill any active tweens on each idea sprite (safety)
        foreach (var idea in ideas)
        {
            if (idea != null)
                DOTween.Kill(idea);
        }
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


    // ------------------- Completion Cutscene -------------------

    public void RevealIdea()
    {
        if (ideasRevealedCount >= ideas.Length) return;

        var idea = ideas[ideasRevealedCount];
        if (idea != null)
        {
            idea.DOKill();
            idea.DOFade(1f, fadeDuration);
        }

        ideasRevealedCount++;
    }


    private void HideAllIdeas()  //Only for awake
    {
        // Make sure all ideas start invisible
        foreach (var idea in ideas)
        {
            if (idea != null)
                idea.color = new Color(idea.color.r, idea.color.g, idea.color.b, 0f);
        }
    }


    private void ShowAllIdeas()  //Only for awake, to keep it persistent
    {
        // Make sure all ideas start Visible
        foreach (var idea in ideas)
        {
            if (idea != null)
                idea.color = new Color(idea.color.r, idea.color.g, idea.color.b, 1f);
        }
    }
}
