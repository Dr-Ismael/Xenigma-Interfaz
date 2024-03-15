using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Mapbox Library
using Mapbox.Examples;
using Mapbox.Utils;
using UnityEngine.UI;
using System.Text;
using MySql.Data.MySqlClient;

public class EventPointer : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 50f;
    [SerializeField] float amplitude = 2.0f;
    [SerializeField] float frequency = 0.50f;
    [SerializeField] Text _name;
    [SerializeField] Text _name2;
    [SerializeField] Text _description;
    [SerializeField] Text _description2;
    [SerializeField] Text _dificultad;
    [SerializeField] Text _dificultad2;
    [SerializeField] Text _seguridad;
    [SerializeField] Text _seguridad2;
    [SerializeField] Text _visit;
    [SerializeField] Text _visit2;
    [SerializeField] Text _cercano;
    

    LocationStatus playerLocation;
    public Vector2d eventPos;
    public int eventID;
    public string eventName;
    public string eventDescription;
    public string ValSeguridad = "";
    public string Valdificultad = "";
    public string VisitasClanes = "";

    private string connectionString;
    private MySqlConnection MS_Connection;
    private MySqlCommand MS_Comand;
    private MySqlDataReader MS_Reader;

    public int Puntaje;

    MenuUIManager menuUIManager;

    // Start is called before the first frame update

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



        menuUIManager = GameObject.Find("Canvas").GetComponent<MenuUIManager>();
        
        // Find all objects with tag "spawneable" and set the _name text on each object
        GameObject[] spawnables = GameObject.FindGameObjectsWithTag("Spawnable");
        foreach (GameObject spawnable in spawnables) {
            spawnable.GetComponent<EventPointer>()._name = _name;
            spawnable.GetComponent<EventPointer>()._name2 = _name2;
            spawnable.GetComponent<EventPointer>()._description = _description;
            spawnable.GetComponent<EventPointer>()._description2 = _description2;
            spawnable.GetComponent<EventPointer>()._dificultad = _dificultad;
            spawnable.GetComponent<EventPointer>()._dificultad2 = _dificultad2;
            spawnable.GetComponent<EventPointer>()._seguridad = _seguridad;
            spawnable.GetComponent<EventPointer>()._seguridad2 = _seguridad2;
            spawnable.GetComponent<EventPointer>()._visit = _visit;
            spawnable.GetComponent<EventPointer>()._visit2 = _visit2;
            spawnable.GetComponent<EventPointer>()._cercano = _cercano;

        }
    }

    // Update is called once per frame
    void Update()
    {
        FloatAndRotatePointer();

        // Busca todos los objetos con la etiqueta "Spawnable"
        GameObject[] spawnables = GameObject.FindGameObjectsWithTag("Spawnable");

        // Encuentra el objeto más cercano al jugador
        float closestDistance = float.MaxValue;
        GameObject closestSpawnable = null;
        foreach (GameObject spawnable in spawnables)
        {
            float distance = Vector3.Distance(transform.position, spawnable.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpawnable = spawnable;
            }
        }

        // Si se encontró un objeto cercano, establece su nombre en el Text _cercano
        if (closestSpawnable != null)
        {
            _cercano.text = "Tu primer destino es " +  closestSpawnable.GetComponent<EventPointer>().eventName;
        }

    }

    void FloatAndRotatePointer()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude) + 15, transform.position.z);
    }

    private void OnMouseDown()
    {

        playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
        var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLat(), playerLocation.GetLocationLon());
        var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPos[0], eventPos[1]);
        var distance = currentPlayerLocation.GetDistanceTo(eventLocation);
        Debug.Log("Distance is: " + distance);
        if(distance > 70)
    {
        int randomNum = Random.Range(0, 3);
        menuUIManager.DisplayUserNotInRangePanel();

        _name2.text = eventName;
        _description2.text = eventDescription;
        
        if (randomNum == 0) {
            ValSeguridad = "Medio";
            Valdificultad = "Facil";
            VisitasClanes = "5";
        } else if (randomNum == 1) {
            Valdificultad = "Medio";
            ValSeguridad = "Dificil";
            VisitasClanes = "10";
        } else {
            Valdificultad = "Dificil";
            ValSeguridad = "Facil";
            VisitasClanes = "15";
        }
        _dificultad2.text = "" + Valdificultad;
        _seguridad2.text = "" + ValSeguridad;
        _visit2.text = "" + VisitasClanes + " clanes";

    }
    else
    {
        menuUIManager.DiplayStartEventPanel();
        _description.text = eventDescription;
        _dificultad.text = "Media";
        _name.text = eventName;
        _seguridad.text = "Baja";
        _visit.text = "" + VisitasClanes + "";

        // Crea un objeto StringBuilder
        StringBuilder sb = new StringBuilder();

        // Agrega cada oración de eventName al objeto StringBuilder con un espacio al final.
        string[] sentences = eventName.Split('.');
        foreach (string sentence in sentences)
        {
            sb.Append(sentence.Trim());
            sb.Append(". ");
        }

        // Convierte el objeto StringBuilder en una cadena usando el método ToString().
        string eventNameWithSpaces = sb.ToString();

        // Asigna la cadena resultante al Text _name.
        _name.text = eventNameWithSpaces;
        }
        PlayerPrefs.SetString("IDSitio", eventID.ToString());


        string query = "SELECT * FROM lugares WHERE id = '" + eventID + "';";

        Debug.Log(query);

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);


        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            Puntaje = MS_Reader.GetInt32(2);
        }
        MS_Reader.Close();
        Debug.Log(Puntaje);
        // Guardar el puntaje en PlayerPrefs
        PlayerPrefs.SetInt("Puntaje", Puntaje);

    }
}


        
