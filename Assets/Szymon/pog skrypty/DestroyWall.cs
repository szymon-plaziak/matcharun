using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Jeśli obiekt to nie jest Gracz...
        if (!other.CompareTag("Player"))
        {
            // root znajdzie najwyższego rodzica w hierarchii (czyli roadSection) i usunie CAŁOŚĆ.
            Destroy(other.transform.root.gameObject);
        }
    }
}