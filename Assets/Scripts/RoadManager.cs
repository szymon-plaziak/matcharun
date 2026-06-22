using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Header("Ustawienia Segmentów Drogi")]
    [SerializeField] private GameObject[] segmentPrefabs; // 4 warianty (A, B, C, D)
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float segmentLength = 20f;
    [SerializeField] private int initialSegmentsCount = 5;

    [Header("Ustawienia Monet")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject specialCoinPrefab;
    [SerializeField] private int coinsPerSegment = 3;

    private List<GameObject> activeSegments = new List<GameObject>();
    private float spawnZ = 0f;
    private int lastPrefabIndex = -1;

    // Licznik do spawnowania specjalnej monety (co 20-sta)
    private static int globalCoinSpawnCounter = 0;

    private void Start()
    {
        // Inicjalne spawnowanie drogi
        for (int i = 0; i < initialSegmentsCount; i++)
        {
            SpawnSegment();
        }
    }

    private void Update()
    {
        MoveSegments();
    }

    private void MoveSegments()
    {
        // Przesuwanie wszystkich aktywnych segmentów do tyłu (ujemne Z)
        for (int i = 0; i < activeSegments.Count; i++)
        {
            activeSegments[i].transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // Jeśli ostatni dodany segment zbliżył się wystarczająco, spawnujemy nowy
        // Oznaczamy offset, żeby utrzymać ciągłość niezależnie od klatek
        if (activeSegments[activeSegments.Count - 1].transform.position.z < spawnZ - segmentLength)
        {
            SpawnSegment();
        }
    }

    private void SpawnSegment()
    {
        int prefabIndex = GetRandomSegmentIndex();

        // Obliczenie pozycji nowego segmentu
        Vector3 spawnPosition = new Vector3(0, 0, spawnZ);

        GameObject newSegment = Instantiate(segmentPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
        newSegment.transform.SetParent(transform); // Organizacja w hierarchii

        SpawnCoinsOnSegment(newSegment);

        activeSegments.Add(newSegment);
        spawnZ += segmentLength;

        // Czyszczenie listy z obiektów usuniętych przez Destroyer
        activeSegments.RemoveAll(item => item == null);
    }

    private int GetRandomSegmentIndex()
    {
        if (segmentPrefabs.Length <= 1) return 0;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, segmentPrefabs.Length);
        }
        // Zapobieganie powtórzeniom tego samego prefabu z rzędu
        while (randomIndex == lastPrefabIndex);

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }

    private void SpawnCoinsOnSegment(GameObject segment)
    {
        // Prosta logika spawnu monet (można rozbudować o czytanie z listy SpawnPointów na prefabie)
        for (int i = 0; i < coinsPerSegment; i++)
        {
            globalCoinSpawnCounter++;

            GameObject prefabToSpawn = coinPrefab;

            // Co 20-sta moneta jest specjalna
            if (globalCoinSpawnCounter % 20 == 0)
            {
                prefabToSpawn = specialCoinPrefab;
            }

            // Losujemy pas dla monety (-2.5, 0, 2.5) i pozycję na długości segmentu
            float[] lanes = { -2.5f, 0f, 2.5f };
            float randomX = lanes[Random.Range(0, lanes.Length)];
            float randomZ = Random.Range(-segmentLength / 2f, segmentLength / 2f);

            Vector3 localSpawnPos = new Vector3(randomX, 1f, randomZ); // Y=1 żeby leżała na drodze

            // Spawnujemy jako dziecko segmentu, by poruszała się razem z nim
            Instantiate(prefabToSpawn, segment.transform.position + localSpawnPos, Quaternion.identity, segment.transform);
        }
    }
}