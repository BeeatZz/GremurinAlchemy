using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BehaviorConfig
{
    [Header("Name")]
    public string behaviorName;

    [Header("Wander Settings")]
    public float moveSpeed = 1.5f;
    public float wanderRadius = 2f;
    public float minMoveTime = 3f;
    public float minPauseTime = 0.8f;
    public float maxPauseTime = 2f;
    public float padding = 0.3f;

    
}

[System.Serializable]
public class GremData
{
    public string id;             
    public Sprite sprite;         
    public Color tint = Color.white;
    public BehaviorConfig[] behaviors;  
}

[CreateAssetMenu(fileName = "GremDatabaseSO", menuName = "Grems/All Grems Database")]
public class GremDatabaseSO : ScriptableObject
{
    public List<GremData> grems;

    public GremData GetGremById(string id)
    {
        return grems.Find(g => g.id == id);
    }
}