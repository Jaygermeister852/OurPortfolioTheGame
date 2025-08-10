using UnityEngine;
using System;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
    private HashSet<CollectibleID> collected = new HashSet<CollectibleID>();
    private HashSet<CollectibleID> allCollectibles = new HashSet<CollectibleID>();


    //Properties
    public int CollectedCount => collected.Count;
    public int TotalCount => allCollectibles.Count;


    //Event
    public event Action OnCollectibleCountChanged;


    // ------------------- Lifecycle -------------------

    void Awake()
    {
        // Prepopulate allCollectibles with every enum entry at startup
        foreach (CollectibleID id in System.Enum.GetValues(typeof(CollectibleID)))
        {
            allCollectibles.Add(id);
        }
    }


    // ------------------- Methods -------------------

    public bool IsCollected(CollectibleID id)
    {
        return collected.Contains(id);
    }

    public void Collect(CollectibleID id)
    {
        if (collected.Add(id))  // only fire if new
        {
            OnCollectibleCountChanged?.Invoke();
        }
    }
}
