using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CollectibleManager _collectibleManager;
    [SerializeField] private BackgroundMusicManager _backgroundMusicManager;
    [SerializeField] private SFXManager _sfxManager;


    [Header("Auto-registered")]
    [SerializeField] private CinemachineCamera _playerCamera; //registered onAwake


    [SerializeField] private List<SpawnPoint> spawnPoints = new(); //registered onEnable
    [SerializeField] private SceneList _lastDoorID; //registered onInteract
    private GameObject _playerInstance; //registered onsceneload

    [Header("States")]
    [SerializeField] bool _doorRevealTriggered = false;
    [SerializeField] bool _catRevealTriggered = false;
    [SerializeField] bool _completionCutsceneTriggered = false;



    //Properties
    public GameObject PlayerInstance { get { return _playerInstance; }}
    public CinemachineCamera PlayerCamera { get { return _playerCamera; } }
    public CollectibleManager CollectibleManager { get { return _collectibleManager; } }
    public BackgroundMusicManager BackgroundMusicManager {  get { return _backgroundMusicManager; } }
    public SFXManager SFXManager { get { return _sfxManager; } }

    public SceneList LastDoorID { get { return _lastDoorID; } set { _lastDoorID = value; } }

    public bool DoorRevealTriggered { get { return _doorRevealTriggered; } set { _doorRevealTriggered = value; } }
    public bool CatRevealTriggered { get { return _catRevealTriggered; } set { _catRevealTriggered = value; } }
    public bool CompletionCutsceneTriggered { get { return _completionCutsceneTriggered; } set { _completionCutsceneTriggered = value; } }


    //Cache
    public static GameManager Instance;


    //Event
    public event Action OnDoorRevealed; //Keeps the door revealed after cutscene
    public event Action OnCatRevealed; //Keeps the cat revealed after cutscene
    public event Action OnCompletion;



    // ------------------- LifeCycle -------------------

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists between scenes
            SceneManager.sceneLoaded += OnSceneLoaded; // listen for scene changes
        }

        else
        {
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayer();
    }



    // ------------------- Spawn Point Registrar -------------------
    public void RegisterSpawnPoint(SpawnPoint sp)
    {
        if (!spawnPoints.Contains(sp))
            spawnPoints.Add(sp);
    }

    public void UnregisterSpawnPoint(SpawnPoint sp)
    {
        spawnPoints.Remove(sp);
    }



    // ------------------- Camera Registrar -------------------

    public void RegisterPlayerCamera(CinemachineCamera camera)
    {
        _playerCamera = camera;
    }


    // ------------------- Player Spawning -------------------
    private void SpawnPlayer()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points registered!");
            return;
        }

        // 1. Match LastDoorID first
        SpawnPoint currentSpawnPoint = spawnPoints.Find(sp => sp.SpawnID == LastDoorID);

        // 2. Fallback to Default
        if (currentSpawnPoint == null)
            currentSpawnPoint = spawnPoints.Find(sp => sp.SpawnID == SceneList.DefaultSpawnPoint);

        if (currentSpawnPoint == null)
        {
            currentSpawnPoint = FindAnyObjectByType<SpawnPoint>();
            Debug.LogWarning($"No matching spawn point ! Spawning at {currentSpawnPoint}.");
        }


        // Spawn or move player
        if (_playerInstance != null)
            _playerInstance.transform.position = currentSpawnPoint.transform.position;
        else
            _playerInstance = Instantiate(playerPrefab, currentSpawnPoint.transform.position, Quaternion.identity);


        //Register player as camera tracking target
        if (_playerCamera != null)
        {
            _playerCamera.Follow = _playerInstance.transform;
        }
    }

    // ------------------- Event Trigger -------------------

    public void TriggerDoorRevealedEvent()
    {
        OnDoorRevealed?.Invoke(); // fire event once

    }

    public void TriggerCatRevealedEvent()
    {
        OnCatRevealed?.Invoke(); // fire event once
    }

    public void TriggerCompletionEvent()
    {
        OnCompletion?.Invoke();  // fire event once
    }
}
