using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(GremWander))] 
public class Grem : MonoBehaviour
{
    public string gremID;                     
    public GremDatabaseSO database;          

    private SpriteRenderer spriteRenderer;
    private GremWander wanderBehavior;
    public bool isMenuGrem = false;

    public void Initialize(string id)
    {
        gremID = id;

        if (database == null)
        {
            Debug.LogError("GremDatabaseSO not assigned!");
            return;
        }

        GremData data = database.GetGremById(gremID);
        if (data == null)
        {
            Debug.LogError($"No grem found in database with ID {gremID}");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        wanderBehavior = GetComponent<GremWander>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = data.defaultSprite;
            spriteRenderer.color = data.tint;
        }

        foreach (var b in data.behaviors)
        {
            if (b.behaviorName == "GremWander")
            {
                wanderBehavior.Initialize(b, data);
            }
            else
            {
                Type behaviorType = Type.GetType(b.behaviorName);
                if (behaviorType == null)
                {
                    Debug.LogWarning($"Behavior class '{b.behaviorName}' not found!");
                    continue;
                }

                MonoBehaviour newBehavior = (MonoBehaviour)this.gameObject.AddComponent(behaviorType);

                var method = behaviorType.GetMethod("Initialize");
                if (method != null)
                    method.Invoke(newBehavior, new object[] { b });
            }
        }
    }
}
