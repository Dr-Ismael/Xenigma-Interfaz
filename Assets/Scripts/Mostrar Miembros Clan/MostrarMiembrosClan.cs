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
public class MostrarMiembrosClan : MonoBehaviour
{
    

    public InputField Nombre_field, nickname_field, edad_field;
    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    private string resultadoNombres, resultadoNickname, NombreG, NicknameG, EdadG;
    public Login JalarId;

    public CrearClan TomarIDClan;

    public TMP_Text RegistroText;
    

    [SerializeField] GameObject RegisMiembrosClan;
    [SerializeField] GameObject EscogerAvatar;
    [SerializeField] GameObject PagPrincipal;
    [SerializeField] GameObject PagMiembros;


    private int idAvatarMiembroG;

    int CuantosMiembros = 0;
    public List<GuardarDatosMiembros> GuardarDatosMiembros = new List<GuardarDatosMiembros>();

    public GameObject miembroPrefab;
    public Transform miembrosContenedor;
    



  
    
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

    public void GuardarMiembros()
    {
        if (Nombre_field == true && nickname_field == true && edad_field == true)
        {

            NicknameG = nickname_field.text;

            string query = "SELECT * FROM users where nickname = '" + NicknameG + "';";

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(query, MS_Connection);

            MS_Reader = MS_Comand.ExecuteReader();
            while (MS_Reader.Read())
            {
                resultadoNickname = MS_Reader.GetString(2);
                Debug.Log(resultadoNickname);
            }
            MS_Reader.Close();

            if(NicknameG == resultadoNickname )
            {
                Debug.Log("Ya existe un miembro con ese nickname");
            }
            else
            {
                NombreG = Nombre_field.text;
                EdadG = edad_field.text;

                
                RegisMiembrosClan.SetActive(false);
                EscogerAvatar.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Llena todos los campos");
        }
    }

    public void AgregarMiembros()
    {
        
        string query = "INSERT INTO `miembros_clanes` (`id_miembro`, `nombre`, `nickname`, `edad`, `idLider`, `idClan`, `idAvatar`, `puntaje`) VALUES (NULL, '" + NombreG + "', '" + NicknameG + "', '" + EdadG + "', '" + JalarId.resultadoID + "', '" + TomarIDClan.resultadoIDClan + "', '" + idAvatarMiembroG + "', 0);";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Registro");
            RegistroText.text = "Registro exitoso";

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Connection.Close();
        
        Debug.Log("Usuario agregado correctamente" + "Datos del Registrado" + "Nombre:"+ " " + NombreG + " " + "Nickname:"+ " " + NicknameG + "Edad:"+ " " + EdadG + "ID Lider:"+ " " + JalarId.resultadoID + "ID Clan:"+ " " + TomarIDClan.resultadoIDClan + "ID Avatar:"+ " " + idAvatarMiembroG);

        EscogerAvatar.SetActive(false);
        PagMiembros.SetActive(true);

        BorrarContenedores();
        Pintar();  
    }
   
    public List<Lista> ObjetosListado = new List<Lista>();

    public void ImagenSeleccionada(int id)
    {
        Lista imagenSeleccionada = ObjetosListado.Find(imagen => imagen.ID == id);

        if (imagenSeleccionada != null)
        {
            Debug.Log("Imagen seleccionada: " + imagenSeleccionada.ID);

            idAvatarMiembroG = imagenSeleccionada.ID;

        }
        else
        {
            Debug.Log("No se encontró la imagen seleccionada");
        }
    }

    public void RegresaRegisPagMiembros()
    {
        LimpiarRegisMiembro();
        RegisMiembrosClan.SetActive(false);
        PagMiembros.SetActive(true);
    }

    public void RegistrarMimebros()
    {
        LimpiarRegisMiembro();
        BorrarDatos();
        RegisMiembrosClan.SetActive(true);
        PagMiembros.SetActive(false);
    }

    private void LimpiarRegisMiembro()
    {
        nickname_field.text = "";
        Nombre_field.text = "";
        edad_field.text = "";



        nickname_field.text = NicknameG;
        Nombre_field.text = NombreG;
        edad_field.text = EdadG;
    }

    private void BorrarDatos()
    {
        nickname_field.text = "";
        Nombre_field.text = "";
        edad_field.text = "";
    }

    public void RegresaRegisMiembroAvatar()
    {
        LimpiarRegisMiembro();
        EscogerAvatar.SetActive(false);
        RegisMiembrosClan.SetActive(true);
    }

    public void Pintar()
    {
        string query = "SELECT COUNT(*) FROM miembros_clanes WHERE idClan = '" + TomarIDClan.resultadoIDClan + "';";


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
            GuardarDatosMiembros nuevoDato = new GuardarDatosMiembros();
            nuevoDato.id = id;
            nuevoDato.nombre = nombre;
            nuevoDato.nickname = nickname;
            nuevoDato.edad = edad;
            GuardarDatosMiembros.Add(nuevoDato);
        }
        MS_Reader.Close();

        Mostrar();
    }

    public void Mostrar()
    {
        // Elimina todos los miembros existentes en la lista
        foreach (Transform child in miembrosContenedor)
        {
            Destroy(child.gameObject);
        }

        float posY = 79.8f; // variable para llevar un seguimiento de la posición Y

        foreach (GuardarDatosMiembros miembro in GuardarDatosMiembros)
        {
            GameObject miembroObject = Instantiate(miembroPrefab, miembrosContenedor);

            // ajusta la posición Y del objeto utilizando la variable posY
            RectTransform rt = miembroObject.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, posY);

            TextMeshProUGUI nombreText = miembroObject.transform.Find("Nombre").GetComponent<TextMeshProUGUI>();
            nombreText.text = miembro.nombre;

            TextMeshProUGUI nicknameText = miembroObject.transform.Find("Nickname").GetComponent<TextMeshProUGUI>();
            nicknameText.text = miembro.nickname;

            TextMeshProUGUI edadText = miembroObject.transform.Find("Edad").GetComponent<TextMeshProUGUI>();
            edadText.text = miembro.edad.ToString();

            // Agrega un botón "Eliminar" para cada miembro que llama a la función eliminar() con el ID del miembro correspondiente
            Button eliminarButton = miembroObject.transform.Find("EliminarBTN").GetComponent<Button>();
            eliminarButton.onClick.AddListener(() => eliminar(miembro.id));

            // aumenta el valor de posY en el espaciado deseado
            posY -= 35.8f;
        }
    }

    public void eliminar(int idMiembro)
    {
        string query = "DELETE FROM miembros_clanes WHERE id_miembro = '" + idMiembro + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Eliminado Exitosamente");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Connection.Close();

        // Elimina el miembro de la lista
        GuardarDatosMiembros miembroEliminado = GuardarDatosMiembros.Find(x => x.id == idMiembro);
        GuardarDatosMiembros.Remove(miembroEliminado);

        // Vuelve a mostrar la lista actualizada
        Mostrar();
    }

    public void BorrarContenedores()
    {
        // Elimina todos los miembros existentes en la lista
        foreach (Transform child in miembrosContenedor)
        {
            Destroy(child.gameObject);
        }

        GuardarDatosMiembros.Clear();
    }

    public void RegresarMenu()
    {
        BorrarContenedores();
        PagMiembros.SetActive(false);
        PagPrincipal.SetActive(true);
    }

}

[System.Serializable] public class GuardarDatosMiembros
{
    public int id;
    public string nombre;
    public string nickname;
    public int edad;
}


[System.Serializable] public class Lista
{
    public Image Image;
    public int ID;
}
