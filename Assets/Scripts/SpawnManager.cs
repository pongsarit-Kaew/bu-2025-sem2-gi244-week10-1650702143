using UnityEngine;

public class SpawnManager : MonoBehaviour
{
   public GameObject[] obstaclePrefabs;
    public Vector3 spawnPos = new Vector3(25, 0, 0);

    public float startDelay = 2;
    public float repeatRate = 2;

    private PlayerController playerController;
    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    void SpawnObstacle()
    {
        
        int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
        Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation);
    }
}

