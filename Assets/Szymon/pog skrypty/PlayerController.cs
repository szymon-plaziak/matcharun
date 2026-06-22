using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using UnityEngine;

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    [Header("Ruch na boki (Lanes)")]
    private int currentLane = 1; // 0 = Lewo, 1 = Środek, 2 = Prawo
    [SerializeField] private float laneDistance = 3f;
    [SerializeField] private float laneSwitchSpeed = 10f;

    [Header("Skok i Slide")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float slideDuration = 1f;
    private bool isGrounded;
    private bool isSliding = false;

    [Header("Stan Gracza")]
    private bool isDead = false;

    private SphereCollider sphereCollider;
    private BoxCollider boxCollider;
    private Vector3 origSphereCenter, origBoxCenter, origBoxSize;
    private float origSphereRadius;

    // Punkt początkowy gracza, by trzymać go w miejscu na osi X
    private float startX;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        boxCollider = GetComponent<BoxCollider>();

        startX = transform.position.x; // Zapisujemy startową pozycję przód/tył

        if (sphereCollider != null) { origSphereCenter = sphereCollider.center; origSphereRadius = sphereCollider.radius; }
        if (boxCollider != null) { origBoxCenter = boxCollider.center; origBoxSize = boxCollider.size; }
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.K)) Die();

        // Wybór linii
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            if (currentLane > 0) currentLane--;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            if (currentLane < 2) currentLane++;

        // Skok
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)
            Jump();

        // Slide
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isSliding)
            StartCoroutine(SlideRoutine());
    }

    void FixedUpdate()
    {
        if (isDead) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, sphereCollider.bounds.extents.y + 0.1f);

        // Płynny ruch na boki
        float targetZ = (currentLane - 1) * laneDistance;
        Vector3 targetPosition = new Vector3(startX, transform.position.y, targetZ);
        Vector3 laneMove = Vector3.MoveTowards(transform.position, targetPosition, laneSwitchSpeed * Time.fixedDeltaTime);

        // Utrzymujemy gracza na stałym X, zmieniając tylko Z (i Y z fizyki)
        rb.MovePosition(new Vector3(startX, rb.position.y, laneMove.z));
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("jump_btn");
    }

    IEnumerator SlideRoutine()
    {
        isSliding = true;
        animator.SetTrigger("slide_btn");

        if (sphereCollider) { sphereCollider.radius = origSphereRadius * 0.5f; sphereCollider.center = new Vector3(origSphereCenter.x, origSphereCenter.y * 0.5f, origSphereCenter.z); }
        if (boxCollider) { boxCollider.size = new Vector3(origBoxSize.x, origBoxSize.y * 0.5f, origBoxSize.z); boxCollider.center = new Vector3(origBoxCenter.x, origBoxCenter.y - (origBoxSize.y * 0.25f), origBoxCenter.z); }

        yield return new WaitForSeconds(slideDuration);

        if (sphereCollider) { sphereCollider.radius = origSphereRadius; sphereCollider.center = origSphereCenter; }
        if (boxCollider) { boxCollider.size = origBoxSize; boxCollider.center = origBoxCenter; }

        isSliding = false;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("die_btn");
        rb.velocity = Vector3.zero;
        Debug.Log("Gracz zginął!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spike"))
        {
            Die();
        }
    }
}