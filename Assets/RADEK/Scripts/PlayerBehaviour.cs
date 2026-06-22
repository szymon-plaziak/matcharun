using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private Vector3 originalScale;
    private int currentLane = 1;
    private bool isGrounded;
    private bool isSliding = false;

    [SerializeField] private float forwardSpeed = 12f;
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneSwitchSpeed = 15f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float slideDuration = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0) currentLane--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < 2) currentLane++;
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)
        {
            Jump();
        }

        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isSliding)
        {
            StartCoroutine(SlideRoutine());
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, sphereCollider.bounds.extents.y + 0.1f);

        float targetZ = (currentLane - 1) * laneDistance;
        float newZ = Mathf.MoveTowards(transform.position.z, targetZ, laneSwitchSpeed * Time.fixedDeltaTime);
        float newX = transform.position.x - forwardSpeed * Time.fixedDeltaTime;

        rb.position = new Vector3(newX, rb.position.y, newZ);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    IEnumerator SlideRoutine()
    {
        isSliding = true;
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);
        yield return new WaitForSeconds(slideDuration);
        transform.localScale = originalScale;
        isSliding = false;
    }
}