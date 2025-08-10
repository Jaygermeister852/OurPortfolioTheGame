public enum InteractType { Primary, Secondary }

public interface IInteractable
{
    void Interact(InteractType type);


    // Optional: control prompt visibility
    void ShowPrompt(bool show);
}
