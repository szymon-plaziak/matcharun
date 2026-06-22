using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SectionTrigger : MonoBehaviour
{
    [Header("Co spawnować?")]
    public GameObject roadSection;

    [Header("Ustawienia drogi")]
    // Długość jednego segmentu drogi (musi być identyczna jak w przeszkodach i monetach!)
    [SerializeField] private float segmentStep = -57f;

    // Zmienne do perfekcyjnego układania drogi
    private float lastSpawnX;
    private float lastSpawnTime;

    void Start()
    {
        // Zakładamy, że pierwszy spawn drogi zaczyna się od zera. 
        // Skrypt zaraz po uderzeniu w trigger doda do tego -57 i tam stworzy drogę.
        lastSpawnX = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Reagujemy na ten sam trigger co reszta systemów
        if (other.gameObject.CompareTag("Trigger"))
        {
            // Ochrona przed podwójnym uderzeniem colliderów Playera
            if (Time.time - lastSpawnTime < 0.5f) return;
            lastSpawnTime = Time.time;

            // 1. Aktualizujemy idealną pozycję X (dodajemy -57)
            lastSpawnX += segmentStep;

            // 2. Składamy pozycję w całość (Y i Z wyzerowane, by droga była równo)
            Vector3 nextPosition = new Vector3(lastSpawnX, 0, 0);

            // 3. Spawniemy drogę
            Instantiate(roadSection, nextPosition, Quaternion.identity);
        }
    }
}