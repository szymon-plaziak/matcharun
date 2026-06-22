using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public Transform endPoint;
    public GameObject[] obstacleLayouts;

    public void ActivateRandomObstacle()
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