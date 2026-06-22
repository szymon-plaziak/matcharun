using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia Ruchu")]
    [SerializeField] private float laneDistance = 2.5f;
    [SerializeField] private float sideMovementSpeed = 10f;
    [SerializeField] private float recoverySpeed = 1f; // Prędkość powrotu do Z=0 po lekkim uderzeniu

    [Header("Warunki Przegranej")]
    [SerializeField] private float minimumZThreshold = -4.0f;

    private Rigidbody rb;
    private Animator animator;

    // 0 = Lewy, 1 = Środek, 2 = Prawy
    private int currentLane = 1;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // Zabezpieczenie przed niekontrolowanym obrotem przez fizykę
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (isDead) return;

        HandleInput();
        CheckDeathCondition();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        MovePlayer();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentLane--;
            if (currentLane < 0) currentLane = 0;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentLane++;
            if (currentLane > 2) currentLane = 2;
        }
    }

    private void MovePlayer()
    {
        // Obliczamy docelową pozycję na osi X
        float targetX = (currentLane - 1) * laneDistance;

        // Używamy MoveTowards dla stałej, równej prędkości przejścia między pasami
        float step = sideMovementSpeed * Time.fixedDeltaTime;
        float newX = Mathf.MoveTowards(rb.position.x, targetX, step);

        // Odzyskiwanie pozycji Z (gdy przeszkoda nas zepchnie)
        float newZ = rb.position.z;
        if (newZ < 0)
        {
            newZ = Mathf.MoveTowards(newZ, 0f, recoverySpeed * Time.fixedDeltaTime);
        }

        // Aplikujemy nową pozycję, zostawiając naturalne Y (aby grawitacja nadal działała)
        rb.MovePosition(new Vector3(newX, rb.position.y, newZ));
    }

    private void CheckDeathCondition()
    {
        // Jeśli przeszkody zepchną gracza poniżej progu na osi Z
        if (transform.position.z < minimumZThreshold)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        // Wyłączenie wpływu fizyki, żeby gracz nie spadał dalej w nicość po śmierci
        rb.isKinematic = true;

        GameManager.Instance.EndGame();
    }
}