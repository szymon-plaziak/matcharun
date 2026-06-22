using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [Header("UI Text Fields")]
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI collectedPointsText;

    [Header("UI Buttons")]
    public Button retryButton;
    public Button mainMenuButton;

    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";
    [SerializeField] private string mainMenuSceneName = "Menu";

    private void Start()
    {
        retryButton.onClick.AddListener(RetryGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void SetupGameOverScreen(int score, int bestScore, int points)
    {
        gameObject.SetActive(true);
        currentScoreText.text = "DYSTANS: " + score + "m";
        bestScoreText.text = "REKORD: " + bestScore + "m";
        collectedPointsText.text = "ZEBRANE: " + points;
    }

    private void RetryGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}