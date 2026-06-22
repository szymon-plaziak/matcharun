using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectTrigger : MonoBehaviour
{
    public GameObject[] roadSections;
    public Transform initialSpawnPoint;
    private Vector3 nextSpawnPosition;

    void Start()
    {
        if (initialSpawnPoint != null)
        {
            nextSpawnPosition = initialSpawnPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            other.enabled = false;

            if (roadSections.Length > 0)
            {
                int randomIndex = Random.Range(0, roadSections.Length);
                GameObject newRoad = Instantiate(roadSections[randomIndex], nextSpawnPosition, Quaternion.identity);

                RoadSectionController controller = newRoad.GetComponent<RoadSectionController>();
                if (controller != null && controller.endPoint != null)
                {
                    nextSpawnPosition = controller.endPoint.position;
                }
            }
        }
    }
}