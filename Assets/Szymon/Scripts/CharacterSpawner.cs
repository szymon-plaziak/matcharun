using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSpawner : MonoBehaviour
{
    [Header("Lista dostępnych postaci")]
    // Tutaj w edytorze wrzucisz swoje prefaby modeli (0 = Egyptian, 1 = Roman, itd.)
    public GameObject[] characterPrefabs;

    // Używamy Awake zamiast Start, aby model pojawił się ZANIM PlayerController 
    // zacznie szukać Animatora w swoim własnym Start()
    void Awake()
    {
        // Pobieramy zapisany numer postaci (domyślnie 0, jeśli ktoś nie wybrał)
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        // Zabezpieczenie przed błędem poza tablicą
        if (selectedIndex < 0 || selectedIndex >= characterPrefabs.Length)
        {
            selectedIndex = 0;
        }

        // Spawnowanie wybranego modelu jako Dziecko obiektu Player (tego skryptu)
        Instantiate(characterPrefabs[selectedIndex], transform);
    }
}
