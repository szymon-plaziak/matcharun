using UnityEngine;

public class RoadSectionController : MonoBehaviour
{
    public GameObject[] obstacleLayouts;
    public Transform endPoint;

    void Start()
    {
        foreach (GameObject layout in obstacleLayouts)
        {
            layout.SetActive(false);
        }

        if (obstacleLayouts.Length > 0)
        {
            int randomIndex = Random.Range(0, obstacleLayouts.Length);
            obstacleLayouts[randomIndex].SetActive(true);
        }
    }
}