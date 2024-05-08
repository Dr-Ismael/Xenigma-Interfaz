using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConexionMySQL", menuName = "MySQL/connectionString")]
public class ConexionMySQL : ScriptableObject
{
    //Datos para la conexion a mysql
    public string Server = "";
    public int Puerto = 0;
    public string Database = "";
    public string User = "";
    public string Password = "";
    public string connectionString;

    // Método llamado cuando se crea o se modifica el ScriptableObject en el Editor de Unity.
    private void OnValidate()
    {
        // Actualizar la cadena de conexión cada vez que se modifica alguno de los campos
        connectionString =
            "Server="
            + Server
            + ";Port="
            + Puerto
            + ";Database="
            + Database
            + ";User="
            + User
            + ";Password="
            + Password
            + ";";
    }
}
