using System;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetDetection : MonoBehaviour
{
    private string connectionString;
    private MySqlConnection MS_Connection;
    private MySqlCommand MS_Comand;
    private MySqlDataReader MS_Reader;

    public GameObject BtnGuardar;
    public GameObject PreInfoSitio;
    public GameObject FormValoracionSito;
    public GameObject InfoText;

    int puntajeBD;
    string IDs;
    string IDclan;
    string IDMiembroPrin;
    string SitioID;

    public InputField MayorPuntaje_field,
        MasBonitos_field,
        MasSeguros_field,
        MasInteresantes_field,
        MayorActFisica_field;
    private string MayorPuntaje,
        MasBonitos,
        MasSeguros,
        MasInteresantes,
        MayorActFisica;

    public ConexionMySQL conexionMySQL;

    private void Start()
    {
        connectionString = conexionMySQL.connectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Conexión exitosa");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Error en la conexión: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public void MostrarBotonInfo()
    {
        //Recuperar Puntaje
        int Puntaje = PlayerPrefs.GetInt("Puntaje", 0);
        puntajeBD = Puntaje;
        Debug.Log("El Puntaje del Sitio es:" + puntajeBD);

        //Recuperar ID�s de los miembros seleccionados
        string idSeleccionados = PlayerPrefs.GetString("idSeleccionados", "");
        IDs = idSeleccionados;
        Debug.Log("El ID de los miembros seleccinados son: " + IDs);

        //Recuperar el ID del clan del usuario principal(Padre y/o Tutor)
        string resultadoIDClan = PlayerPrefs.GetString("IDClan");
        IDclan = resultadoIDClan;
        Debug.Log("El ID del clan es: " + IDclan);

        //Recuperar  el ID de usuario principal(Padre y/o Tutor)
        string idUsuario = PlayerPrefs.GetString("IDUsuario");
        IDMiembroPrin = idUsuario;
        Debug.Log("El ID del usuario principal es: " + IDMiembroPrin);

        //Recuperar el ID del sitio
        string IDSitio = PlayerPrefs.GetString("IDSitio");
        SitioID = IDSitio;
        Debug.Log("El ID del sitio es: " + SitioID);

        InfoText.SetActive(true);
        BtnGuardar.SetActive(true);
    }

    public void ApagarInfoText()
    {
        InfoText.SetActive(false);
    }

    private void GuardarPuntajeMiembros()
    {
        //Asignar el puntaje a los miembros de los clanes
        string query =
            "UPDATE miembros_clanes SET puntaje = puntaje + '"
            + puntajeBD
            + "' WHERE id_miembro IN ("
            + string.Join("", IDs.Split(' '))
            + ") AND idClan = '"
            + IDclan
            + "';";
        Debug.Log(query);

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        try
        {
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Puntaje Actualizado Correctamente");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Reader.Close();
    }

    private void InsertarClanes()
    {
        //Asignar el puntaje al clan
        string query =
            "UPDATE clanes SET puntajeClan = puntajeClan + '"
            + puntajeBD
            + "' WHERE id = '"
            + IDclan
            + "';";
        Debug.Log(query);

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        try
        {
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Puntaje Actualizado Correctamente");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Reader.Close();
    }

    private void InsertarMiembroPrin()
    {
        //Asignar el puntaje al usuario principal(Padre y/o Tutor)
        string query =
            "UPDATE users SET puntajeUser = puntajeUser + '"
            + puntajeBD
            + "' WHERE id = '"
            + IDMiembroPrin
            + "';";
        Debug.Log(query);

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        try
        {
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Puntaje Actualizado Correctamente");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Reader.Close();
    }

    public void Valoracion()
    {
        if (
            MayorPuntaje_field.text == ""
            || MasBonitos_field.text == ""
            || MasBonitos_field.text == ""
            || MasSeguros_field.text == ""
            || MasInteresantes_field.text == ""
            || MayorActFisica_field.text == ""
        )
        {
            Debug.Log("Debe de llenar todo el formulario");
        }
        else
        {
            MayorPuntaje = MayorPuntaje_field.text;
            MasBonitos = MasBonitos_field.text;
            MasSeguros = MasSeguros_field.text;
            MasInteresantes = MasInteresantes_field.text;
            MayorActFisica = MayorActFisica_field.text;

            string query =
                "INSERT INTO `valoracion` (`id_valoracion`, `mayor_puntaje`, `mas_visitaados`, `mas_bonitos`, `mas_seguros`, `mas_interesantes`, `actividad_fisica`, `idSitio`, `idUser`) VALUES (NULL, '"
                + MayorPuntaje
                + "', '1', '"
                + MasBonitos
                + "', '"
                + MasSeguros
                + "', '"
                + MasInteresantes
                + "', '"
                + MayorActFisica
                + "', '"
                + SitioID
                + "', '"
                + IDMiembroPrin
                + "');";
            Debug.Log(query);

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Reader = MS_Comand.ExecuteReader();

            try
            {
                MS_Comand = new MySqlCommand(query, MS_Connection);
                MS_Comand.ExecuteNonQuery();
                Debug.Log("Valoracion actualizada");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            MS_Reader.Close();

            GuardarPuntajeMiembros();
            InsertarClanes();
            InsertarMiembroPrin();
            SceneManager.LoadScene("xenigma");
        }

        //Consulta para sacar la valoracion del sitio
        //SELECT SUM(mayor_puntaje) / COUNT(*) AS mayor_puntaje FROM valoracion WHERE idSitio = 1;

        //Consulta para tomar valoracion de igual o mayor a 3
        //SELECT * FROM valoracion WHERE mayor_puntaje >= 3;
    }

    public void EmpezarVuforia()
    {
        PreInfoSitio.SetActive(false);
    }

    public void RegresaarMapa()
    {
        SceneManager.LoadScene("xenigma");
    }

    public void RegresarDescrip()
    {
        PreInfoSitio.SetActive(true);
    }

    public void IrInsertarDatosValoracion()
    {
        FormValoracionSito.SetActive(true);
    }
}
