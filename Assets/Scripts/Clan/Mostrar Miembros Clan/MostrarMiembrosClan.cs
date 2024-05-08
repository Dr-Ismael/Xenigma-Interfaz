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
using System.Data;


public class MostrarMiembrosClan : MonoBehaviour
{
    public TextMeshProUGUI nombre_Clan, CambiarNombre_TXT, NombreCambiado_TXT;

    //Guarda el id del usuario elegido en el menu de clanes
    public string idMiembroPublico;

    public InputField Nombre_field, nickname_field;
    public TextMeshProUGUI edadTxt;
    public TMP_Dropdown dropNewMemberYears, dropEditMemberYears;
    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    private MySqlDataReader MS_Reader;

    private string resultadoNombres, resultadoNickname, NombreG, NicknameG, EdadG, generoG;
    public Login JalarId;

    public CrearClan TomarIDClan;

    public TMP_Text RegistroText;

    public TextMeshProUGUI generoTXT;

    [SerializeField] GameObject RegisMiembrosClan;
    [SerializeField] GameObject EscogerAvatar;
    [SerializeField] GameObject PagPrincipal;
    [SerializeField] GameObject PagMiembros;

    private int idAvatarMiembroG;

    int CuantosMiembros = 0;
    public List<GuardarDatosMiembros> GuardarDatosMiembros = new List<GuardarDatosMiembros>();

    public GameObject miembroPrefab;
    public Transform miembrosContenedor;

    public GameObject subMenu, subMenuMiembros, subMenuInfo, subMenuEdit, subMenuGenero, subMenuAvatar;

    //Variables para mostrar los datos en los campos de informacion del clan
    public TextMeshProUGUI txtInfoClanName, txtLugaresClan, txtHrsClan, txtMedallasClan, txtPuntosClan, txtMiembrosClan, txtColeccionablesClan;

    //Variables de los campos para mostrar la informacion del usuario.
    public Image imgMiembro;
    public TextMeshProUGUI txtMemberName, txtMemberPlaces, txtMemberHours, txtMemberMedals, txtMemberPoints, txtMemberColeccionables;

    //Variables para mostar los datos actuales de editar la informacion del usuario
    public TextMeshProUGUI txtEditMemberName, txtEditMemberNickname, txtEditMemberEdad;

    //Campos de texto con los nuevos datos del miembro a editar
    public TextMeshProUGUI txtNewEditMemberName, txtNewEditMemberNickname, txtNewEditMemberEdad;

    //Variable para guardar el id del miembro a editar
    private int idEditMember;
    //Variables para guardar el avatar y genero elegido a cambiar
    private string generoElegido = "";
    private string nombreEditMember = "";
    private string nicknameEditMember = "";
    //Panel de datos faltantes en editar miembro
    public Image pnlErrorEditar, infoMemberImage;

    public ConexionMySQL conexionMySQL; 

    private void Start()
    {
        connectionString = conexionMySQL.connectionString;
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

    public void ShowClanData()
    {
        //Consulta el nombre del clan con el id del clan
        string queryData = "SELECT NombreClan FROM clanes WHERE id = '" + TomarIDClan.resultadoIDClan + "'";

        //Abre una nueva conexion
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MySqlCommand MS_Command = new MySqlCommand(queryData, MS_Connection);
            object result = MS_Command.ExecuteScalar();
            //Si el resultado de la consulta no es null, entonces muestra el nombre dle clan
            if (result != null)
            {
                nombre_Clan.text = result.ToString();
            }
            else
            {
                //De lo contrario manada un mensaje donde dice que no lo encontro
                Debug.Log("No se encontró ningún clan con el ID especificado.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al ejecutar la consulta: " + e.Message);
        }
        finally
        {
            if (MS_Connection.State == ConnectionState.Open)
            {
                MS_Connection.Close();
            }
        }
    }

    public void PintarInputNombreClan()
    {
        //Consulta el nombre del clan con el id del clan
        string queryData = "SELECT NombreClan FROM clanes WHERE id = '" + TomarIDClan.resultadoIDClan + "'";

        //Abre una nueva conexion
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MySqlCommand MS_Command = new MySqlCommand(queryData, MS_Connection);
            object result = MS_Command.ExecuteScalar();
            //Si el resultado de la consulta no es null, entonces muestra el nombre dle clan
            if (result != null)
            {
                CambiarNombre_TXT.text = result.ToString();
            }
            else
            {
                //De lo contrario manada un mensaje donde dice que no lo encontro
                Debug.Log("No se encontró ningún clan con el ID especificado.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al ejecutar la consulta: " + e.Message);
        }
        finally
        {
            if (MS_Connection.State == ConnectionState.Open)
            {
                MS_Connection.Close();
            }
        }
    }

    public void mostarInfoClan()
    {
        string queryInfo = "SELECT NombreClan, lugaresClan, hrsClan, medallasClan, puntajeClan, miembros, objetosClan FROM clanes WHERE id = '" + TomarIDClan.resultadoIDClan + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            string nombre = MS_Reader.GetString(0);
            int lugares = MS_Reader.GetInt32(1);
            int hrs = MS_Reader.GetInt32(2);
            int medallas = MS_Reader.GetInt32(3);
            int puntos = MS_Reader.GetInt32(4);
            int miembros = MS_Reader.GetInt32(5);
            int coleccionables = MS_Reader.GetInt32(6);

            txtInfoClanName.text = nombre;
            txtLugaresClan.text = "" + lugares;
            txtHrsClan.text = "" + hrs;
            txtMedallasClan.text = "" + medallas;
            txtPuntosClan.text = "" + puntos;
            txtMiembrosClan.text = "" + miembros;
            txtColeccionablesClan.text = "" + coleccionables;
        }
        MS_Reader.Close();
    }

