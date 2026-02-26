using System.Collections.Generic;
using UnityEngine;

public class InfiniteWorld : MonoBehaviour
{
    public GameObject chunkPrefab;
    public int chunkSize = 16;
    public int viewRadius = 2;

    private Dictionary<Vector2Int, GameObject> chunks = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] private Transform player;

    void Update()
    {
        Vector2Int playerChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        HashSet<Vector2Int> required = new HashSet<Vector2Int>();

        for (int x = -viewRadius; x <= viewRadius; x++)
            for (int y = -viewRadius; y <= viewRadius; y++)
                required.Add(new Vector2Int(playerChunk.x + x, playerChunk.y + y));

        foreach (var coord in required)
        {
            if (!chunks.ContainsKey(coord))
            {
                Vector3 worldPos = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
                GameObject go = Instantiate(chunkPrefab, worldPos, Quaternion.identity, transform);
                chunks.Add(coord, go);
            }
        }

        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var kv in chunks)
            if (!required.Contains(kv.Key))
                toRemove.Add(kv.Key);

        foreach (var coord in toRemove)
        {
            Destroy(chunks[coord]);
            chunks.Remove(coord);
        }
    }
}