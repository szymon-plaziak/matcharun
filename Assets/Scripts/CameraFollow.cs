using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{
    // Tutaj w edytorze Unity przeciągniesz swojego gracza
    public Transform player;

    // Odległość kamery od gracza
    private Vector3 offset;

    void Start()
    {
        // Obliczamy początkowy dystans między kamerą a graczem
        offset = transform.position - player.position;
    }

    // LateUpdate jest lepsze dla kamery niż Update, 
    // ponieważ wykonuje się tuż PO ruchu gracza. Zapobiega to szarpaniu obrazu.
    void LateUpdate()
    {
        if (player != null)
        {
            // Ustawiamy pozycję kamery na pozycję gracza + stały dystans
            transform.position = player.position + offset;
        }
    }
}