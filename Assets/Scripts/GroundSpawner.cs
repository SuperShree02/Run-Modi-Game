using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject firstTile; // The first tile that spawns only once
    public GameObject[] groundTile; // Array of tiles to spawn randomly
    public float zSpawn = 0; // Starting z-position for spawning tiles
    public float tileLength = 30; // Length of each tile
    public int numberOfTiles = 3; // Number of tiles to keep active
    public Transform player; // Reference to the player
    private List<GameObject> activeTiles = new List<GameObject>(); // List to track active tiles

    void Start()
    {
        // Spawn the first tile explicitly
        SpawnFirstTile();

        // Spawn additional tiles randomly
        for (int i = 1; i < numberOfTiles; i++)
        {
            SpawnTile(Random.Range(0, groundTile.Length));
        }
    }

    void Update()
    {
        // Spawn new tiles as the player progresses
        if (player.position.z - 65 > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, groundTile.Length));
            DeleteTiles();
        }
    }

    // Spawns the first tile at the starting position
    private void SpawnFirstTile()
    {
        GameObject go = Instantiate(firstTile, transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    // Spawns a random tile from the groundTile array
    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(groundTile[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }

    // Deletes the oldest tile to manage memory
    private void DeleteTiles()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
