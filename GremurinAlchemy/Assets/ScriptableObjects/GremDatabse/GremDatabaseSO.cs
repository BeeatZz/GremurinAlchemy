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
    [Header("General Info")]
    public string id;
    public bool isItem = false;
    public Sprite defaultSprite;
    public Color tint = Color.white;

    [Header("Behaviors")]
    public BehaviorConfig[] behaviors;

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip walkAnimation;
    public AnimationClip clickAnimation;

    [Header("Optional Sound")]
    public AudioClip clickSound;
}

[CreateAssetMenu(fileName = "GremDatabaseSO", menuName = "Grems/Grem Database")]
[System.Serializable]
public class GremDatabaseSO : ScriptableObject
{
    public List<GremData> grems;

    public GremData GetGremById(string id) //s
    {
        return grems.Find(g => g.id == id);
    }

    public bool IsItem(string id)
    {
        GremData data = GetGremById(id);
        return data != null && data.isItem;
    }

    public bool IsGrem(string id)
    {
        GremData data = GetGremById(id);
        return data != null && !data.isItem;
    }
}
