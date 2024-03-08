﻿using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class SpawnOnMap : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    [Geocode]
    string[] _locationStrings;
    Vector2d[] _locations;
    [SerializeField] Text _list;
    [SerializeField] Text _lugares;

    [SerializeField]
    float _spawnScale = 100f;


    [SerializeField]
    GameObject _markerPrefab;
    [SerializeField] private Dropdown _dropdown;
    [SerializeField] private Dropdown _dropdown2;

    List<GameObject> _spawnedObjects;

    // Variables para los filtros
    bool _showParks = false;
    bool _showMuseums = false;
    bool _showStatues = false;
    bool _showHistoric = false;
    bool _showObj = true;


    private int selectedEventID = -1;
    private const string SpawnedObjectsKey = "SpawnedObjects";

    private string activeFilters = "";

    private void Start()
    {
        _locations = new Vector2d[_locationStrings.Length];
        _spawnedObjects = new List<GameObject>();
        var options = new List<Dropdown.OptionData>();

        //LoadSpawnedObjects();

        // Inicializar los objetos y las ubicaciones
        for (int i = 0; i < _locationStrings.Length; i++)
        {
            var locationString = _locationStrings[i];
            _locations[i] = Conversions.StringToLatLon(locationString);
            var instance = Instantiate(_markerPrefab);
            // Configurar los componentes del objeto
            instance.GetComponent<EventPointer>().eventPos = _locations[i];
            instance.GetComponent<EventPointer>().eventID = i + 1;
            instance.GetComponent<EventPointer>().eventName = GetObjectName(i + 1);
            instance.GetComponent<EventPointer>().eventDescription = GetObjectDescription(i + 1);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(instance);

            options.Add(new Dropdown.OptionData(GetObjectName(i + 1)));
        }

        // Configurar las opciones del primer Dropdown
        _dropdown.ClearOptions();
        _dropdown.AddOptions(options);
        _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // Configurar las opciones del segundo Dropdown
        _dropdown2.ClearOptions();
        _dropdown2.AddOptions(options);
        _dropdown2.onValueChanged.AddListener(DropdownValue);
    }


    private void SaveSpawnedObjects()
    {
        // Lista para almacenar los datos de los objetos spawnados
        var spawnedObjectsData = new List<string>();

        // Iterar a través de cada objeto spawnado
        foreach (var spawnedObject in _spawnedObjects)
        {
            // Verificar si el objeto todavía existe y no ha sido destruido
            if (spawnedObject != null && spawnedObject.gameObject != null)
            {
                // Obtener la posición, rotación y escala del objeto
                var position = spawnedObject.transform.position;
                var rotation = spawnedObject.transform.rotation;
                var scale = spawnedObject.transform.localScale;

                // Crear una cadena de datos que contiene la posición, rotación y escala del objeto
                var data = $"{position.x},{position.y},{position.z}|{rotation.x},{rotation.y},{rotation.z},{rotation.w}|{scale.x},{scale.y},{scale.z}";

                // Agregar los datos del objeto a la lista
                spawnedObjectsData.Add(data);
            }
        }

        // Convertir la lista de datos de objetos a una cadena separada por punto y coma
        var serializedData = string.Join(";", spawnedObjectsData);

        // Guardar los datos en la memoria del dispositivo utilizando PlayerPrefs
        PlayerPrefs.SetString(SpawnedObjectsKey, serializedData);
    }

    private void LoadSpawnedObjects()
    {
        // Verificar si hay datos guardados en PlayerPrefs bajo la clave SpawnedObjectsKey
        if (PlayerPrefs.HasKey(SpawnedObjectsKey))
        {
            // Obtener los datos serializados de PlayerPrefs
            var serializedData = PlayerPrefs.GetString(SpawnedObjectsKey);

            // Dividir los datos en una matriz de cadenas usando el delimitador ';'
            var spawnedObjectsData = serializedData.Split(';');

            // Recorrer cada dato de objeto
            foreach (var data in spawnedObjectsData)
            {
                // Dividir los datos del objeto en partes: posición, rotación y escala
                var parts = data.Split('|');
                var positionParts = parts[0].Split(',');
                var rotationParts = parts[1].Split(',');
                var scaleParts = parts[2].Split(',');

                // Convertir las partes en vectores y cuaterniones
                var position = new Vector3(float.Parse(positionParts[0]), float.Parse(positionParts[1]), float.Parse(positionParts[2]));
                var rotation = new Quaternion(float.Parse(rotationParts[0]), float.Parse(rotationParts[1]), float.Parse(rotationParts[2]), float.Parse(rotationParts[3]));
                var scale = new Vector3(float.Parse(scaleParts[0]), float.Parse(scaleParts[1]), float.Parse(scaleParts[2]));

                // Instanciar el objeto en la escena con los datos obtenidos
                var spawnedObject = Instantiate(_markerPrefab, position, rotation, transform);

                // Establecer la escala del objeto
                spawnedObject.transform.localScale = scale;

                // Agregar el objeto instanciado a la lista de objetos generados
                _spawnedObjects.Add(spawnedObject);
            }
        }
    }


    private void OnDestroy()
    {
        SaveSpawnedObjects(); // Guardar los objetos al salir de la escena
    }


    public void DropdownValueChanged(Dropdown change)
    {
        selectedEventID = change.value + 1;
    }

    private void OnDropdownValueChanged(int index)
    {
        // Obtener el evento seleccionado y mostrar su objeto
        var eventName = _dropdown.options[index].text;
        foreach (var obj in _spawnedObjects)
        {
            var eventPointer = obj.GetComponent<EventPointer>();
            if (eventPointer.eventName == eventName)
            {
                obj.SetActive(true);
                _map.UpdateMap();
                ActualizarEventos();

                // Agregar el objeto visible a la lista de objetos visibles
                _spawnedObjects.Add(obj);

                break;
            }
        }
    }

    private void DropdownValue(int index)
    {
        var eventName = _dropdown2.options[index].text;
        foreach (var obj in _spawnedObjects)
        {
            var eventPointer = obj.GetComponent<EventPointer>();
            if (eventPointer.eventID == selectedEventID && obj.activeSelf)
            {
                obj.SetActive(false);
                _spawnedObjects.Remove(obj);
                ActualizarEventos();
                break;
            }
        }
    }

    public void ActualizarEventos()
    {
        StringBuilder sb = new StringBuilder("Puntos en el mapa:\n");
        // Limpiar la cadena de texto

        // Recorrer todos los objetos en la escena
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            // Verificar si el objeto tiene un componente EventPointer
            if (obj.GetComponent<EventPointer>() != null)
            {
                // Verificar si el evento está activo
                if (obj.activeSelf)
                {
                    // Agregar el nombre del evento a la cadena de texto
                    sb.AppendLine(obj.GetComponent<EventPointer>().eventName);
                }
            }
        }

        // Actualizar el texto en el TextMeshProUGUI
        _list.text = sb.ToString();
    }


    private string GetObjectName(int eventID)
    {
        string objectName = "";
        switch (eventID)
        {
            case 1:
                objectName = "La loma";
                break;
            case 2:
                objectName = "La Alameda";
                break;
            case 3:
                objectName = "Parque juan escutia";
                break;
            case 4:
                objectName = "Parque a la madre";
                break;
            case 5:
                objectName = "Plaza bicentenario";
                break;
            case 6:
                objectName = "La hermana agua";
                break;
            case 7:
                objectName = "Monumento a Emilio M. Gonzalez";
                break;
            case 8:
                objectName = "Monumento a Benardo Macias Mora";
                break;
            case 9:
                objectName = "Columna de la pacificacion";
                break;
            case 10:
                objectName = "Monumento Amado Nervo";
                break;
            case 11:
                objectName = "Ángel de la independencia";
                break;
            case 12:
                objectName = "Monumento de Antonio Rivas Mercado";
                break;
            case 13:
                objectName = "Estatua Rey Nayar";
                break;
            case 14:
                objectName = "Muro de los periodistas";
                break;
            case 15:
                objectName = "Monumento a Manuel Lozada";
                break;
            case 16:
                objectName = "Palacio de gobierno";
                break;
            case 17:
                objectName = "Hotel Sierra de Álica";
                break;
            case 18:
                objectName = "Estacion de tren";
                break;
            case 19:
                objectName = "Palacio municipal de Tepic";
                break;
            case 20:
                objectName = "Teatro Calderon";
                break;
            case 21:
                objectName = "Teatro del pueblo Alí Chumacero";
                break;
            case 22:
                objectName = "Catedral";
                break;
            case 23:
                objectName = "Cecupi";
                break;
            case 24:
                objectName = "CICESE-UT3";
                break;
            case 25:
                objectName = "Centro de arte contemporaneo Emilia Ortiz";
                break;
            case 26:
                objectName = "Casa museo Amado Nervo";
                break;
            case 27:
                objectName = "Casa museo Juan Escutia";
                break;
            case 28:
                objectName = "Museo regional centro INAH";
                break;
            case 29:
                objectName = "";
                break;
        }
        return objectName;
    }

    private string GetObjectDescription(int eventID)
    {
        string objectDescripcion = "";
        switch (eventID)
        {
            case 1:
                objectDescripcion = "es un punto de convivencia de mayor importancia para los nayaritas donde se pueden encontrar juegos y el famoso trenecito";
                break;
            case 2:
                objectDescripcion = "Parque iconico de la ciudad de tepic";
                break;
            case 3:
                objectDescripcion = "Parque al niño heroe juan escutia";
                break;
            case 4:
                objectDescripcion = "el H. Ayuntamiento de Tepic, había realizado un evento en el espacio creado (en los años cincuenta del siglo pasado) para honrar a la Madre,";
                break;
            case 5:
                objectDescripcion = "es el espacio que se localiza frente al Palacio de Gobierno. Además, el cual correspondió a la manzana número 120 en la segunda mitad del siglo XIX, fue comprado en 500 pesos y donada a la ciudad en 1870 por el primer jefe político del Distrito Militar de Tepic Juan de Sanromán, con la finalidad de darle un mayor atractivo visual a la entonces Penitenciaria, hoy sede del Poder Ejecutivo del Estado.";
                break;
            case 6:
                objectDescripcion = "una figura estilizada de una mujer desnuda y muy hermosa delante de un muro en el que por su parte posterior se leía un fragmento del poema del mismo nombre del recordado poeta nayarita Amado Nervo; precisamente, se dice, este poema fue la inspiración para la creación de la escultura de la Hermana Agua. Fue tan conocida que se tomaba como punto de referencia en la ciudad.";
                break;
            case 7:
                objectDescripcion = "Monumento a Emilio M. Gonzalez";
                break;
            case 8:
                objectDescripcion = "Monumento a Benardo Macias Mora";
                break;
            case 9:
                objectDescripcion = "Columna de la pacificacion";
                break;
            case 10:
                objectDescripcion = "Monumento Amado Nervo";
                break;
            case 11:
                objectDescripcion = "Ángel de la independencia";
                break;
            case 12:
                objectDescripcion = "Monumento de Antonio Rivas Mercado";
                break;
            case 13:
                objectDescripcion = "Estatua Rey Nayar";
                break;
            case 14:
                objectDescripcion = "Muro de los periodistas";
                break;
            case 15:
                objectDescripcion = "Monumento a Manuel Lozada";
                break;
            case 16:
                objectDescripcion = "Palacio de gobierno";
                break;
            case 17:
                objectDescripcion = "Hotel Sierra de Álica";
                break;
            case 18:
                objectDescripcion = "Estacion de tren";
                break;
            case 19:
                objectDescripcion = "Palacio municipal de Tepic";
                break;
            case 20:
                objectDescripcion = "Teatro Calderon";
                break;
            case 21:
                objectDescripcion = "Teatro del pueblo Alí Chumacero";
                break;
            case 22:
                objectDescripcion = "Catedral";
                break;
            case 23:
                objectDescripcion = "Cecupi";
                break;
            case 24:
                objectDescripcion = "la Unidad de Transferencia Tecnológica Tepic del Centro de Investigación Científica y de Educación Superior de Ensenada (CICESE-UT3). Realizamos investigación aplicada en el área de las Tecnologías de la Información y Comunicación (TIC), y generamos desarrollos tecnológicos.";
                break;
            case 25:
                objectDescripcion = "Centro de arte contemporaneo Emilia Ortiz";
                break;
            case 26:
                objectDescripcion = "Casa museo Amado Nervo";
                break;
            case 27:
                objectDescripcion = "Casa museo Juan Escutia";
                break;
            case 28:
                objectDescripcion = "Museo regional centro INAH";
                break;
            case 29:
                objectDescripcion = "";
                break;

        }
        return objectDescripcion;
    }
    private void Update()
    {
        EventShow();
        ShowVisibleObjects();

        _list.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _list.preferredHeight);
    }

    public void EventShow()
    {
        // Verificar si los arrays están nulos o tienen longitudes diferentes
        if (_spawnedObjects == null || _locations == null || _spawnedObjects.Count != _locations.Length)
        {
            Debug.LogError("Los arrays _spawnedObjects y _locations son nulos o tienen longitudes diferentes.");
            return;
        }

        int count = _spawnedObjects.Count;
        for (int i = 0; i < count; i++)
        {
            // Verificar si el índice está dentro de los límites del arreglo _spawnedObjects
            if (i >= _spawnedObjects.Count)
            {
                Debug.LogError("Index out of range: " + i);
                continue;
            }

            // Verificar si el índice está dentro de los límites del arreglo _locations
            if (i >= _locations.Length)
            {
                Debug.LogError("No hay ubicación válida para el objeto spawnado en la posición " + i + ".");
                continue;
            }

            var spawnedObject = _spawnedObjects[i];
            var location = _locations[i];
            spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, false);
            spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            // Aplicar los filtros de acuerdo al eventoID
            var eventID = spawnedObject.GetComponent<EventPointer>().eventID;
            spawnedObject.SetActive(
                (_showParks && eventID >= 1 && eventID <= 5) ||
                (_showMuseums && eventID >= 25 && eventID <= 28) ||
                (_showStatues && eventID >= 6 && eventID <= 15) ||
                (_showHistoric && eventID >= 16 && eventID <= 24) ||
                (_showObj && eventID == 29)
            );

            // Construir la cadena de filtros activos
            StringBuilder activeFiltersBuilder = new StringBuilder();

            if (_showParks)
                activeFiltersBuilder.Append("Parques, ");
            if (_showMuseums)
                activeFiltersBuilder.Append("Museos, ");
            if (_showStatues)
                activeFiltersBuilder.Append("Estatuas, ");
            if (_showHistoric)
                activeFiltersBuilder.Append("Lugares históricos, ");
            if (_showObj)
                activeFiltersBuilder.Append("");

            // Eliminar la última coma y espacio de la cadena de filtros activos
            if (activeFiltersBuilder.Length > 0)
                activeFiltersBuilder.Length -= 2;

            string activeFilters = activeFiltersBuilder.ToString();

            // Actualizar el texto mostrando los filtros activos
            _lugares.text = "Vas a visitar: " + activeFilters + ". Llevate agua y tennis porque caminarás mucho.";
        }
    }


    public void DisableEventShow()
    {
        enabled = false;
        foreach (var obj in _spawnedObjects)
        {
            var eventPointer = obj.GetComponent<EventPointer>();
            if (eventPointer.eventID == 29 && obj.activeSelf)
            {
                obj.SetActive(false);
                _spawnedObjects.Remove(obj);
                ActualizarEventos();
                break;
            }
        }
    }

    private void ShowVisibleObjects()
    {
        // Usar StringBuilder para construir la cadena de texto que muestra los nombres de objetos visibles
        StringBuilder sb = new StringBuilder("Puntos en el mapa:\n");

        // Recorrer la lista de objetos spawneados
        foreach (GameObject obj in _spawnedObjects)
        {
            // Si el objeto está activo, agregar su nombre a la cadena de texto
            if (obj.activeSelf)
            {
                sb.AppendLine(obj.GetComponent<EventPointer>().eventName);
            }
        }

        // Mostrar la cadena de texto en la consola
        _list.text = (sb.ToString());
    }

    public void ToggleParks(bool value)
    {
        _showParks = value;
    }

    public void ToggleMuseums(bool value)
    {
        _showMuseums = value;
    }

    public void ToggleStatues(bool value)
    {
        _showStatues = value;
    }

    public void ToggleHistoric(bool value)
    {
        _showHistoric = value;
    }

    public void Toggleshow(bool value)
    {
        _showObj = value;
    }


}