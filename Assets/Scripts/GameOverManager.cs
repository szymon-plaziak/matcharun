using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using TMPro; // TextMeshPro jest aktualnym standardem w Unity
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencje UI")]
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text totalCoinsText;

    [Header("Ustawienia Scen")]
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        // Odczyt danych z PlayerPrefs
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int collectedCoins = PlayerPrefs.GetInt("CollectedCoins", 0);

        // Aktualizacja UI
        currentScoreText.text = $"Wynik: {currentScore}";
        highScoreText.text = $"Najlepszy Wynik: {highScore}";
        totalCoinsText.text = $"Zebrane Monety: {collectedCoins}";
    }

    /// <summary>
    /// Metoda podpinana pod przycisk "Zagraj Ponownie".
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    /// <summary>
    /// Metoda podpinana pod przycisk "Menu Główne".
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}