    public void CambiarNombreClan()
    {
        //Consulta para actualizar el nombre del clan con el id del clan
        string queryData = "UPDATE clanes SET NombreClan = '" + NombreCambiado_TXT.text + "' WHERE id = '" + TomarIDClan.resultadoIDClan + "'";

        //Abre una nueva conexión
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MySqlCommand MS_Command = new MySqlCommand(queryData, MS_Connection);
            int rowsAffected = MS_Command.ExecuteNonQuery();
            ShowClanData();
            //Comprueba si se actualizaron filas
            if (rowsAffected > 0)
            {
                Debug.Log("Nombre del clan actualizado correctamente.");
            }
            else
            {
                Debug.Log("No se encontró ningún clan con el ID especificado.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al ejecutar la consulta: " + e.Message);
        }
        finally
        {
            if (MS_Connection.State == ConnectionState.Open)
            {
                MS_Connection.Close();
            }
        }
    }

    public void GuardarMiembros()
    {
        if (Nombre_field == true && nickname_field == true && edadTxt == true)
        {
            NicknameG = nickname_field.text;
            generoG = generoTXT.text;

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

            if (NicknameG == resultadoNickname)
            {
                Debug.Log("Ya existe un miembro con ese nickname");
            }
            else
            {
                NombreG = Nombre_field.text;
                EdadG = edadTxt.text;
                generoG = generoTXT.text;

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
        string query = "INSERT INTO `miembros_clanes` (`id_miembro`, `nombre`, `nickname`, `edad`, `idLider`, `idClan`, `idAvatar`, `puntaje`, `genero`, `lugares`,`hrs`,`medallas`,`coleccionables`,`distancia` ) VALUES (NULL, '" + NombreG + "', '" + NicknameG + "', '" + EdadG + "', '" + JalarId.resultadoID + "', '" + TomarIDClan.resultadoIDClan + "', '" + idAvatarMiembroG + "', 0, '" + generoG + "',0,0,0,0,0);";
        sumarMiembro();

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

        Debug.Log("Usuario agregado correctamente" + "Datos del Registrado" + "Nombre:" + " " + NombreG + " " + "Nickname:" + " " + NicknameG + "Edad:" + " " + EdadG + "ID Lider:" + " " + JalarId.resultadoID + "ID Clan:" + " " + TomarIDClan.resultadoIDClan + "ID Avatar:" + " " + idAvatarMiembroG);
    

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

    //Permite guardar el genero seleccionado que cambiara
    public void generoSeleccionado(string generoS)
    {
        Debug.Log("Genero seleccionaod : " + generoS);
        generoElegido = generoS;
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
        agregarAniosDropDownMembers();
    }

    private void LimpiarRegisMiembro()
    {
        nickname_field.text = "";
        Nombre_field.text = "";
        generoTXT.text = "No seleccionado";

        nickname_field.text = NicknameG;
        Nombre_field.text = NombreG;
        generoTXT.text = generoG;
    }

    private void BorrarDatos()
    {
        nickname_field.text = "";
        Nombre_field.text = "";
        generoTXT.text = "No seleccionado";
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

        ShowClanData();
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
            //nuevoDato.genero = genero;
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

            // Agrego el boton de información del miembro
            Button infoButton = miembroObject.transform.Find("InfoBtn").GetComponent<Button>();
            infoButton.onClick.AddListener(() => cargarInfoMiembro(miembro.id));

            //Agrego el edicion del miembro
            Button editButton = miembroObject.transform.Find("ModificarBTN").GetComponent<Button>();
            editButton.onClick.AddListener(() => mostrarEditarMiembro(miembro.id));

            // aumenta el valor de posY en el espaciado deseado
            posY -= 180f;
        }
    }

    public void eliminar(int idMiembro)
    {
        string query = "DELETE FROM miembros_clanes WHERE id_miembro = '" + idMiembro + "';";
        restarMiembro();

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
        subMenu.SetActive(false);
        subMenuEdit.SetActive(false);
        subMenuInfo.SetActive(false);
        subMenuMiembros.SetActive(false);
        subMenuGenero.SetActive(false);
        subMenuAvatar.SetActive(false);
        pnlErrorEditar.gameObject.SetActive(false);
    }

    //Función que permite visualizar la info del miembro seleccionado
    public void cargarInfoMiembro(int idMiembro)
    {
        idMiembroPublico = idMiembro.ToString(); ;
        string queryInfo = "SELECT nombre, lugares, hrs, medallas, puntaje, idAvatar, coleccionables FROM miembros_clanes WHERE id_miembro = '" + idMiembro + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            string nombre = MS_Reader.GetString(0);
            int lugares = MS_Reader.GetInt32(1);
            int hrs = MS_Reader.GetInt32(2);
            int medallas = MS_Reader.GetInt32(3);
            int puntos = MS_Reader.GetInt32(4);
            int idAvatar = MS_Reader.GetInt32(5);
            int coleccionables = MS_Reader.GetInt32(6); // Corregido el índice

            // Busca la imagen correspondiente en la lista de objetos
            Lista imagenSeleccionada = ObjetosListado.Find(imagen => imagen.ID == idAvatar);

            if (imagenSeleccionada != null)
            {
                Debug.Log("Avatar del miembro: " + imagenSeleccionada.ID);
                infoMemberImage.sprite = imagenSeleccionada.Image.sprite; // Asigna el sprite de la imagen seleccionada a infoMemberImage
            }
            else
            {
                Debug.Log("No se encontró la imagen seleccionada");
            }

            txtMemberName.text = nombre;
            txtMemberHours.text = "" + hrs;
            txtMemberColeccionables.text = "" + coleccionables;
            txtMemberMedals.text = "" + medallas;
            txtMemberPlaces.text = "" + lugares;
            txtMemberPoints.text = "" + puntos;

            subMenu.SetActive(true);
            subMenuMiembros.SetActive(true);
            subMenuInfo.SetActive(true);
        }
        MS_Reader.Close();
    }

    //Funcion que permite cargar el menu de editar miembro
    public void mostrarEditarMiembro(int idMiembro)
    {
        string queryInfo = "SELECT nombre, nickname, edad FROM miembros_clanes WHERE id_miembro = '" + idMiembro + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(queryInfo, MS_Connection);

        agregarAniosDropDownMembers();

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            string nombre = MS_Reader.GetString(0);
            string nickname = MS_Reader.GetString(1);
            int edad = MS_Reader.GetInt32(2);

            txtEditMemberName.text = nombre;
            txtNewEditMemberName.text = nombre;
            txtEditMemberNickname.text = nickname;
            txtNewEditMemberNickname.text = nickname;
            nombreEditMember = nombre;
            nicknameEditMember = nickname;

            // Asigna la edad al dropdown de edad
            dropEditMemberYears.value = edad;
        }
        MS_Reader.Close();

        // Muestra el submenú de edición
        subMenu.SetActive(true);
        subMenuMiembros.SetActive(true);
        subMenuEdit.SetActive(true);
        idEditMember = idMiembro;
    }

