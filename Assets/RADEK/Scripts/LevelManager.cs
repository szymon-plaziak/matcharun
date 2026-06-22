using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public List<RoadSegment> allSegments;
    private Queue<RoadSegment> segmentQueue;
    private Vector3 nextSpawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        segmentQueue = new Queue<RoadSegment>(allSegments);
        nextSpawnPoint = allSegments[allSegments.Count - 1].endPoint.position;
    }

    public void MoveSegmentToFront()
    {
        RoadSegment segmentToMove = segmentQueue.Dequeue();

        segmentToMove.transform.position = nextSpawnPoint;
        nextSpawnPoint = segmentToMove.endPoint.position;

        segmentToMove.ActivateRandomObstacle();

        segmentQueue.Enqueue(segmentToMove);
    }
}