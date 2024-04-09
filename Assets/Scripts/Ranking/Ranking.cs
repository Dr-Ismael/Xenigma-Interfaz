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

    //Objetos para desactivar y activar botones de clanes y miembros en el ranking
    public GameObject BotonesClan;
    public GameObject BotonesMiembros;

    void Start()
    {
        //Inicio de la conexión a la base de datos de la aplicacion
        connectionString = "Server=localhost;Port=3306;Database=Xenigmabd;User=XenigmaJuego;Password=OHfoUIt[gt7uHWJS;";
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

        BotonesMiembros.SetActive(true);
        BotonesClan.SetActive(false);

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

            // ajusta la posicion Y del objeto utilizando la variable posY
            RectTransform rt = miembroObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

            TextMeshProUGUI nombreText = miembroObject.transform.Find("Nombre").GetComponent<TextMeshProUGUI>();
            nombreText.text = miembro.nombre;

            TextMeshProUGUI nicknameText = miembroObject.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
            nicknameText.text = miembro.nombre;

            TextMeshProUGUI puntajeText = miembroObject.transform.Find("DatoMiembro").GetComponent<TextMeshProUGUI>();
            puntajeText.text = miembro.puntajeMiembro.ToString();

            TextMeshProUGUI tipoDatoText = miembroObject.transform.Find("TipoDatoMiembro").GetComponent<TextMeshProUGUI>();
            tipoDatoText.text = "Pts";

            // aumenta el valor de posY en el espaciado deseado
            posY -= 210f;
        }
    }

    public void PintarMayorPuntajeClanes()
    {
        BorrarContenedores();

        BotonesMiembros.SetActive(false);
        BotonesClan.SetActive(true);

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

            TextMeshProUGUI edadText = miembroObject.transform.Find("DatoClan").GetComponent<TextMeshProUGUI>();
            edadText.text = miembro.puntajeClan.ToString();

            TextMeshProUGUI tipoDatoText = miembroObject.transform.Find("tipoDato").GetComponent<TextMeshProUGUI>();
            tipoDatoText.text = "Pts";

            // aumenta el valor de posY en el espaciado deseado
            posY -= 210f;
        }
    }

    public void recuperarLugaresClanes()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los clanes en el ranking de puntuacion
        string query = "SELECT * FROM clanes ORDER BY lugaresClan DESC;;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombreClan = MS_Reader.GetString(1);
            int lugaresClan = MS_Reader.GetInt32(4);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombreClan;
            nuevoDato.lugaresClan = lugaresClan;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingClanes(1);
    }

    public void recuperarMedallasClanes()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los clanes en el ranking de puntuacion
        string query = "SELECT * FROM clanes ORDER BY medallasClan DESC;;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombreClan = MS_Reader.GetString(1);
            int medallasClan = MS_Reader.GetInt32(5);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombreClan;
            nuevoDato.medallasClan = medallasClan;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingClanes(2);
    }

    public void recuperarColeccionablesClanes()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los clanes en el ranking de puntuacion
        string query = "SELECT * FROM clanes ORDER BY objetosClan DESC;;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombreClan = MS_Reader.GetString(1);
            int objetosClan = MS_Reader.GetInt32(6);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombreClan;
            nuevoDato.coleccionablesClan = objetosClan;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingClanes(3);
    }

    public void recuperarDistanciaClanes()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los clanes en el ranking de puntuacion
        string query = "SELECT * FROM clanes ORDER BY distanciaClan DESC;;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombreClan = MS_Reader.GetString(1);
            int distanciaClan = MS_Reader.GetInt32(7);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombreClan;
            nuevoDato.distanciaClan = distanciaClan;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingClanes(4);
    }

    public void recuperarLugaresMiembros()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los miembros del clan en el ranking de puntuacion
        string query = "SELECT * FROM miembros_clanes ORDER BY lugares DESC;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombre = MS_Reader.GetString(1);
            string nickname = MS_Reader.GetString(2);
            int lugaresMiembro = MS_Reader.GetInt32(9);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.lugaresMiembro = lugaresMiembro;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingMiembros(1);
    }

    public void recuperarMedallasMiembros()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los miembros del clan en el ranking de puntuacion
        string query = "SELECT * FROM miembros_clanes ORDER BY medallas DESC;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombre = MS_Reader.GetString(1);
            string nickname = MS_Reader.GetString(2);
            int medallasMiembro = MS_Reader.GetInt32(11);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.medallasMiembro = medallasMiembro;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingMiembros(2);
    }

    public void recuperarColeccionablesMiembros()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los miembros del clan en el ranking de puntuacion
        string query = "SELECT * FROM miembros_clanes ORDER BY coleccionables DESC;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombre = MS_Reader.GetString(1);
            string nickname = MS_Reader.GetString(2);
            int coleccionablesMiembro = MS_Reader.GetInt32(12);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.coleccionablesMiembro = coleccionablesMiembro;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingMiembros(3);
    }

    public void recuperarDistanciaMiembros()
    {
        BorrarContenedores();

        //Consulta para acomodar de mayor a menor los miembros del clan en el ranking de puntuacion
        string query = "SELECT * FROM miembros_clanes ORDER BY distancia DESC;";
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        MS_Comand = new MySqlCommand(query, MS_Connection);
        MS_Reader = MS_Comand.ExecuteReader();

        while (MS_Reader.Read())
        {
            int id = MS_Reader.GetInt32(0);
            string nombre = MS_Reader.GetString(1);
            string nickname = MS_Reader.GetString(2);
            int distanciaMiembro = MS_Reader.GetInt32(13);
            GuardarDatosMiembrosRanking nuevoDato = new GuardarDatosMiembrosRanking();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.distanciaMiembro = distanciaMiembro;
            GuardarDatosMiembrosRanking.Add(nuevoDato);
        }
        MS_Reader.Close();

        mostrarRankingMiembros(4);
    }

    public void mostrarRankingClanes(int tipoDato)
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

            TextMeshProUGUI infoText = miembroObject.transform.Find("DatoClan").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI tipoDatoText = miembroObject.transform.Find("tipoDato").GetComponent<TextMeshProUGUI>();

            switch (tipoDato)
            {
                case 1:
                    infoText.text = miembro.lugaresClan.ToString();
                    tipoDatoText.text = "Lugares";
                    break;
                case 2:
                    infoText.text = miembro.medallasClan.ToString();
                    tipoDatoText.text = "Medallas";
                    break;
                case 3:
                    infoText.text = miembro.coleccionablesClan.ToString();
                    tipoDatoText.text = "Objetos";
                    break;
                case 4:
                    infoText.text = miembro.distanciaClan.ToString();
                    tipoDatoText.text = "KM";
                    break;
                default:
                    infoText.text = "N/A"; // Para casos en los que tipoDato no coincida
                    break;
            }

            // aumenta el valor de posY en el espaciado deseado
            posY -= 210f;
        }
    }

    public void mostrarRankingMiembros(int tipoDato)
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

            // ajusta la posicion Y del objeto utilizando la variable posY
            RectTransform rt = miembroObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

            TextMeshProUGUI nombreText = miembroObject.transform.Find("Nombre").GetComponent<TextMeshProUGUI>();
            nombreText.text = miembro.nombre;

            TextMeshProUGUI nicknameText = miembroObject.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
            nicknameText.text = miembro.nombre;

            TextMeshProUGUI puntajeText = miembroObject.transform.Find("DatoMiembro").GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI tipoDatoText = miembroObject.transform.Find("TipoDatoMiembro").GetComponent<TextMeshProUGUI>();


            switch (tipoDato)
            {
                case 1:
                    puntajeText.text = miembro.lugaresMiembro.ToString();
                    tipoDatoText.text = "Lugares";
                    break;
                case 2:
                    puntajeText.text = miembro.medallasMiembro.ToString();
                    tipoDatoText.text = "Medallas";
                    break;
                case 3:
                    puntajeText.text = miembro.coleccionablesMiembro.ToString();
                    tipoDatoText.text = "Objetos";
                    break;
                case 4:
                    puntajeText.text = miembro.distanciaMiembro.ToString();
                    tipoDatoText.text = "KM";
                    break;
                default:
                    puntajeText.text = "N/A"; // Para casos en los que tipoDato no coincida
                    break;
            }

            // aumenta el valor de posY en el espaciado deseado
            posY -= 210f;
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

    public int lugaresClan;

    public int lugaresMiembro;

    public int medallasClan;

    public int medallasMiembro;

    public int coleccionablesClan;

    public int coleccionablesMiembro;

    public int distanciaClan;

    public int distanciaMiembro;
}
