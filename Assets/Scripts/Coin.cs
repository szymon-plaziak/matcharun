using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private float rotationSpeed = 100f;

    private void Update()
    {
        // Wizualny obrót monety
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Informujemy GameManager (false = zwykła moneta)
            GameManager.Instance.AddCoin(false);

            // Odtwarzamy dźwięk w miejscu, żeby nie ucięło go po zniszczeniu obiektu
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}