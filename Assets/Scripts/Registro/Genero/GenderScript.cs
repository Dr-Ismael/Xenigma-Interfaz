using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenderScript : MonoBehaviour
{
    //Variable que permite mostrar el genero seleccionado en la pantalla de registro
    public TextMeshProUGUI generoSeleccionado;

    //Variable que permite mostrar el genero seleccionado al registrar un nuevo miembro de clan
    public TextMeshProUGUI memberGeneroSeleccionado;

    public void selectedGender(string gender)
    {
      //Asigno el genero seleccionado a una variable temporal en el UI
      generoSeleccionado.text = gender;    
    }

    //Permite a los miembros de el clan creados elegir su genero
    public void memberSelectedGender(string memberGender)
    {
      memberGeneroSeleccionado.text = memberGender;
    }
}
