using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BirthdaryScript : MonoBehaviour
{
    public TMP_Dropdown dropdownYears;
    public TextMeshProUGUI txtDiaSeleccionado, txtMesSeleccionado, txtAnioSeleccionado, txtBirthday;
    
    void Start()
    {
        // Llama al método para agregar opciones al Dropdown
        AgregarAniosAlDropdown();
    }

    void AgregarAniosAlDropdown()
    {
        // Limpia las opciones actuales del Dropdown
        dropdownYears.ClearOptions();

        // Crea una lista para almacenar las opciones
        TMP_Dropdown.OptionDataList options = new TMP_Dropdown.OptionDataList();

        // Agrega las opciones al Dropdown
        for (int year = 1900; year <= System.DateTime.Now.Year; year++)
        {
            options.options.Add(new TMP_Dropdown.OptionData(year.ToString()));
        }

        // Establece las opciones en el Dropdown
        dropdownYears.options = options.options;

        // Encuentra el índice de la opción correspondiente al año 2000
        int defaultIndex = 2000 - 1900;

        // Establece la opción predeterminada seleccionando el índice correspondiente
        dropdownYears.value = defaultIndex;
    }

    public void getBirthday()
    {
      //Variable para guardar el cumpleaños
      string userBirthday="";
      //Recojo los valores ingresados por el usuario
      userBirthday = ""+txtDiaSeleccionado.text+"/"+txtMesSeleccionado.text+"/"+txtAnioSeleccionado.text;
      Debug.Log(userBirthday);
      
      //Establezco el cumpleaños del usuario en la pantalla de registro
      txtBirthday.text = userBirthday;

    }
}
