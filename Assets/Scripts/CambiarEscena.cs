using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambiarEscena : MonoBehaviour
{
    //public Canvas cvDesct;
    //public Canvas cvAct;
    private string emailDEV = "luis@gmail.com";
    private string passDEV = "Aa@11111";

    // Referencias a los InputFields en el canvas
    public InputField emailInputField;
    public InputField passInputField;

    // Permite cambiar a la escena escrita en el inspector de Unity
    public void Cambiar(string escena)
    {
        SceneManager.LoadScene(escena);
    }

    //Permite iniciar sesion rapidamente (para desarrolladores)
    public void devLogin()
    {
        // Aquí se asignan los valores a las variables emailDEV y passDEV al inputfield
        emailInputField.text = emailDEV;
        passInputField.text = passDEV;

        // Puedes imprimir las variables para verificar si se asignaron correctamente
        Debug.Log("Email: " + emailInputField.text);
        Debug.Log("Contraseña: " + passInputField.text);
    }
}
