using UnityEngine;

public class GremSpawner : MonoBehaviour
{
    public GremDatabaseSO database;
    public GameObject gremPrefab;

    void Start()
    {
        SpawnGrem("Basic_Grem", new Vector3(0, 0, 0));
        SpawnGrem("Basic_Grem2", new Vector3(0, 0, 0));
    }
    public void SpawnGrem(string id, Vector3 position)
    {
        GameObject go = Instantiate(gremPrefab, position, Quaternion.identity);
        Grem grem = go.GetComponent<Grem>();
        grem.database = database;
        grem.Initialize(id);
    }
}