    public void agregarAniosDropDownMembers()
    {
        // Limpia las opciones actuales del Dropdown
        dropNewMemberYears.ClearOptions();
        dropEditMemberYears.ClearOptions();

        // Crea una lista para almacenar las opciones
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        // Agrega las opciones al Dropdown, del 1 al 99
        for (int number = 0; number <= 100; number++)
        {
            options.Add(new TMP_Dropdown.OptionData(number.ToString()));
        }

        // Establece las opciones en el Dropdown
        dropNewMemberYears.options = options;
        dropEditMemberYears.options = options;

        // Para que el Dropdown muestre la opción seleccionada correctamente
        dropNewMemberYears.RefreshShownValue();
        dropEditMemberYears.RefreshShownValue();
    }

    //Permite cambiar los datos del miembro
    public void cambiarDatosMiembro()
    {
        if (idAvatarMiembroG == 0 || string.IsNullOrEmpty(generoElegido) || string.IsNullOrEmpty(nombreEditMember) || string.IsNullOrEmpty(nicknameEditMember))
        {
            Debug.Log("Selecciona el nuevo género y el nuevo avatar");
            pnlErrorEditar.gameObject.SetActive(true);
            return;
        }
        else
        {
            nombreEditMember = txtNewEditMemberName.text;
            nicknameEditMember = txtNewEditMemberNickname.text;
        }

        string queryEdit = "UPDATE `miembros_clanes` " +
               "SET `nombre` = '" + nombreEditMember + "', " +
               "`nickname` = '" + nicknameEditMember + "', " +
               "`edad` = '" + txtNewEditMemberEdad.text + "', " +
               "`idAvatar` = '" + idAvatarMiembroG + "', " +
               "`genero` = '" + generoElegido + "' " +
               "WHERE `id_miembro` = '" + idEditMember + "';";

        Debug.Log("Nuevos datos: Nombre " + nombreEditMember + ", Edad " + txtNewEditMemberEdad.text + ", Nickname " + nicknameEditMember + ", Avatar " + idAvatarMiembroG + ", Genero " + generoElegido);

        //Recarga la lista de miembros
        BorrarContenedores();

        //Cierra el menu y reinicia los valores
        subMenu.SetActive(false);
        subMenuMiembros.SetActive(false);
        subMenuEdit.SetActive(false);
        generoElegido = "";
        idAvatarMiembroG = 0;
        nombreEditMember = "";
        nicknameEditMember = "";
        txtNewEditMemberName.text = "" + nombreEditMember;
        txtNewEditMemberNickname.text = "" + nicknameEditMember;
        pnlErrorEditar.gameObject.SetActive(false);

        //Abre una nueva conexión
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MySqlCommand MS_Command = new MySqlCommand(queryEdit, MS_Connection);
            int rowsAffected = MS_Command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al ejecutar la consulta: " + e.Message);
        }
        finally
        {
            if (MS_Connection.State == ConnectionState.Open)
            {
                MS_Connection.Close();
            }
        }

