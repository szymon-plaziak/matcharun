using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    // Tę funkcję podepniesz pod przyciski UI wyboru postaci
    public void SelectCharacter(int characterIndex)
    {
        // Zapisujemy wybór gracza w pamięci (pod kluczem "SelectedCharacter")
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        PlayerPrefs.Save();

        Debug.Log("Wybrano postać nr: " + characterIndex);
    }

    // Tę funkcję podepniesz pod przycisk "Graj" (Play)
    public void StartGame()
    {
        // Wpisz dokładną nazwę swojej sceny z grą (zamiast "SampleScene")
        SceneManager.LoadScene("SampleScene 1");
    }
}
