using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Niszczymy każdy obiekt (segment, monetę, przeszkodę), który wpadnie w ten trigger
        // Ważne: Gracz nie powinien tu wpaść, chyba że jego Z spadnie poniżej threshold śmierci.
        // Zabezpieczenie na wypadek dotknięcia gracza:
        if (!other.CompareTag("Player"))
        {
            // Jeśli obiekt posiada rodzica będącego segmentem (np. moneta na segmencie), 
            // niszczenie segmentu zniszczy też dzieci. 
            // Najbezpieczniej usuwać obiekt najwyższego rzędu, jeśli dotyczy to drogi.
            if (other.transform.parent != null && other.transform.parent.GetComponent<RoadManager>() != null)
            {
                Destroy(other.gameObject);
            }
            else
            {
                // Niszczy korzeń (rodzica), jeśli uderzono w element składowy (np. ścianę segmentu)
                Destroy(other.transform.root.gameObject);
            }
        }
    }
}