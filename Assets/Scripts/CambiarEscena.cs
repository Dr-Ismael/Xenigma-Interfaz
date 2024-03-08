using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    //Permite cambiar a la escena escrita en el inspector de unity
    public void Cambiar(string escena)
    {
       SceneManager.LoadScene(escena);
    }
}
