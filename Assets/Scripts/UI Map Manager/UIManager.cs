using System.Collections;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    //Variables de conexion
    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    //Listas de datos del usuario y de avatares
    public List<DatosUsuario> DatosUsuario = new List<DatosUsuario>();
    public List<Lista> Avatares = new List<Lista>();

    public Image imgAvatarUsuario;

    public TextMeshProUGUI txtNicknameUsuario;

    void Start()
    {
        connectionString = "Server=localhost;Port=3306;Database=Xenigmabd;User=XenigmaJuego;Password=OHfoUIt[gt7uHWJS;";
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
            string queryInfo = "SELECT nickname, idAvatar FROM users WHERE id = '" + idUser + "';";

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

            MS_Reader = MS_Comand.ExecuteReader();
            while (MS_Reader.Read())
            {
                // Recupera los datos del usuario y asígnalos a variables
                string nickname = MS_Reader.GetString(0);
                int idAvatar = MS_Reader.GetInt32(1);

                // Busca la imagen correspondiente en la lista de objetos
                Lista imagenSeleccionada = Avatares.Find(imagen => imagen.ID == idAvatar);

                if (imagenSeleccionada != null)
                {
                    Debug.Log("Avatar del miembro: " + imagenSeleccionada.ID);
                    imgAvatarUsuario.sprite = imagenSeleccionada.Image.sprite; // Asigna el sprite de la imagen seleccionada a infoMemberImage
                }
                else
                {
                    Debug.Log("No se encontró la imagen seleccionada");
                }

                txtNicknameUsuario.text = nickname;

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
    public string nickname;
    public int idAvatar;
}
