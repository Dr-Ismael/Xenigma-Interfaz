using System.Collections;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Variables de conexion
    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    public TextMeshProUGUI txtClanUsuario;

    public ConexionMySQL conexionMySQL;

    private void Start()
    {
        connectionString = conexionMySQL.connectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        
        try
        {
            connection.Open();
            Debug.Log("Conexión exitosa");
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error en la conexión: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }

        cargarInfoUsuario();
    }

    public void cargarInfoUsuario()
    {
        string idUserString = PlayerPrefs.GetString("idUser", "0"); // Obtener el valor como string
        int idUser;

        if (int.TryParse(idUserString, out idUser))
        {
            string queryInfo = "SELECT NombreClan FROM clanes WHERE id_lider = '" + idUser + "';";

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

            MS_Reader = MS_Comand.ExecuteReader();
            while (MS_Reader.Read())
            {
                // Recupera los datos del usuario y asígnalos a variables
                string nombreClan = MS_Reader.GetString(0);

                txtClanUsuario.text = nombreClan;
            }
            MS_Reader.Close();
        }
        else
        {
            Debug.Log("Error al parsear el id del usuario");
        }
    }
}

[System.Serializable]
public class DatosUsuario
{
    public int id;
    public string clanName;
}
