using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEditor;

public class SeleccionarJugadores : MonoBehaviour
{

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    int CuantosMiembros = 0;
    public List<MostrarMiembros> MostrarMiembros = new List<MostrarMiembros>();

    public GameObject miembroPrefab;
    public Transform miembrosContenedor;
    public CrearClan TomarIDClan;

    [SerializeField] GameObject PagPrincipal;
    [SerializeField] GameObject MiembrosJuego;

    string idSeleccionados;




    void Start()
    {
        connectionString = "Server=158.97.122.147;Port=3306;Database=unity;User=root;Password=;";
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



        Pintar();
    }
    
    public void Pintar()
    {
        string query = "SELECT COUNT(*) FROM miembros_clanes WHERE idClan = '" + TomarIDClan.resultadoIDClan + "';";
        //  string query = "SELECT COUNT(*) FROM miembros_clanes WHERE idClan = 1;";
        PlayerPrefs.GetString("resultadoIDClan", TomarIDClan.resultadoIDClan);
        PlayerPrefs.Save();

        Debug.Log(query);

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);


        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            CuantosMiembros = MS_Reader.GetInt32(0);
        }
        MS_Reader.Close();

        string query2 = "SELECT * FROM miembros_clanes where idClan = '" + TomarIDClan.resultadoIDClan + "';";
        // string query2 = "SELECT * FROM miembros_clanes where idClan = 1;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query2, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombre = MS_Reader.GetString(1);
            string nickname = MS_Reader.GetString(2);
            int edad = MS_Reader.GetInt32(3);
            MostrarMiembros nuevoDato = new MostrarMiembros();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.edad = edad;
            MostrarMiembros.Add(nuevoDato);
        }
        MS_Reader.Close();

        Mostrar();
    }

    private void Mostrar()
    {
        float posY = 79.8f; // variable para llevar un seguimiento de la posición Y

        foreach (MostrarMiembros miembro in MostrarMiembros)
        {
            GameObject miembroObject = Instantiate(miembroPrefab, miembrosContenedor);

            // ajusta la posición Y del objeto utilizando la variable posY
            RectTransform rt = miembroObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

            TextMeshProUGUI nombreText = miembroObject.transform.Find("Nombre").GetComponent<TextMeshProUGUI>();
            nombreText.text = miembro.nombre;

            TextMeshProUGUI nicknameText = miembroObject.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
            nicknameText.text = miembro.nickname;

            // obtener el componente Toggle y agregar un listener para el evento onValueChanged
            Toggle toggle = miembroObject.transform.Find("Toggle").GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(miembro, toggle); });

            // aumenta el valor de posY en el espaciado deseado
            posY -= 35.8f;
        }
    }

    private void OnToggleValueChanged(MostrarMiembros miembro, Toggle toggle)
    {
        miembro.toggleEstado = toggle.isOn;
    }

    public void MostrarSeleccionados()
    {
        bool algunoSeleccionado = false;
        string seleccionados = "Miembros seleccionados: ";

        foreach (MostrarMiembros miembro in MostrarMiembros)
        {
            if (miembro.toggleEstado)
            {
                algunoSeleccionado = true;
                seleccionados += miembro.nombre + ", ";
                idSeleccionados += miembro.id + ", ";
            }
        }

        if (algunoSeleccionado)
        {
            // Quitar la última coma y espacio
            seleccionados = seleccionados.Remove(seleccionados.Length - 2);
            Debug.Log(seleccionados);

            idSeleccionados = idSeleccionados.Remove(idSeleccionados.Length - 2);
            Debug.Log(idSeleccionados);

            // Almacenar el valor de idSeleccionados
            PlayerPrefs.SetString("idSeleccionados", idSeleccionados);
            PlayerPrefs.Save();

            // Cambiar a la siguiente escena
            SceneManager.LoadScene("xenigma");
        }

        else
        {
            Debug.Log("Por favor, seleccione al menos un miembro para continuar.");
        }
    }


    public void RegresarInicio()
    {
        BorrarContenedores();
        MiembrosJuego.SetActive(false);
        PagPrincipal.SetActive(true);
    }

    private void BorrarContenedores()
    {
        // Elimina todos los miembros existentes en la lista
        foreach (Transform child in miembrosContenedor)
        {
            Destroy(child.gameObject);
        }

        MostrarMiembros.Clear();
    }
}

[System.Serializable] public class MostrarMiembros
{
    public int id;
    public string nombre;
    public string nickname;
    public int edad;
    public bool toggleEstado;
}
