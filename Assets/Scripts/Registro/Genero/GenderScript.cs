using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenderScript : MonoBehaviour
{
    public TextMeshProUGUI generoSeleccionado;

    public void selectedGender(string gender)
    {
        //Asigno el genero seleccionado a una variable temporal en el UI
      generoSeleccionado.text = gender;    
    }
}
