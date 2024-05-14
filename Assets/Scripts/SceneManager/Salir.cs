using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Si no, entonces cierra la aplicación
        Application.Quit();
    }
}
