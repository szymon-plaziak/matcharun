using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Ustawienia Menu")]
    [SerializeField, Tooltip("Nazwa sceny z grą (Scena 2)")]
    private string gameplaySceneName = "Gameplay";

    // Domyślnie wybrana postać to 0
    private int selectedCharacterIndex = 0;

    private void Start()
    {
        // Odczytanie poprzedniego wyboru, jeśli istnieje
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
    }

    /// <summary>
    /// Metoda podpinana pod przyciski wyboru postaci (np. OnClick w Canvasie).
    /// </summary>
    /// <param name="index">Indeks postaci (0, 1, 2)</param>
    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
        PlayerPrefs.Save();
        Debug.Log($"Wybrano postać o indeksie: {selectedCharacterIndex}");
    }

    /// <summary>
    /// Metoda podpinana pod przycisk "Start".
    /// </summary>
    public void StartGame()
    {
        // Upewniamy się, że wybór jest zapisany przed załadowaniem sceny
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacterIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameplaySceneName);
    }
}