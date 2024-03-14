using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    public InputField email_field, password_field;

    private MySqlDataReader MS_Reader;

    [SerializeField] public GameObject pagBienvenida;
    [SerializeField] public GameObject MenuLogin;
    [SerializeField] public GameObject MenuPrincipal;
    [SerializeField] public GameObject ajustes;
    [SerializeField] public GameObject PagAvatar;

    public TMP_Text NicknameInput;

    private string emailG;
    private string passwordG;

    private string resultadoUser;
    private string resultadoPassword;
    private string resultadoNickname;
    public string resultadoID;

    bool isajustes;

    //Variable que permite mostrar error si te equivocaste al poner el correo o contraseña
    public TextMeshProUGUI txtError;

    //Varibles que permiten a un nuevo usuario registrarse al momento e iniciar sesión
    private string emailGRegistro;
    private string passwordGRegistro;
    public InputField email_field_Reg, password_field_Reg;
    public Button BtnIniciarRegistro;


    private void Start()
    {
        connectionString = "Server=localhost;Port=3306;Database=Xenigmabd;User=XenigmaJuego;Password=OHfoUIt[gt7uHWJS;";
        MySqlConnection connection = new MySqlConnection(connectionString);
          
        txtError.gameObject.SetActive(false);
        BtnIniciarRegistro.gameObject.SetActive(false);

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

    public void logearse()
    {

        if (email_field.text == "" || password_field.text == "")
        {
            Debug.Log("Comprueba de que los campos no esten vacios");
        }
        else
        {
            emailG = email_field.text;
            passwordG = password_field.text;

            byte[] password_bytes = new UTF8Encoding().GetBytes(passwordG);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(password_bytes);
            string encoded_password = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            Debug.Log(encoded_password);

            string query = "SELECT * FROM users where email = '" + emailG + "' and password = '" + encoded_password + "';";
            Debug.Log(query);

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Reader = MS_Comand.ExecuteReader();
            while (MS_Reader.Read())
            {
                resultadoUser = MS_Reader.GetString(1);
                resultadoPassword = MS_Reader.GetString(5);
                resultadoNickname = MS_Reader.GetString(3);
                resultadoID = MS_Reader.GetString(0);

                Debug.Log(resultadoID + " " + resultadoUser + " " + resultadoPassword);
            }

            MS_Reader.Close();

            if (resultadoUser == emailG && resultadoPassword == encoded_password)
            {
                // Guardar el ID de usuario en PlayerPrefs
                PlayerPrefs.SetString("IDUsuario", resultadoID);

                MenuLogin.SetActive(false);
                NicknameInput.text = resultadoNickname;
                pagBienvenida.SetActive(true);
                StartCoroutine(apagar());
                txtError.gameObject.SetActive(false);

                Debug.Log("Sesión Iniciada con exito, Bienvenido " + " " + resultadoNickname);
            }
            else
            {
                Debug.Log("El usuario o contraseña son incorrectos");
                txtError.gameObject.SetActive(true);
            }
        }

        
    }

    public void HacerTrampa()
    {
        MenuLogin.SetActive(false);
        NicknameInput.text = resultadoNickname;
        pagBienvenida.SetActive(true);
        StartCoroutine(apagar());
    }

    IEnumerator apagar()
    {
            yield return new WaitForSeconds(3f);
            pagBienvenida.SetActive(false);
            MenuPrincipal.SetActive(true);
    }

    public void Displayajustes()
    {
        if(isajustes == true)
        {
            ajustes.SetActive(true);
            isajustes = false; 
            MenuPrincipal.SetActive(false); 
        }
        else
        {
            ajustes.SetActive(false); 
            isajustes = true;
            MenuPrincipal.SetActive(true); 
                
        }
    }
 
    //Permite a un nuevo usuario registrarse y logearse sin volver a la pantalla de inicio
    public void logearseAlRegistro()
    {
        if (email_field_Reg.text == "" || password_field_Reg.text == "")
        {
            Debug.Log("Comprueba de que los campos no esten vacios");
        }
        else
        {
            emailGRegistro = email_field_Reg.text;
            passwordGRegistro = password_field_Reg.text;

            byte[] password_bytes = new UTF8Encoding().GetBytes(passwordGRegistro);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(password_bytes);
            string encoded_password = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            Debug.Log(encoded_password);

            string query = "SELECT * FROM users where email = '" + emailGRegistro + "' and password = '" + encoded_password + "';";
            Debug.Log(query);

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Reader = MS_Comand.ExecuteReader();
            while (MS_Reader.Read())
            {
                resultadoUser = MS_Reader.GetString(1);
                resultadoPassword = MS_Reader.GetString(5);
                resultadoNickname = MS_Reader.GetString(3);
                resultadoID = MS_Reader.GetString(0);

                Debug.Log(resultadoID + " " + resultadoUser + " " + resultadoPassword);
            }

            MS_Reader.Close();

            if (resultadoUser == emailGRegistro && resultadoPassword == encoded_password)
            {
                // Guardar el ID de usuario en PlayerPrefs
                PlayerPrefs.SetString("IDUsuario", resultadoID);

                PagAvatar.SetActive(false);
                NicknameInput.text = resultadoNickname;
                pagBienvenida.SetActive(true);
                StartCoroutine(apagar());

                Debug.Log("Sesión Iniciada con exito, Bienvenido " + " " + resultadoNickname);
            }
            else
            {
                Debug.Log("El usuario o contraseña son incorrectos");
            }
        }

        
    }
}
