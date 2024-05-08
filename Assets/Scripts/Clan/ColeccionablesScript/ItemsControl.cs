using System.Collections;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;

public class ItemsControl : MonoBehaviour
{
    public CrearClan TomarIDClan;

    public MostrarMiembrosClan TomarIDMiembro;

    //Lista de coleccionables a usar
    public List<ListaColeccionables> Coleccionables = new List<ListaColeccionables>();
    public List<ListaMedallas> Medallas = new List<ListaMedallas>();

    // Crear una lista para almacenar los resultados de la consulta de medallas que hay obtenidas
    private List<int> idsMedallas = new List<int>();

    private List<int> idsColeccionables = new List<int>();

    // Variables para la conexion a la base de datos

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;
    
    public ConexionMySQL conexionMySQL;

    private void Start()
    {
        connectionString = conexionMySQL.connectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Debug.Log("Conexión exitosa en ItemsControl");
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error en la conexión: " + ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public void cargarColeccionablesClan()
    {
        //Limpio la lista de coleccionables que se muestran
        reiniciarInterfazColeccionables();

        string queryInfo =
            "SELECT Col_IdColeccionable FROM coleccionables WHERE Col_IdClan = '"
            + TomarIDClan.resultadoIDClan
            + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            int idRecColeccionable = MS_Reader.GetInt32(0);

            // Agrega el ID de la medalla a la lista
            idsColeccionables.Add(idRecColeccionable);
        }

        foreach (var itemRecuperado in idsColeccionables)
        {
            foreach (var itemLista in Coleccionables)
            {
                if (itemRecuperado == itemLista.IDColeccionable)
                {
                    // Modificar la opacidad de la imagen
                    Color color = itemLista.imgColeccionable.color;
                    color.a = 1f; // Cambiar la opacidad al 100% (1f)
                    itemLista.imgColeccionable.color = color;
                }
            }
        }

        MS_Reader.Close();
    }

    public void cargarMedallasClan()
    {
        //Limpio la lista de medallas que se muestran
        reiniciarInterfazMedallas();

        string queryInfo =
            "SELECT Med_IdMedalla FROM medallas WHERE Med_IdClan = '"
            + TomarIDClan.resultadoIDClan
            + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            int idRecMedalla = MS_Reader.GetInt32(0);

            // Agrega el ID de la medalla a la lista
            idsMedallas.Add(idRecMedalla);
        }

        foreach (var itemRecuperado in idsMedallas)
        {
            foreach (var itemLista in Medallas)
            {
                if (itemRecuperado == itemLista.IDMedalla)
                {
                    // Modificar la opacidad de la imagen
                    Color color = itemLista.imgMedalla.color;
                    color.a = 1f; // Cambiar la opacidad al 100% (1f)
                    itemLista.imgMedalla.color = color;
                }
            }
        }

        MS_Reader.Close();
    }

    //Carga los coleccionables de los miembros
    public void cargarColeccionablesMiembro()
    {
        Debug.Log(TomarIDMiembro.idMiembroPublico + " ID Miembro Coleccionable");
        //Limpio la lista de coleccionables que se muestran
        reiniciarInterfazColeccionables();

        string queryInfo =
            "SELECT Col_IdColeccionable FROM coleccionables WHERE Col_IdMiembro = '"
            + TomarIDMiembro.idMiembroPublico
            + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            int idRecColeccionable = MS_Reader.GetInt32(0);

            // Agrega el ID de la medalla a la lista
            idsColeccionables.Add(idRecColeccionable);
        }

        foreach (var itemRecuperado in idsColeccionables)
        {
            foreach (var itemLista in Coleccionables)
            {
                if (itemRecuperado == itemLista.IDColeccionable)
                {
                    // Modificar la opacidad de la imagen
                    Color color = itemLista.imgColeccionable.color;
                    color.a = 1f; // Cambiar la opacidad al 100% (1f)
                    itemLista.imgColeccionable.color = color;
                }
            }
        }

        MS_Reader.Close();
    }

    //Carga las medallas de los miembros
    public void cargarMedallasMiembro()
    {
        Debug.Log(TomarIDMiembro.idMiembroPublico + " ID Miembro medalla");
        //Limpio la lista de medallas que se muestran
        reiniciarInterfazMedallas();

        string queryInfo =
            "SELECT Med_IdMedalla FROM medallas WHERE Med_IdMiembro = '"
            + TomarIDMiembro.idMiembroPublico
            + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            int idRecMedalla = MS_Reader.GetInt32(0);

            // Agrega el ID de la medalla a la lista
            idsMedallas.Add(idRecMedalla);
        }

        foreach (var itemRecuperado in idsMedallas)
        {
            foreach (var itemLista in Medallas)
            {
                if (itemRecuperado == itemLista.IDMedalla)
                {
                    // Modificar la opacidad de la imagen
                    Color color = itemLista.imgMedalla.color;
                    color.a = 1f; // Cambiar la opacidad al 100% (1f)
                    itemLista.imgMedalla.color = color;
                }
            }
        }

        MS_Reader.Close();
    }

    public void reiniciarInterfazMedallas()
    {
        foreach (var itemLista in Medallas)
        {
            // Modificar la opacidad de la imagen
            Color colorMedio = itemLista.imgMedalla.color;
            colorMedio.a = 128 / 255.0f; // Establece el valor de alpha aproximadamente a 0.502
            itemLista.imgMedalla.color = colorMedio;
        }
        idsMedallas.Clear(); //Limpia la lista de medallas
    }

    public void reiniciarInterfazColeccionables()
    {
        foreach (var itemLista in Coleccionables)
        {
            // Modificar la opacidad de la imagen
            Color colorMedio = itemLista.imgColeccionable.color;
            colorMedio.a = 128 / 255.0f; // Establece el valor de alpha aproximadamente a 0.502
            itemLista.imgColeccionable.color = colorMedio;
        }
        idsColeccionables.Clear(); //Limpia la lista de coleccionables
    }
}

[System.Serializable]
public class ListaColeccionables
{
    public Image imgColeccionable;
    public int IDColeccionable;
    public byte active;
}

[System.Serializable]
public class ListaMedallas
{
    public Image imgMedalla;
    public int IDMedalla;
    public byte active;
}