        Pintar();
    }

    public void sumarMiembro()
    {
        string idLider = PlayerPrefs.GetString("idUser");
        Debug.Log("id del lider a sumar" + idLider);
        int miembros=0;
        string query = "Select miembros FROM clanes WHERE id_lider = '" + idLider + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            miembros = MS_Reader.GetInt32(0);

            miembros++;
            Debug.Log("Nueva cantidad de miembros: "+miembros);
        }
        MS_Reader.Close();

        string queryUpdate = "UPDATE `clanes` SET `miembros` = " + miembros + " WHERE `id_lider` = " + idLider + ";";

        //Abre una nueva conexión
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MySqlCommand MS_Command = new MySqlCommand(queryUpdate, MS_Connection);
            int rowsAffected = MS_Command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al ejecutar la consulta: " + e.Message);
        }
        finally
        {
            if (MS_Connection.State == ConnectionState.Open)
            {
                MS_Connection.Close();
            }
        }
    }

    public void restarMiembro()
    {
        string idLider = PlayerPrefs.GetString("idUser");
        Debug.Log("id del lider a restar" + idLider);
        int miembros=0;
        string query = "Select miembros FROM clanes WHERE id_lider = '" + idLider + "';";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();

        MS_Comand = new MySqlCommand(query, MS_Connection);

        MS_Reader = MS_Comand.ExecuteReader();
        while (MS_Reader.Read())
        {
            // Recupera los datos del usuario y asígnalos a variables
            miembros = MS_Reader.GetInt32(0);

            miembros--;
            Debug.Log("Nueva cantidad de miembros: "+miembros);
        }
        MS_Reader.Close();

        string queryUpdate = "UPDATE `clanes` SET `miembros` = " + miembros + " WHERE `id_lider` = " + idLider + ";";

        //Abre una nueva conexión
        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MySqlCommand MS_Command = new MySqlCommand(queryUpdate, MS_Connection);
            int rowsAffected = MS_Command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.LogError("Error al ejecutar la consulta: " + e.Message);
        }
        finally
        {
            if (MS_Connection.State == ConnectionState.Open)
            {
                MS_Connection.Close();
            }
        }
    }


}

[System.Serializable]
public class GuardarDatosMiembros
{
    public int id;
    public string nombre;
    public string nickname;
    public int edad;
    public string genero;
}


[System.Serializable]
public class Lista
{
    public Image Image;
    public int ID;
}
