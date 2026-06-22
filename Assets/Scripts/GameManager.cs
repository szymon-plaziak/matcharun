using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ustawienia Gracza")]
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform startPosition;

    [Header("Ustawienia Punktacji")]
    [SerializeField] private float scoreMultiplier = 10f;
    [SerializeField] private int normalCoinValue = 10;
    [SerializeField] private int specialCoinValue = 50;

    [Header("Przejście Scen")]
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private float delayBeforeGameOver = 2.0f;

    // Stan gry
    private float currentScore = 0f;
    private int collectedCoinsTotal = 0;
    private int specialCoinsCount = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        // Inicjalizacja Singletona
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (isGameOver) return;

        // Płynne naliczanie wyniku w czasie
        currentScore += Time.deltaTime * scoreMultiplier;
    }

    private void SpawnPlayer()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Zabezpieczenie przed błędem indeksu
        if (selectedIndex < 0 || selectedIndex >= playerPrefabs.Length)
            selectedIndex = 0;

        Instantiate(playerPrefabs[selectedIndex], startPosition.position, Quaternion.identity);
    }

    public void AddScore(int value)
    {
        if (isGameOver) return;
        currentScore += value;
    }

    public void AddCoin(bool isSpecial)
    {
        if (isGameOver) return;

        collectedCoinsTotal++;

        if (isSpecial)
        {
            specialCoinsCount++;
            AddScore(specialCoinValue);
        }
        else
        {
            AddScore(normalCoinValue);
        }
    }

    public void EndGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        SaveStats();
        StartCoroutine(LoadGameOverRoutine());
    }

    private void SaveStats()
    {
        int finalScore = Mathf.FloorToInt(currentScore);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
        }

        PlayerPrefs.SetInt("CurrentScore", finalScore);
        PlayerPrefs.SetInt("CollectedCoins", collectedCoinsTotal);
        PlayerPrefs.Save();
    }

    private IEnumerator LoadGameOverRoutine()
    {
        // Czekamy na zakończenie animacji śmierci
        yield return new WaitForSeconds(delayBeforeGameOver);
        SceneManager.LoadScene(gameOverSceneName);
    }
}