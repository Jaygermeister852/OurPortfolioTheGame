using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline; // Assign your Timeline in the Inspector
    [SerializeField] private CutsceneType type;

    private PlayerController player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))   //Doesn't do anything if not player
        { return; }

        if (type == CutsceneType.DoorReveal && GameManager.Instance.DoorRevealTriggered)  //Checks the cutscene type against gamemanager bool
        { return; }

        if (type == CutsceneType.CatReveal && GameManager.Instance.CatRevealTriggered)  //Checks the cutscene type against gamemanager bool
        { return; }

        // Mark cutscene as triggered
        MarkTriggered();


        // Cache the player reference
        player = other.GetComponent<PlayerController>();

        // Freeze the player
        player.SetFrozen(true);

        // Subscribe to timeline stopped event
        timeline.stopped += OnTimelineStopped;

        // Play the cutscene
        timeline.Play();
    }



    private void MarkTriggered()
    {
        switch (type)
        {
            case CutsceneType.DoorReveal:
                {
                    GameManager.Instance.DoorRevealTriggered = true;
                    GameManager.Instance.TriggerDoorRevealedEvent();
                    break;
                }
            case CutsceneType.CatReveal:
                {
                    GameManager.Instance.CatRevealTriggered = true;
                    GameManager.Instance.TriggerCatRevealedEvent();
                    break;
                }
        }
    }



    private void OnTimelineStopped(PlayableDirector _)
    {
        // Unfreeze the player
        if (player != null)
            player.SetFrozen(false);

        // Unsubscribe to avoid memory leaks or double-calls on scene reload
        timeline.stopped -= OnTimelineStopped;
    }
}
