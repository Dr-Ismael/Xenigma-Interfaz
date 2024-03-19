using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salir : MonoBehaviour
{
    //Permite cerrar la aplicacion
    public void SalirDelJuego()
    {
        //Si se esta ejecutando en editor de unity, lo detiene
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            //Si no, entonces cierra la aplicación
            Application.Quit();
        }
    }
}
