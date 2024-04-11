using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegresarMenuPrincipal : MonoBehaviour
{
    public CambiarEscena cambiarEscena;

    public void regresarMenu()
    {
        cambiarEscena.Cambiar("Registro");
        cambiarEscena.Inicio.SetActive(false);
        cambiarEscena.MenuPrincipal.SetActive(true);
    }
}
