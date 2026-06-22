using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Sprawdzamy, czy obiekt, który w nas wszedł, ma tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Zebrano monetę! Tutaj w przyszłości dodamy punkty do wyniku.");

            // Niszczymy obiekt monety (znika ze sceny)
            Destroy(gameObject);
        }
    }
}