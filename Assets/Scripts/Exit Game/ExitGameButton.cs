using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGameButton : MonoBehaviour
{
    // Enlaza este script al botón que cerrará el juego
    public Button exitButton;

    void Start()
    {
        // Agrega un evento de clic al botón que cerrará el juego
        exitButton.onClick.AddListener(ExitGame);
    }

    void ExitGame()
    {
        // Cierra la aplicación
        Application.Quit();
    }
}
