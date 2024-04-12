using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Salir : MonoBehaviour
{
    // Permite cerrar la aplicación
    public void SalirDelJuego()
    {
        StartCoroutine(SalirConRetraso());
    }

    private IEnumerator SalirConRetraso()
    {
        // Espera durante 1 segundo antes de cerrar la aplicación
        yield return new WaitForSeconds(1);

        // Si se está ejecutando en el editor de Unity, lo detiene
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            // Si no, entonces cierra la aplicación
            Application.Quit();
        }
    }
}
