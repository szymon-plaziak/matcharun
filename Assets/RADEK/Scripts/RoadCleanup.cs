using UnityEngine;

public class RoadCleanUp : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float destroyDistance = 60f;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (transform.position.x > playerTransform.position.x + destroyDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}