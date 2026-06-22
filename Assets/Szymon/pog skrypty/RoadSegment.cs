using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    [Header("Ruch Drogi")]
    public float moveSpeed = 15f; // Jak szybko droga jedzie na gracza
    public Vector3 moveDirection = new Vector3(1, 0, 0); // Zakładam +1 w osi X wg starego skryptu Move

    [Header("Generowanie Następnej Drogi")]
    public GameObject roadPrefab;
    public float segmentLength = -57f;
    private bool hasSpawnedNext = false;

    [Header("Przeszkody")]
    public GameObject[] obstaclePrefabs;
    [Range(1, 4)] public int obstaclesCount = 2;

    [Header("Monety")]
    public GameObject coinPrefab;
    public int coinsInLine = 5;
    public float spacingX = 2f;
    public float laneDistance = 3f;

    void Start()
    {
        SpawnObstacles();
        SpawnCoins();
    }

    void Update()
    {
        // Ciągłe przesuwanie całego segmentu (wraz z jego dziećmi - monetami i przeszkodami)
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void SpawnObstacles()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        float step = segmentLength / obstaclesCount;
        for (int i = 0; i < obstaclesCount; i++)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            float obstacleX = (step * i) + (step / 2f);

            // Pozycja lokalna (względem drogi)
            Vector3 localPos = new Vector3(obstacleX, 0, 0);

            // Zwróć uwagę na "transform" na końcu - to czyni przeszkodę dzieckiem drogi
            GameObject obstacle = Instantiate(obstaclePrefabs[randomIndex], transform);
            obstacle.transform.localPosition = localPos;
        }
    }

    void SpawnCoins()
    {
        if (coinPrefab == null) return;

        int randomLane = Random.Range(0, 3);
        float spawnZ = (randomLane - 1) * laneDistance;
        float startX = segmentLength * 0.3f;
        float direction = Mathf.Sign(segmentLength);

        for (int i = 0; i < coinsInLine; i++)
        {
            float coinX = startX + (i * spacingX * direction);
            Vector3 localPos = new Vector3(coinX, 0.5f, spawnZ);

            // Monety też stają się dziećmi drogi
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.transform.localPosition = localPos;
        }
    }

    public void SpawnNextSegment()
    {
        if (!hasSpawnedNext)
        {
            hasSpawnedNext = true;

            // Generujemy nową drogę relatywnie do obecnej (np. -57 jednostek od niej).
            // Dzięki temu obie drogi jadą z tą samą prędkością, idealnie sklejone.
            Vector3 nextPos = transform.position + new Vector3(segmentLength, 0, 0);

            // Przekazujemy ten sam prefab drogi
            GameObject nextRoad = Instantiate(roadPrefab, nextPos, Quaternion.identity);

            // Kopiujemy prędkość ruchu do nowej drogi, żeby jechała identycznie
            nextRoad.GetComponent<RoadSegment>().moveSpeed = this.moveSpeed;
        }
    }
}