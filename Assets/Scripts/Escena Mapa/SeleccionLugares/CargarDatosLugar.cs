using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CargarDatosLugar : MonoBehaviour
{
    //Lista de datos que se asignan al abrir los detalles de un lugar
    public List<DatosLugarSeleccionado> LugarSeleccionado = new List<DatosLugarSeleccionado>();

    //Lista de datos de todos los puntos de interes
    public List<DatosLugares> DatosLugares = new List<DatosLugares>();

    //Lista de estrellas que se asignan al abrir los detalles de un lugar
    public List<EstrellasLugarSeleccionado> EstrellasLugarSeleccionado =
        new List<EstrellasLugarSeleccionado>();

    //Sprites de las estrellas vacias y llenas para los detalles de un lugar
    public Sprite estrellaLlenaSprite;
    public Sprite estrellaVaciaSprite;
    public GameObject SubMenuDetalles;

    //Lista que guarda los lugares seleccionados por el usuario

    [SerializeField]
    public List<int> totalSitios = new List<int>();

    [SerializeField]
    private List<int> totalLugares = new List<int>();

    [SerializeField]
    private List<int> totalMonumentos = new List<int>();

    [SerializeField]
    private List<int> totalParques = new List<int>();

    [SerializeField]
    private List<int> totalEstatuas = new List<int>();

    [SerializeField]
    private List<int> totalObjetos = new List<int>();

    // Lista para guardar los valores de cada uno en la base de datos
    List<int> listaVal_PromedioTotal = new List<int>();
    List<int> listaVal_Diversion = new List<int>();
    List<int> listaVal_Belleza = new List<int>();
    List<int> listaVal_Seguridad = new List<int>();
    List<int> listaVal_Interes = new List<int>();
    List<int> listaVal_Limpieza = new List<int>();

    //Textos que muestran el numero de lugares elegidos
    public TextMeshProUGUI txtTotalSitiosElegidos,
        txtTotalLugaresHistoricos,
        txtTotalMonumentos,
        txtTotalParques,
        txtTotalEstatuas,
        txtTotalObjetos;

    //Lista de totales de los diferentes lugares elegidos
    private int numTotalSitios,
        numTotalLugares,
        numTotalMonumentos,
        numTotalParques,
        numTotalEstatuas,
        numTotalObjetos;

    //Variables para guardar los prefabs generados en la lista de lugares seleccionados
    public GameObject lugarSeleccionadoPrefab;
    public Transform lugaresContenedor;

    //Variables para panel de error
    public TextMeshProUGUI toastText; // Variable para toast de elegir lugares
    public float duration = 2.0f; // Duración del Toast
    public GameObject PN_Error;
    private Coroutine toastCoroutine;

    //Pantallas varias
    public GameObject ListaLugaresSeleccionados;
    public GameObject FiltroLugares;

    //Conexion a MySQL
    public ConexionMySQL conexionMySQL;
    private string connectionString;
    private MySqlConnection MS_Connection;
    private MySqlCommand MS_Comand;
    private MySqlDataReader MS_Reader;

    private void Start()
    {
        connectionString = conexionMySQL.connectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Debug.Log("Conexión exitosa en CargarDatosLugar");
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error en la conexión de CargarDatosLugar: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    private void Awake()
    {
        if (toastText != null)
        {
            toastText.gameObject.SetActive(false);
            PN_Error.gameObject.SetActive(false);
        }
    }

    public void cargarDetallesPuntoSeleccionado(int idLugar)
    {
        string queryInfo =
            "SELECT Val_PromedioTotal, Val_Diversion, Val_Belleza, Val_Seguridad, Val_Interes, Val_Limpieza FROM valoracion WHERE idSitio = '"
            + idLugar
            + "';";

        // Ejecutar la consulta SQL
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Crear una instancia de ValoracionData para almacenar los datos de la fila actual
            ValoracionData valoracion = new ValoracionData();

            // Asignar los valores de las columnas a las propiedades de la instancia de ValoracionData
            valoracion.Val_PromedioTotal = MS_Reader.GetInt32("Val_PromedioTotal");
            valoracion.Val_Diversion = MS_Reader.GetInt32("Val_Diversion");
            valoracion.Val_Belleza = MS_Reader.GetInt32("Val_Belleza");
            valoracion.Val_Seguridad = MS_Reader.GetInt32("Val_Seguridad");
            valoracion.Val_Interes = MS_Reader.GetInt32("Val_Interes");
            valoracion.Val_Limpieza = MS_Reader.GetInt32("Val_Limpieza");

            // Agregar los valores a las listas correspondientes
            listaVal_PromedioTotal.Add(valoracion.Val_PromedioTotal);
            listaVal_Diversion.Add(valoracion.Val_Diversion);
            listaVal_Belleza.Add(valoracion.Val_Belleza);
            listaVal_Seguridad.Add(valoracion.Val_Seguridad);
            listaVal_Interes.Add(valoracion.Val_Interes);
            listaVal_Limpieza.Add(valoracion.Val_Limpieza);
        }
        MS_Reader.Close();

        //Variables para guardar el valor promedio de las distintas valoraciones
        float promedioTotal = 0;
        float promedioDiversion = 0;
        float promedioBelleza = 0;
        float promedioSeguridad = 0;
        float promedioInteres = 0;
        float promedioLimpieza = 0;
        int valoraciones = 0;

        //Saco todos los promedios de las valoraciones y se lo asigno a si correspondiente
        foreach (var item in listaVal_PromedioTotal)
        {
            valoraciones++;
            promedioTotal += item;
        }
        if (valoraciones != 0)
        {
            EstrellasLugarSeleccionado[5].Valoracion = promedioTotal / valoraciones;
            valoraciones = 0;
        }
        else
        {
            EstrellasLugarSeleccionado[5].Valoracion = 0;
        }

        foreach (var item in listaVal_Diversion)
        {
            valoraciones++;
            promedioDiversion += item;
        }
        if (valoraciones != 0)
        {
            EstrellasLugarSeleccionado[4].Valoracion = promedioDiversion / valoraciones;
            valoraciones = 0;
        }
        else
        {
            EstrellasLugarSeleccionado[4].Valoracion = 0;
        }

        foreach (var item in listaVal_Belleza)
        {
            valoraciones++;
            promedioBelleza += item;
        }
        if (valoraciones != 0)
        {
            EstrellasLugarSeleccionado[3].Valoracion = promedioBelleza / valoraciones;
            valoraciones = 0;
        }
        else
        {
            EstrellasLugarSeleccionado[3].Valoracion = 0;
        }

        foreach (var item in listaVal_Seguridad)
        {
            valoraciones++;
            promedioSeguridad += item;
        }
        if (valoraciones != 0)
        {
            EstrellasLugarSeleccionado[0].Valoracion = promedioSeguridad / valoraciones;
            valoraciones = 0;
        }
        else
        {
            EstrellasLugarSeleccionado[0].Valoracion = 0;
        }

        foreach (var item in listaVal_Interes)
        {
            valoraciones++;
            promedioInteres += item;
        }
        if (valoraciones != 0)
        {
            EstrellasLugarSeleccionado[2].Valoracion = promedioInteres / valoraciones;
            valoraciones = 0;
        }
        else
        {
            EstrellasLugarSeleccionado[2].Valoracion = 0;
        }

        foreach (var item in listaVal_Limpieza)
        {
            valoraciones++;
            promedioLimpieza += item;
        }
        if (valoraciones != 0)
        {
            EstrellasLugarSeleccionado[1].Valoracion = promedioLimpieza / valoraciones;
            valoraciones = 0;
        }
        else
        {
            EstrellasLugarSeleccionado[1].Valoracion = 0;
        }

        //Recorre el array de todos los lugares guardados
        foreach (var item in DatosLugares)
        {
            //Si el id recibido por el boton es igual al de un elmento de la lista entonces asigna los valores correspondientes al id que coincidio
            if (item.id == idLugar)
            {
                LugarSeleccionado[0].nombreLugar.text = item.nombre;
                LugarSeleccionado[0].descripcion.text = item.descripcion;

                //Recorro el array de las estrellas seleccionadas para asignar las estrellas segun la valoracion asignada
                foreach (var itemEstrellas in EstrellasLugarSeleccionado)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                if (itemEstrellas.Valoracion > 0.5)
                                {
                                    itemEstrellas.Star1.sprite = estrellaLlenaSprite;
                                }
                                break;
                            case 2:
                                if (itemEstrellas.Valoracion > 1.5)
                                {
                                    itemEstrellas.Star2.sprite = estrellaLlenaSprite;
                                }
                                break;
                            case 3:
                                if (itemEstrellas.Valoracion > 2.5)
                                {
                                    itemEstrellas.Star3.sprite = estrellaLlenaSprite;
                                }
                                break;
                            case 4:
                                if (itemEstrellas.Valoracion > 3.5)
                                {
                                    itemEstrellas.Star4.sprite = estrellaLlenaSprite;
                                }
                                break;
                            case 5:
                                if (itemEstrellas.Valoracion > 4.5)
                                {
                                    itemEstrellas.Star5.sprite = estrellaLlenaSprite;
                                }
                                break;
                            default:
                                Debug.Log("Error: no se pudo asignar el valor o Final del array");
                                break;
                        }

                        //En caso de que no tenga valoracion, entonces vacia todas las estrellas de esa valoracion
                        if (itemEstrellas.Valoracion == 0)
                        {
                            itemEstrellas.Star1.sprite = estrellaVaciaSprite;
                            itemEstrellas.Star2.sprite = estrellaVaciaSprite;
                            itemEstrellas.Star3.sprite = estrellaVaciaSprite;
                            itemEstrellas.Star4.sprite = estrellaVaciaSprite;
                            itemEstrellas.Star5.sprite = estrellaVaciaSprite;
                        }
                    }
                }
            }
        }
        SubMenuDetalles.SetActive(true);

        //Limpio las listas para usarlas despues
        listaVal_PromedioTotal.Clear();
        listaVal_Diversion.Clear();
        listaVal_Belleza.Clear();
        listaVal_Seguridad.Clear();
        listaVal_Interes.Clear();
        listaVal_Limpieza.Clear();
    }

    //Agrega lugares a la lista
    public void agregarVisitaLugar(int idLugar)
    {
        //bool sitioEncontrado = false;

        // Verifica si el lugar está en alguna de las otras listas y lo elimina
        if (totalLugares.Contains(idLugar))
        {
            totalLugares.Remove(idLugar);
            totalSitios.Remove(idLugar);
            numTotalLugares--;
            numTotalSitios--;
            txtTotalSitiosElegidos.text = numTotalSitios.ToString();
            txtTotalLugaresHistoricos.text = numTotalLugares.ToString();
        }
        else if (totalMonumentos.Contains(idLugar))
        {
            totalMonumentos.Remove(idLugar);
            totalSitios.Remove(idLugar);
            numTotalMonumentos--;
            numTotalSitios--;
            txtTotalSitiosElegidos.text = numTotalSitios.ToString();
            txtTotalMonumentos.text = numTotalMonumentos.ToString();
        }
        else if (totalParques.Contains(idLugar))
        {
            totalParques.Remove(idLugar);
            totalSitios.Remove(idLugar);
            numTotalParques--;
            numTotalSitios--;
            txtTotalSitiosElegidos.text = numTotalSitios.ToString();
            txtTotalParques.text = numTotalParques.ToString();
        }
        else if (totalEstatuas.Contains(idLugar))
        {
            totalEstatuas.Remove(idLugar);
            totalSitios.Remove(idLugar);
            numTotalEstatuas--;
            numTotalSitios--;
            txtTotalSitiosElegidos.text = numTotalSitios.ToString();
            txtTotalEstatuas.text = numTotalEstatuas.ToString();
        }
        else if (totalObjetos.Contains(idLugar))
        {
            totalObjetos.Remove(idLugar);
            totalSitios.Remove(idLugar);
            numTotalObjetos--;
            numTotalSitios--;
            txtTotalSitiosElegidos.text = numTotalSitios.ToString();
            txtTotalObjetos.text = numTotalObjetos.ToString();
        }
        else
        {
            // Si no se ha elegido el sitio, lo agrega a la lista correspondiente
            foreach (var item in DatosLugares)
            {
                if (idLugar == item.id)
                {
                    switch (item.tipoLugar)
                    {
                        case "Lugar historico":
                            numTotalSitios++;
                            numTotalLugares++;
                            totalLugares.Add(idLugar);
                            txtTotalLugaresHistoricos.text = numTotalLugares.ToString();
                            break;
                        case "Monumento":
                            numTotalSitios++;
                            numTotalMonumentos++;
                            totalMonumentos.Add(idLugar);
                            txtTotalMonumentos.text = numTotalMonumentos.ToString();
                            break;
                        case "Parque":
                            numTotalSitios++;
                            numTotalParques++;
                            totalParques.Add(idLugar);
                            txtTotalParques.text = numTotalParques.ToString();
                            break;
                        case "Estatua":
                            numTotalSitios++;
                            numTotalEstatuas++;
                            totalEstatuas.Add(idLugar);
                            txtTotalEstatuas.text = numTotalEstatuas.ToString();
                            break;
                        case "Objeto":
                            numTotalSitios++;
                            numTotalObjetos++;
                            totalObjetos.Add(idLugar);
                            txtTotalObjetos.text = numTotalObjetos.ToString();
                            break;
                        default:
                            Debug.Log("Error al filtrar el tipo de lugar");
                            break;
                    }
                    // Agrega el lugar a totalSitios
                    totalSitios.Add(idLugar);
                    txtTotalSitiosElegidos.text = numTotalSitios.ToString();
                    break; // Sale del bucle una vez que encuentra el lugar
                }
            }
        }
    }

    public void cargarLugaresElegidos()
    {
        //Comprueba si el usuario ha elegido al menos un sitio
        if (totalSitios.Count == 0)
        {
            ShowToast("No has elegido ningun punto de interes");
        }
        else
        {
            Debug.Log("totalSitios tiene elementos: " + totalSitios.Count);
            float posY = 79.8f; // variable para llevar un seguimiento de la posición Y
            foreach (var idLugar in totalSitios)
            {
                foreach (var itemLugar in DatosLugares)
                {
                    if (itemLugar.id == idLugar)
                    {
                        GameObject lugarObject = Instantiate(
                            lugarSeleccionadoPrefab,
                            lugaresContenedor
                        );

                        // ajusta la posición Y del objeto utilizando la variable posY
                        RectTransform rt = lugarObject.GetComponent<RectTransform>();
                        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

                        TextMeshProUGUI nombreText = lugarObject
                            .transform.Find("Txt_Lugar")
                            .GetComponent<TextMeshProUGUI>();
                        nombreText.text = itemLugar.nombre;

                        // obtener el componente Button y agregarle un listener
                        Button btn_Subir = lugarObject
                            .transform.Find("Btn_Subir")
                            .GetComponent<Button>();
                        btn_Subir.onClick.AddListener(() => subirElementoLista(itemLugar.id));

                        // obtener el componente Button y agregarle un listener
                        Button btn_Bajar = lugarObject
                            .transform.Find("Btn_Bajar")
                            .GetComponent<Button>();
                        btn_Bajar.onClick.AddListener(() => bajarElementoLista(itemLugar.id));

                        // aumenta el valor de posY en el espaciado deseado
                        posY -= 210f;
                    }
                }
            }
            ListaLugaresSeleccionados.SetActive(true);
            FiltroLugares.SetActive(false);
        }
    }

    public void subirElementoLista(int idLugar)
    {
        // Encuentra el índice del número en la lista
        int indiceActual = totalSitios.IndexOf(idLugar);

        // Verifica que el índice no es -1 (el número no está en la lista) y que no es el primero
        if (indiceActual > 0)
        {
            // Intercambia el número con el elemento anterior en la lista
            int elementoAnterior = totalSitios[indiceActual - 1];
            totalSitios[indiceActual - 1] = idLugar;
            totalSitios[indiceActual] = elementoAnterior;

            //Recargo la lista que se muestra al usuario
            ClearChildren();
            cargarLugaresElegidos();

            // Opcional: imprime la lista para depuración
            Debug.Log("Lista después del movimiento: " + string.Join(", ", totalSitios));
        }
        else
        {
            Debug.Log("El número no se puede mover o ya está en la primera posición.");
        }
    }

    public void bajarElementoLista(int idLugar)
    {
        // Encuentra el índice del número en la lista
        int indiceActual = totalSitios.IndexOf(idLugar);

        // Verifica que el índice no es -1 (el número no está en la lista) y que no es el último
        if (indiceActual != -1 && indiceActual < totalSitios.Count - 1)
        {
            // Intercambia el número con el elemento siguiente en la lista
            int elementoSiguiente = totalSitios[indiceActual + 1];
            totalSitios[indiceActual + 1] = idLugar;
            totalSitios[indiceActual] = elementoSiguiente;

            // Recargo la lista que se muestra al usuario
            ClearChildren();
            cargarLugaresElegidos();

            // Opcional: imprime la lista para depuración
            Debug.Log("Lista después del movimiento: " + string.Join(", ", totalSitios));
        }
        else
        {
            Debug.Log("El número no se puede mover o ya está en la última posición.");
        }
    }

    public void ShowToast(string message)
    {
        if (toastText != null)
        {
            if (toastCoroutine != null)
            {
                StopCoroutine(toastCoroutine);
            }
            toastCoroutine = StartCoroutine(ShowToastCoroutine(message));
        }
    }

    private IEnumerator ShowToastCoroutine(string message)
    {
        toastText.text = message;
        toastText.gameObject.SetActive(true);
        PN_Error.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        toastText.gameObject.SetActive(false);
        PN_Error.gameObject.SetActive(false);
    }

    public void limpiarRegistros()
    {
        numTotalSitios = 0;
        numTotalEstatuas = 0;
        numTotalLugares = 0;
        numTotalMonumentos = 0;
        numTotalObjetos = 0;
        numTotalParques = 0;
        totalParques.Clear();
        totalEstatuas.Clear();
        totalLugares.Clear();
        totalMonumentos.Clear();
        totalObjetos.Clear();
        totalSitios.Clear();
        txtTotalEstatuas.text = "0";
        txtTotalLugaresHistoricos.text = "0";
        txtTotalMonumentos.text = "0";
        txtTotalObjetos.text = "0";
        txtTotalParques.text = "0";
        txtTotalSitiosElegidos.text = "0";
    }

     // Método para eliminar todos los hijos del contenedor
    public void ClearChildren()
    {
        // Iterar sobre los hijos y destruirlos
        foreach (Transform child in lugaresContenedor)
        {
            Destroy(child.gameObject);
        }
    }
}

[System.Serializable]
public class DatosLugarSeleccionado
{
    public TextMeshProUGUI nombreLugar;
    public TextMeshProUGUI descripcion;
}

[System.Serializable]
public class EstrellasLugarSeleccionado
{
    public Image Star1;
    public Image Star2;
    public Image Star3;
    public Image Star4;
    public Image Star5;
    public float Valoracion;
}

[System.Serializable]
public class DatosLugares
{
    public int id;
    public string nombre;
    public string descripcion;
    public string tipoLugar;
}

// Clase para representar los datos de la valoración
public class ValoracionData
{
    public int Val_PromedioTotal { get; set; }
    public int Val_Diversion { get; set; }
    public int Val_Belleza { get; set; }
    public int Val_Seguridad { get; set; }
    public int Val_Interes { get; set; }
    public int Val_Limpieza { get; set; }
}
