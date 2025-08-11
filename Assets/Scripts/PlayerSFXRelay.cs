using UnityEngine;

public class PlayerSFXRelay : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] walkClips;       // Played during footstep animation
    [SerializeField] private AudioClip jumpClip;       // Played on jump
    [SerializeField] private AudioClip landClip;


    //Cache
    private GameManager gameManager;



    // ------------------- Lifecycle -------------------

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }



    // ------------------- Methods -------------------


    // Called from Animation Event
    public void PlayFootstep()
    {
        AudioClip selectedClip = walkClips[Random.Range(0, walkClips.Length)];  //Picks a random clip out of all options

        gameManager.SFXManager.PlayInterruptible(selectedClip);

    }


    public void PlayJump()
    {
        gameManager.SFXManager.PlayInterruptible(jumpClip);
    }


    public void PlayLand()
    {
        gameManager.SFXManager.PlayPersistent(landClip);
    }
}