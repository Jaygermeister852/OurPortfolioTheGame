using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentTarget;
    private IInteractable lastTarget;


    // ------------------- Lifecycle -------------------

    private void Update()
    {
        DetectInteractable();

        // Show/hide prompt logic
        if (currentTarget != lastTarget)
        {
            if (lastTarget != null) lastTarget.ShowPrompt(false);
            if (currentTarget != null) currentTarget.ShowPrompt(true);

            lastTarget = currentTarget;
        }
    }



    // ------------------- Methods -------------------

    private void DetectInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactableLayer);
        currentTarget = hit ? hit.GetComponent<IInteractable>() : null;
    }



    // ------------------- Input -------------------

    // Input System callbacks
    public void OnPrimaryInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentTarget != null)
            currentTarget.Interact(InteractType.Primary);
    }

    public void OnSecondaryInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentTarget != null)
            currentTarget.Interact(InteractType.Secondary);
    }



    // ------------------- Methods -------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
