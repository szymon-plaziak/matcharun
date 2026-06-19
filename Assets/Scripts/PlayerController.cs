using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
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

    // Oryginalna skala i collider do zmniejszania podczas slide'u
    private Vector3 originalScale;
    private SphereCollider sphereCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        originalScale = transform.localScale;
    }

    void Update()
    {
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
        // Sprawdzamy, czy kulka dotyka ziemi
        isGrounded = Physics.Raycast(transform.position, Vector3.down, sphereCollider.bounds.extents.y + 0.1f);

        // Obliczanie docelowej pozycji Z (zamieniliśmy X na Z)
        // 0 = Lewo, 1 = Środek, 2 = Prawo
        // Jeśli naciśniesz w lewo, gracz przesunie się w ujemną stronę osi Z
        float targetZ = (currentLane - 1) * laneDistance;

        // Obliczamy wektor przesunięcia na boki (teraz celujemy w oś Z)
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, targetZ);
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, laneSwitchSpeed * Time.fixedDeltaTime);

        // Przypisujemy pozycję: 
        // X - zostaje nienaruszone (lub kontrolowane przez ruch do przodu)
        // Y - zostaje dla grawitacji Rigidbody
        // Z - płynnie zmienia się na nową linię
        rb.position = new Vector3(transform.position.x, rb.position.y, newPosition.z);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;

        // Wizualne i fizyczne zmniejszenie kulki (spłaszczenie), aby przeszła pod przeszkodą
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);

        yield return new WaitForSeconds(slideDuration);

        // Powrót do normalnych wymiarów
        transform.localScale = originalScale;
        isSliding = false;
    }

    // 4. WYKRYWANIE KOLIZJI Z PRZESZKODAMI (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Uderzenie w przeszkodę! Tutaj wywołaj GameManager.");
            // Przykładowe wywołanie: GameManager.instance.GameOver();
        }
    }
}