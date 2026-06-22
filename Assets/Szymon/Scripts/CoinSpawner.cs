using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Co spawnować?")]
    public GameObject coinPrefab;

    [Header("Ustawienia Linii")]
    public int coinsInLine = 5;       // Ile monet w jednym rzędzie
    public float spacingX = 2f;       // Odległość między monetami (w osi X)
    public float coinHeightY = 0.5f;  // Wysokość, na jakiej lewitują monety

    [Header("Ustawienia Pozycji")]
    // NOWOŚĆ: Długość segmentu drogi (musi być zgodna z resztą gry)
    [SerializeField] private float segmentStep = -57f;

    // O ile jednostek od początku NOWEGO segmentu mają zacząć się monety
    [SerializeField] private float spawnOffsetFromSegment = -40f;
    [SerializeField] private float laneDistance = 3f;

    // Zmienne do perfekcyjnej synchronizacji (jak w przeszkodach)
    private float lastSpawnX;
    private float lastSpawnTime;

    void Start()
    {
        lastSpawnX = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            // FIX: Ochrona przed podwójnym uderzeniem colliderów Playera
            if (Time.time - lastSpawnTime < 0.5f) return;
            lastSpawnTime = Time.time;

            SpawnCoinLine();
        }
    }

    void SpawnCoinLine()
    {
        // 1. Aktualizujemy wirtualną pozycję segmentu (np. na -57, potem -114)
        lastSpawnX += segmentStep;

        // 2. Losujemy tor dla monet: 0 (Lewo), 1 (Środek), 2 (Prawo)
        int randomLane = Random.Range(0, 3);
        float spawnZ = (randomLane - 1) * laneDistance;

        // Określamy kierunek układania monet (-1 dla osi biegnącej na minus)
        float direction = Mathf.Sign(segmentStep);

        // 3. Generujemy linię monet
        for (int i = 0; i < coinsInLine; i++)
        {
            // KLUCZOWE: Pozycja X to teraz: 
            // początek segmentu (lastSpawnX) + przesunięcie (-40) + odstępy kolejnych monet
            float coinX = lastSpawnX + spawnOffsetFromSegment + (i * spacingX * direction);

            Vector3 spawnPosition = new Vector3(coinX, coinHeightY, spawnZ);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}