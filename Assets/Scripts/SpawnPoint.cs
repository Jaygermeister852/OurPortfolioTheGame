using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private SceneList _spawnID;

    //Properties
    public SceneList SpawnID { get { return _spawnID; } }


    // ------------------- Lifecycle -------------------



    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterSpawnPoint(this);
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.UnregisterSpawnPoint(this);
    }
}
