using UnityEngine;

public class GremSpawner : MonoBehaviour
{
    public GremDatabaseSO database;
    public GameObject gremPrefab;
    void Awake()
    {
        if (database == null)
            database = Resources.Load<GremDatabaseSO>("GremDatabaseSO");
    }
   

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SpawnGrem("Basic_Grem", new Vector3(10, 10, 0));
        SpawnGrem("Basic_Grem", new Vector3(-10, 20, 0));
        SpawnGrem("Basic_Grem", new Vector3(30, 20, 0));
        SpawnGrem("Basic_Grem", new Vector3(-50, 10, 0));
        SpawnGrem("Basic_Grem", new Vector3(10, -10, 0));
        SpawnGrem("Basic_Grem", new Vector3(-10, -20, 0));
        SpawnGrem("Basic_Grem", new Vector3(30, -24, 0));
        SpawnGrem("Basic_Grem", new Vector3(-20, -30, 0));
    }
    public void SpawnGrem(string id, Vector3 position)
    {
        GameObject go = Instantiate(gremPrefab, position, Quaternion.identity);
        Grem grem = go.GetComponent<Grem>();
        grem.database = database;
        grem.Initialize(id);
    }
}
