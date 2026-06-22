using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    private RoadSegment parentSegment;

    void Start()
    {
        parentSegment = GetComponentInParent<RoadSegment>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Gdy ten trigger jadący razem z drogą wpadnie na gracza...
        if (other.CompareTag("Player") && parentSegment != null)
        {
            parentSegment.SpawnNextSegment();
        }
    }
}