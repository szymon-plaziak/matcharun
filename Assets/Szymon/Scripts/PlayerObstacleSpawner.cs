using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObstacleSpawner : MonoBehaviour
{
    [Header("Lista segmentów z przeszkodami")]
    public GameObject[] obstacleSections;

    [Header("Ustawienia spawnowania")]
    // Całkowita długość jednego segmentu drogi (np. 57)
    [SerializeField] private float segmentStep = -57f;

    // NOWOŚĆ: Ile przeszkód chcesz na jednym segmencie drogi? 
    // Jeśli ustawisz 2, przeszkody pojawią się co 28.5 jednostki!
    [Range(1, 5)]
    [SerializeField] private int obstaclesPerSegment = 2;

    private float lastSpawnX;
    private float lastSpawnTime; // Ochrona przed podwójnym uderzeniem colliderów

    void Start()
    {
        lastSpawnX = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            // FIX: Jeśli od ostatniego spawnu minęło mniej niż 0.5 sekundy - zignoruj!
            // Blokuje to podwójne wywołanie przez Twoje dwa collidery na Playerze.
            if (Time.time - lastSpawnTime < 0.5f) return;
            lastSpawnTime = Time.time;

            if (obstacleSections.Length > 0)
            {
                // Obliczamy dystans między przeszkodami 
                // (np. -57 / 2 = -28.5 odstępu między nimi)
                float step = segmentStep / obstaclesPerSegment;

                // Generujemy zadaną ilość przeszkód
                for (int i = 0; i < obstaclesPerSegment; i++)
                {
                    int randomIndex = Random.Range(0, obstacleSections.Length);

                    lastSpawnX += step;
                    Vector3 spawnPosition = new Vector3(lastSpawnX, 0f, 0f);

                    Instantiate(obstacleSections[randomIndex], spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}