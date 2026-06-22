using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSegment : MonoBehaviour
{
    [Header("Generowanie Drogi")]
    public GameObject roadPrefab;
    public float segmentLength = 57f; // Długość Twojego segmentu
    private bool hasSpawnedNext = false; // Gwarantuje, że wygeneruje kolejną drogę tylko raz

    [Header("Generowanie Przedmiotów")]
    public bool spawnItems = true; // Czy na tej drodze mają być przeszkody? (Przydatne na start)

    [Header("Przeszkody")]
    public GameObject[] obstaclePrefabs;
    [Range(1, 4)] public int obstaclesCount = 2; // Ile przeszkód na jednym segmencie?

    [Header("Monety")]
    public GameObject coinPrefab;
    public int coinsInLine = 5;
    public float spacingX = 2f;
    public float laneDistance = 3f;

    void Start()
    {
        // Od razu po załadowaniu tego kawałka drogi, układamy na nim przeszkody i monety
        // (jeśli pole spawnItems jest zaznaczone)
        if (spawnItems)
        {
            SpawnObstacles();
            SpawnCoins();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Upewnij się, że Player ma na sobie tag "Player"!
        if (other.CompareTag("Player") && !hasSpawnedNext)
        {
            hasSpawnedNext = true;

            // Budujemy kolejną drogę idealnie na końcu obecnej
            Vector3 nextPos = new Vector3(transform.position.x - segmentLength, 0, 0);
            Instantiate(roadPrefab, nextPos, Quaternion.identity);

            // Optymalizacja: Niszczymy TEN (stary) kawałek drogi po 10 sekundach
            
        }
    }

    void SpawnObstacles()
    {
        if (obstaclePrefabs.Length == 0) return;

        // Obliczamy równe odstępy
        float step = segmentLength / obstaclesCount;

        for (int i = 0; i < obstaclesCount; i++)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);

            // Obliczamy pozycję na TEJ konkretnej drodze
            float obstacleX = transform.position.x - (step * i) - (step / 2f);
            Vector3 spawnPos = new Vector3(obstacleX, 0, 0);

            // Tworzymy przeszkodę jako "dziecko" tej drogi
            Instantiate(obstaclePrefabs[randomIndex], spawnPos, Quaternion.identity, transform);
        }
    }

    void SpawnCoins()
    {
        if (coinPrefab == null) return;

        int randomLane = Random.Range(0, 3);
        float spawnZ = (randomLane - 1) * laneDistance;

        // Monety pojawią się w 1/3 długości segmentu
        float startX = transform.position.x - (segmentLength * 0.3f);

        for (int i = 0; i < coinsInLine; i++)
        {
            float coinX = startX - (i * spacingX);
            Vector3 spawnPos = new Vector3(coinX, 0.5f, spawnZ);

            // Tworzymy monety jako "dzieci" tej drogi
            Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
        }
    }
}