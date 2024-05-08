using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrearClan : MonoBehaviour
{
    public MostrarMiembrosClan FuncionesClan;

    public InputField NombreClan_field;

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    [SerializeField]
    GameObject PantallaClan;

    [SerializeField]
    GameObject MenuPrincipal;

    [SerializeField]
    GameObject RegisClan;

    [SerializeField]
    GameObject RegisMiembrosClan;

    [SerializeField]
    GameObject EleccionDeJugadores;

    private string emailG;
    private string passwordG;
    private string resultadoID;
    private string resultadoNombre;
    private string resultadoNombreClan;

    public string resultadoIDClan;

    public string TengoElIDClan;
    private string NombreClanG;
    public Login JalarId;

    public MostrarMiembrosClan TomarFuncion;

    public SeleccionarJugadores TomarOtraFuncion;

    public ConexionMySQL conexionMySQL;

    private void Start()
    {
        connectionString = conexionMySQL.connectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            Console.WriteLine("Conexión exitosa en CrearClan");
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

    public void CrearClanes()
    {
        string query = "SELECT * FROM clanes where id_lider = '" + JalarId.resultadoID + "' ;";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            resultadoID = MS_Reader.GetString(2);
            resultadoIDClan = MS_Reader.GetString(0);

            Debug.Log(resultadoID);
            TengoElIDClan = resultadoID;
        }

        MS_Reader.Close();

        if (resultadoID == JalarId.resultadoID)
        {
            Debug.Log("Tienes un clan");
            MenuPrincipal.SetActive(false);
            PantallaClan.SetActive(true);
            TomarFuncion.Pintar();
        }
        else
        {
            RegisClan.SetActive(true);
            MenuPrincipal.SetActive(false);
            Debug.Log("No tienes ningun clan");
        }
    }

    public void JugarClanes()
    {
        string query = "SELECT * FROM clanes where id_lider = '" + JalarId.resultadoID + "' ;";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            resultadoID = MS_Reader.GetString(2);
            resultadoIDClan = MS_Reader.GetString(0);
            Debug.Log(resultadoID);

            // Establecer el valor de IDClan en PlayerPrefs
            PlayerPrefs.SetString("IDClan", resultadoIDClan);
        }
        MS_Reader.Close();

        if (resultadoID == JalarId.resultadoID)
        {
            Debug.Log("Adelante puedes jugar");
            MenuPrincipal.SetActive(false);
            EleccionDeJugadores.SetActive(true);
            TomarOtraFuncion.Pintar();
        }
        else
        {
            Debug.Log("Necesitas un clan para poder jugar ¿deseas crear un clan?");
        }
    }

    public void guardartodoslosdatos()
    {
        NombreClanG = NombreClan_field.text;
        Debug.Log(NombreClanG);

        string query = "SELECT * FROM clanes where NombreClan = '" + NombreClanG + "' ;";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            resultadoNombreClan = MS_Reader.GetString(1);
            Debug.Log(resultadoNombreClan);
        }
        MS_Reader.Close();

        if (NombreClanG == resultadoNombreClan)
        {
            Debug.Log("Ya existe un clan con ese nombre");
        }
        else
        {
            GuardarClan();
        }
    }

    public void GuardarClan()
    {
        string query =
            "insert into clanes (id, NombreClan, id_lider, puntajeClan, lugaresClan, medallasClan, objetosClan, distanciaClan, hrsClan, miembros) VALUES (null,'"
            + NombreClanG
            + "', '"
            + JalarId.resultadoID
            + "', 0,0,0,0,0,0,0);";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();

        Debug.Log("Registro exitoso");

        crearLiderClan();

        //Muestra la pantalla de miembros del clan
        RegisClan.SetActive(false);
        PantallaClan.SetActive(true);
    }

    public void regresarPagPrincipal()
    {
        RegisClan.SetActive(false);
        RegisMiembrosClan.SetActive(false);
        MenuPrincipal.SetActive(true);
    }

    public void crearLiderClan()
    {
        //Recupera el id del usuario principal
        string idUser = PlayerPrefs.GetString("idUser");
        //Recupera los datos del usuario principal
        string queryUser =
            "SELECT nombre, nickname, edad, idAvatar, genero FROM users WHERE id = '"
            + idUser
            + "';";

        //Variables para guardar los datos del usuario previamente
        string nombre = "";
        string nickname = "";
        int edad = 0;
        int idAvatar = 0;
        string genero = "";
        int idClan = 0;

        //Conexion para recuperar los datos
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryUser, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y se asignan a las variables
            nombre = MS_Reader.GetString(0);
            nickname = MS_Reader.GetString(1);

            // Obtener la fecha de nacimiento desde la base de datos
            DateTime fechaNacimiento = MS_Reader.GetDateTime("edad");

            // Calcular la edad a partir de la fecha de nacimiento
            DateTime fechaActual = DateTime.Today;
            edad = fechaActual.Year - fechaNacimiento.Year;

            // Reducir la edad si aún no ha cumplido su cumpleaños este año
            if (fechaNacimiento > fechaActual.AddYears(-edad))
            {
                edad--;
            }

            idAvatar = MS_Reader.GetInt32(3);
            genero = MS_Reader.GetString(4);
        }
        MS_Reader.Close();

        //query para recuperar la id del clan recientemente creado
        string queryIdClan = "Select id FROM clanes WHERE id_lider = '" + idUser + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryIdClan, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Asigno la id del clan a la del nuevo lider
            idClan = MS_Reader.GetInt32(0);

            int resultadoIDClanStr = MS_Reader.GetInt32(0);
            resultadoIDClan = resultadoIDClanStr.ToString();
        }
        MS_Reader.Close();

        //Ingreso el nuevo lider al clan
        string queryNLider =
            "INSERT INTO `miembros_clanes` (`id_miembro`, `nombre`, `nickname`, `edad`, `idLider`, `idClan`, `idAvatar`, `puntaje`, `genero`, `lugares`, `hrs`, `medallas`, `coleccionables`, `distancia`, `Miem_Rango`) VALUES (NULL, '"
            + nombre
            + "', '"
            + nickname
            + "', '"
            + edad
            + "', '"
            + idUser
            + "', '"
            + idClan
            + "', '"
            + idAvatar
            + "', 0, '"
            + genero
            + "', 0, 0, 0, 0, 0, 'Lider');";
        FuncionesClan.sumarMiembro();

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MS_Comand = new MySqlCommand(queryNLider, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Registro de lider exitoso");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Connection.Close();

        FuncionesClan.Pintar();

        Debug.Log(
            "Lider agregado correctamente"
                + "Datos del Registrado"
                + "Nombre:"
                + " "
                + nombre
                + " "
                + "Nickname:"
                + " "
                + nickname
                + "Edad:"
                + " "
                + edad
                + "ID Lider:"
                + " "
                + idUser
                + "ID Clan:"
                + " "
                + idClan
                + "ID Avatar:"
                + " "
                + idAvatar
        );
    }
}
