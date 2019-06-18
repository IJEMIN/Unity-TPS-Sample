using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public Transform playerTransform;
    
    private float lastSpawnTime;
    public float maxDistance = 5f;
    
    private float timeBetSpawn;

    public float timeBetSpawnMax = 7f;
    public float timeBetSpawnMin = 2f;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void Spawn()
    {

    }
}