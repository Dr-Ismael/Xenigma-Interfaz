using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambiarEscena : MonoBehaviour
{
    public Canvas cvDesct;
    public Canvas cvAct;

    // Permite cambiar a la escena escrita en el inspector de Unity
    public void Cambiar(string escena, string cvDesactivar, string cvActivar)
    {
        SceneManager.LoadScene(escena);

        // Comprueba si se deben cambiar los canvas
        if (cvDesactivar != null && cvActivar != null)
        {
            CambiarCanvas(cvDesactivar, cvActivar);
        }
    }

    // Permite cambiar el canvas activo recibiendo de parámetros el canvas a desactivar y el canvas a activar
    public void CambiarCanvas(string cvDescStr, string cvActStr)
    {
        GameObject canvasDescGO = GameObject.Find(cvDescStr);
        if (canvasDescGO != null)
        {
            cvDesct = canvasDescGO.GetComponent<Canvas>();
        }
        else
        {
            Debug.LogError("No se pudo encontrar el Canvas con el nombre: " + cvDescStr);
            return;
        }

        GameObject canvasActGO = GameObject.Find(cvActStr);
        if (canvasActGO != null)
        {
            cvAct = canvasActGO.GetComponent<Canvas>();
        }
        else
        {
            Debug.LogError("No se pudo encontrar el Canvas con el nombre: " + cvActStr);
            return;
        }

        // Desactiva el canvas a desactivar y activa el canvas a activar
        cvDesct.enabled = false;
        cvAct.enabled = true;
    }
}