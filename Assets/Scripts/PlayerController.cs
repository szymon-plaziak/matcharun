using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    [Header("Ruch na boki (Lanes)")]
    // 0 = Lewo, 1 = Środek, 2 = Prawo
    private int currentLane = 1;
    [SerializeField] private float laneDistance = 3f; // Odległość między liniami
    [SerializeField] private float laneSwitchSpeed = 10f; // Jak szybko gracz zmienia linię

    [Header("Skok i Slide")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float slideDuration = 1f;
    private bool isGrounded;
    private bool isSliding = false;

    [Header("Stan Gracza")]
    private bool isDead = false;

    // Referencje do colliderów i zapis ich oryginalnych wymiarów
    private SphereCollider sphereCollider;
    private BoxCollider boxCollider;

    private Vector3 originalSphereCenter;
    private float originalSphereRadius;

    private Vector3 originalBoxCenter;
    private Vector3 originalBoxSize;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        sphereCollider = GetComponent<SphereCollider>();
        boxCollider = GetComponent<BoxCollider>();

        if (sphereCollider != null)
        {
            originalSphereCenter = sphereCollider.center;
            originalSphereRadius = sphereCollider.radius;
        }

        if (boxCollider != null)
        {
            originalBoxCenter = boxCollider.center;
            originalBoxSize = boxCollider.size;
        }
    }

    void Update()
    {
        if (isDead) return;

        // TESTOWE AKTYWOWANIE ŚMIERCI KLAWISZEM 'K'
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
            return;
        }

        // 1. WYBÓR LINII (Sterowanie lewo/prawo)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0) currentLane--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < 2) currentLane++;
        }

        // 2. SKOK (Strzałka w górę lub W)
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)
        {
            Jump();
        }

        // 3. SLIDE (Strzałka w dół lub S)
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isSliding)
        {
            StartCoroutine(SlideRoutine());
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, sphereCollider.bounds.extents.y + 0.1f);

        float targetZ = (currentLane - 1) * laneDistance;

        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, targetZ);
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, laneSwitchSpeed * Time.fixedDeltaTime);

        rb.position = new Vector3(transform.position.x, rb.position.y, newPosition.z);
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

        if (sphereCollider != null)
        {
            sphereCollider.radius = originalSphereRadius * 0.5f;
            sphereCollider.center = new Vector3(originalSphereCenter.x, originalSphereCenter.y * 0.5f, originalSphereCenter.z);
        }

        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(originalBoxSize.x, originalBoxSize.y * 0.5f, originalBoxSize.z);
            boxCollider.center = new Vector3(originalBoxCenter.x, originalBoxCenter.y - (originalBoxSize.y * 0.25f), originalBoxCenter.z);
        }

        yield return new WaitForSeconds(slideDuration);

        if (sphereCollider != null)
        {
            sphereCollider.radius = originalSphereRadius;
            sphereCollider.center = originalSphereCenter;
        }

        if (boxCollider != null)
        {
            boxCollider.size = originalBoxSize;
            boxCollider.center = originalBoxCenter;
        }

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

    // 4. WYKRYWANIE KOLIZJI Z KOLCAMI (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        // Zmieniono warunek: teraz sprawdzamy tag "Spike"
        if (other.CompareTag("Spike"))
        {
            Debug.Log("Uderzenie w kolce (Spike)!");
            Die();
        }
    }
}