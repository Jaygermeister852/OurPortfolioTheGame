using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEditor; // DOTween


public class Door : MonoBehaviour, IInteractable
{
    [Header("Parameters")]
    [SerializeField] private float promptFadeDuration = 0.5f;
    [SerializeField] private bool isLobbyDoor = false;

    [Header("Only needed if isLobbyDoor")]
    [SerializeField] private SceneList doorID; //Only needed for Lobby Doors

    [Header("Required References")]
    [SerializeField] private CanvasGroup inputPrompt;
    [SerializeField] private SceneList sceneToLoad;
    [SerializeField] private Animator animator;

    [Header("SFX")]
    [SerializeField] private AudioClip doorRevealSFX;
    [SerializeField] private AudioClip doorEnterSFX;
    [SerializeField] private float sfxVolume = 0.5f;


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

        if ( !isLobbyDoor || gameManager.DoorRevealTriggered) //Keeps the door revealed on next load once the cutscene has played. Non lobby doors are always revealed.
        {
            KeepOpen();
        }
    }

    private void OnEnable()
    {
        gameManager.OnDoorRevealed += KeepOpen;
    }

    void OnDisable()
    {
        gameManager.OnDoorRevealed -= KeepOpen;

        DOTween.Kill(gameObject); // // Safety Kill all tweens on this object

        if (inputPrompt != null)
        {
            DOTween.Kill(inputPrompt); // Kill CanvasGroup tweens
        }
    }



    // ------------------- Cutscene-related -------------------

    private void PlayOpeningSound() //Used by timeline signal, for reveal cutscene. Creates temporary audio sources.
    {
        gameManager.SFXManager.PlayPersistent(doorRevealSFX, sfxVolume);
    }

    private void KeepOpen() //Subbed to event, called by cutscenetrigger
    {
        animator.SetBool("IsOpen", true);
    }


    // ------------------- Cutscene-related -------------------


    public void Interact(InteractType type)
    {
        // Only react to primary interact
        if (type == InteractType.Primary)
        {
            // Only main page doors update the last door
            if (isLobbyDoor)
            {
                gameManager.LastDoorID = doorID;
            }

            gameManager.SFXManager.PlayPersistent(doorEnterSFX);  //Plays the persistent SFX

            SceneManager.LoadScene(sceneToLoad.ToString());  //Loads next scene
        }
    }

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
