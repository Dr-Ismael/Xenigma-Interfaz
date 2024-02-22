using System.Dynamic;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Ranking : MonoBehaviour
{

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    [SerializeField] GameObject PagPrincipal;
    [SerializeField] GameObject Rankingpag;

    public List<GuardarDatosMiembrosRanking> GuardarDatosMiembrosRanking = new List<GuardarDatosMiembrosRanking>();

    public GameObject miembroPrefab;
    public GameObject clanPrefab;
    public Transform miembrosContenedor;

    public GameObject[] BotonesClan;
    public GameObject[] BotonesMiembros;

    void Start()
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

    public void PintarMayorPuntajeMiembrosClanes()
    {
        BorrarContenedores();

        for (int i = 0; i < BotonesMiembros.Length; i++)
        {
            BotonesMiembros[i].SetActive(true);
            BotonesClan[i].SetActive(false);
        }


        //Consulta para acomodar de mayor a menor los miembros del clan en el ranking de puntuacion
        string query = "SELECT * FROM miembros_clanes ORDER BY puntaje DESC;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombre = MS_Reader.GetString(1);
            string nickname = MS_Reader.GetString(2);
            int puntajeMiembro = MS_Reader.GetInt32(7);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.puntajeMiembro = puntajeMiembro;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        MostrarMayorPuntajeMiembrosClanes();
    }

    public void MostrarMayorPuntajeMiembrosClanes()
    {
        // Elimina todos los miembros existentes en la lista
        foreach (Transform child in miembrosContenedor)
        {
            Destroy(child.gameObject);
        }

        float posY = 15f; // variable para llevar un seguimiento de la posici�n Y

        foreach (GuardarDatosMiembrosRanking miembro in GuardarDatosMiembrosRanking)
        {
            GameObject miembroObject = Instantiate(miembroPrefab, miembrosContenedor);

            // ajusta la posici�n Y del objeto utilizando la variable posY
            RectTransform rt = miembroObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

            TextMeshProUGUI nombreText = miembroObject.transform.Find("Nombre").GetComponent<TextMeshProUGUI>();
            nombreText.text = miembro.nombre;

            TextMeshProUGUI nicknameText = miembroObject.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
            nicknameText.text = miembro.nombre;

            TextMeshProUGUI puntajeText = miembroObject.transform.Find("PuntajeMiembro").GetComponent<TextMeshProUGUI>();
            puntajeText.text = miembro.puntajeMiembro.ToString();

            // aumenta el valor de posY en el espaciado deseado
            posY -= 35.8f;
        }
    }

    public void PintarMayorPuntajeClanes()
    {
        BorrarContenedores();

        for (int i = 0; i < BotonesClan.Length; i++)
        {
            BotonesMiembros[i].SetActive(false);
            BotonesClan[i].SetActive(true);
        }

        //Consulta para acomodar de mayor a menor los clanes en el ranking de puntuacion
        string query = "SELECT * FROM clanes ORDER BY puntajeClan DESC;;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombreClan = MS_Reader.GetString(1);
            int puntajeClan = MS_Reader.GetInt32(3);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombreClan;
            nuevoDato.puntajeClan = puntajeClan;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        MostrarMayorPuntajeClanes();
    }

    public void MostrarMayorPuntajeClanes()
    {
        // Elimina todos los miembros existentes en la lista
        foreach (Transform child in miembrosContenedor)
        {
            Destroy(child.gameObject);
        }

        float posY = 15f; // variable para llevar un seguimiento de la posici�n Y

        foreach (GuardarDatosMiembrosRanking miembro in GuardarDatosMiembrosRanking)
        {
            GameObject miembroObject = Instantiate(clanPrefab, miembrosContenedor);

            // ajusta la posici�n Y del objeto utilizando la variable posY
            RectTransform rt = miembroObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

            TextMeshProUGUI nombreText = miembroObject.transform.Find("NombreClan").GetComponent<TextMeshProUGUI>();
            nombreText.text = miembro.nombre;

            TextMeshProUGUI edadText = miembroObject.transform.Find("PuntajeClan").GetComponent<TextMeshProUGUI>();
            edadText.text = miembro.puntajeClan.ToString();

            // aumenta el valor de posY en el espaciado deseado
            posY -= 35.8f;
        }
    }

    private void BorrarContenedores()
    {
        // Elimina todos los miembros existentes en la lista
        foreach (Transform child in miembrosContenedor)
        {
            Destroy(child.gameObject);
        }

        GuardarDatosMiembrosRanking.Clear();
    }

    public void IrRanking()
    {
        Rankingpag.SetActive(true);
        PagPrincipal.SetActive(false);
        PintarMayorPuntajeClanes();
    }

    public void RegresarMenu()
    {
        BorrarContenedores();
        Rankingpag.SetActive(false);
        PagPrincipal.SetActive(true);
    }

}

[System.Serializable]
public class GuardarDatosMiembrosRanking
{
    public int id;
    public string nombre;
    public string nickname;
    public int puntajeClan;
    public int puntajeMiembro;
}
