using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrearClan : MonoBehaviour
{
    public InputField NombreClan_field;

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    [SerializeField] GameObject PantallaClan;
    [SerializeField] GameObject MenuPrincipal;
    [SerializeField] GameObject RegisClan;
    [SerializeField] GameObject RegisMiembrosClan;
    [SerializeField] GameObject EleccionDeJugadores;


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


    private void Start()
    {

        connectionString = "Server=sql3.freemysqlhosting.net;Port=3306;Database=sql3686159;User=sql3686159;Password=XYxplE7HS9;";
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
            string query = "insert into clanes (id, NombreClan, id_lider, puntajeClan) VALUES (null,'"+ NombreClanG +"', '"+ JalarId.resultadoID +"', 0);";

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(query, MS_Connection);

            MS_Reader = MS_Comand.ExecuteReader();

            Debug.Log("Registro exitoso");
    }

    public void regresarPagPrincipal()
    {
        RegisClan.SetActive(false);
        RegisMiembrosClan.SetActive(false);
        PantallaClan.SetActive(true);
        MenuPrincipal.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
