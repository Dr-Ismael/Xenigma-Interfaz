using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Registrar : MonoBehaviour
{
    public string password_regex;

    public TMP_Text pass_mayuculas,
        pass_minusculas,
        pass_digito,
        pass_especial_caracter,
        pass_8letras,
        pass_confirm,
        userBirthday,
        userGender;

    private string connectionString;

    private MySqlConnection MS_Connection;

    private MySqlCommand MS_Comand;

    public InputField nombre_field,
        nickname_field,
        email_field,
        password_field,
        passwordConfirm_field;

    private MySqlDataReader MS_Reader;

    private bool email_valid = false;
    private bool password_valid = false;
    private bool password_confirm_valid = false;

    [SerializeField]
    GameObject MenuAvatar;

    [SerializeField]
    GameObject MenuRegistro;

    [SerializeField]
    GameObject MenuLogin;

    [SerializeField]
    GameObject MenuInicio;

    private string nombreG;
    private string nicknameG;
    private string edadG;
    private string emailG;
    private string passwordG;
    public int idAvatarG;
    public string birthdaryG;
    private string resultado;
    public TMP_Text RespuestaText;

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

        //IrAlInicio();
    }

    public void email_check()
    {
        Match match = Regex.Match(email_field.text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        if (match.Success)
        {
            email_valid = true;
            email_field.image.color = Color.green;
        }
        else
        {
            email_valid = false;
            email_field.image.color = Color.red;
        }
    }

    public void password_Check()
    {
        Match match = Regex.Match(password_field.text, password_regex);
        if (match.Success)
        {
            password_valid = true;
            password_field.image.color = Color.green;
        }
        else
        {
            password_valid = false;
            password_field.image.color = Color.red;
        }

        //Mayusculas
        if (Regex.IsMatch(password_field.text, @"(?=.*?[A-Z])"))
        {
            pass_mayuculas.color = Color.green;
        }
        else
        {
            pass_mayuculas.color = Color.red;
        }

        //Minusculas
        if (Regex.IsMatch(password_field.text, @"(?=(.*[a-z]){1,})"))
        {
            pass_minusculas.color = Color.green;
        }
        else
        {
            pass_minusculas.color = Color.red;
        }

        //Digito
        if (Regex.IsMatch(password_field.text, @"(?=(.*[\d]){1,})"))
        {
            pass_digito.color = Color.green;
        }
        else
        {
            pass_digito.color = Color.red;
        }

        //Caracter Especial
        if (Regex.IsMatch(password_field.text, @"(?=(.*[\W]){1,})"))
        {
            pass_especial_caracter.color = Color.green;
        }
        else
        {
            pass_especial_caracter.color = Color.red;
        }

        //Cantdad de letras/caracteres
        if (Regex.IsMatch(password_field.text, @".{8,}$"))
        {
            pass_8letras.color = Color.green;
        }
        else
        {
            pass_8letras.color = Color.red;
        }
    }

    public void passowrd_confirm_check()
    {
        if (password_field.text == passwordConfirm_field.text)
        {
            passwordConfirm_field.image.color = Color.green;
            pass_confirm.color = Color.green;
        }
        else
        {
            passwordConfirm_field.image.color = Color.red;
            pass_confirm.color = Color.red;
        }
    }

    public void registro_submit()
    {
        email_valid = Regex
            .Match(email_field.text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
            .Success;
        password_valid = Regex.Match(password_field.text, password_regex).Success;
        if (password_field.text == passwordConfirm_field.text)
        {
            password_confirm_valid = true;
        }
        else
        {
            password_confirm_valid = false;
        }

        if (
            nombre_field == true
            && nickname_field == true
            && userBirthday == true
            && email_valid == true
            && password_valid == true
            && password_confirm_valid == true
        )
        {
            emailG = email_field.text;

            string query = "SELECT * FROM users where email = '" + emailG + "';";

            MS_Connection = new MySqlConnection(connectionString);
            MS_Connection.Open();

            MS_Comand = new MySqlCommand(query, MS_Connection);

            MS_Reader = MS_Comand.ExecuteReader();
            while (MS_Reader.Read())
            {
                resultado = MS_Reader.GetString(1);
                Debug.Log(resultado);
            }
            MS_Reader.Close();

            if (resultado == emailG)
            {
                Debug.Log("Correo existente");
            }
            else
            {
                passwordG = password_field.text;
                nombreG = nombre_field.text;
                nicknameG = nickname_field.text;
                // Recibir la fecha de nacimiento en formato "día/mes/año" o "mes/día/año"
                string fechaNacimientoString = userBirthday.text;

                DateTime fechaNacimiento;

                // Intentar analizar la fecha con el primer formato "día/mes/año"
                if (
                    DateTime.TryParseExact(
                        fechaNacimientoString,
                        "d/M/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out fechaNacimiento
                    )
                )
                {
                    // Formatear la fecha de nacimiento en el formato "año-mes-día"
                    edadG = fechaNacimiento.ToString("yyyy-MM-dd");
                }
                // Si no se pudo analizar con el primer formato, intentar con el segundo formato "mes/día/año"
                else if (
                    DateTime.TryParseExact(
                        fechaNacimientoString,
                        "M/d/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out fechaNacimiento
                    )
                )
                {
                    // Formatear la fecha de nacimiento en el formato "año-mes-día"
                    edadG = fechaNacimiento.ToString("yyyy-MM-dd");
                }
                else
                {
                    // Manejar el caso en el que la cadena de fecha no pueda ser analizada
                    // Aquí puedes mostrar un mensaje de error o tomar alguna otra acción adecuada
                    Debug.LogError("La fecha de nacimiento no tiene un formato válido.");
                }

                birthdaryG = userGender.text;

                Debug.Log(
                    emailG
                        + " "
                        + passwordG
                        + " "
                        + nombreG
                        + " "
                        + nicknameG
                        + " "
                        + edadG
                        + " "
                        + birthdaryG
                );

                MenuAvatar.SetActive(true);
                MenuRegistro.SetActive(false);
            }
        }
    }

    public void SeleccionarAvatar()
    {
        Debug.Log(idAvatarG);
    }

    public void regresar()
    {
        MenuInicio.SetActive(false);
        MenuLogin.SetActive(false);
        MenuAvatar.SetActive(false);
        Limpiar();
        MenuRegistro.SetActive(true);
    }

    private void Limpiar()
    {
        emailG = "";
        passwordG = "";
        nombreG = "";
        edadG = "";
        nicknameG = "";
        birthdaryG = "";

        email_field.text = emailG;
        password_field.text = passwordG;
        userBirthday.text = birthdaryG;
        nickname_field.text = nicknameG;
        nombre_field.text = nombreG;
        passwordConfirm_field.text = passwordG;
        userGender.text = birthdaryG;
    }

    public void guaradr()
    {
        byte[] password_bytes = new UTF8Encoding().GetBytes(passwordG);
        byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(
            password_bytes
        );
        string encoded_password = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

        string query =
            "INSERT INTO users(id, nombre, nickname, edad, email, password, idAvatar, puntajeUser, genero) VALUES (null, '"
            + nombreG
            + "', '"
            + nicknameG
            + "', '"
            + edadG
            + "', '"
            + emailG
            + "', '"
            + encoded_password
            + "', '"
            + idAvatarG
            + "', 0, '"
            + birthdaryG
            + "');";

        MS_Connection = new MySqlConnection(connectionString);
        MS_Connection.Open();
        try
        {
            MS_Comand = new MySqlCommand(query, MS_Connection);
            MS_Comand.ExecuteNonQuery();
            Debug.Log("Registro");
            RespuestaText.text = "Registro exitoso";
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        MS_Connection.Close();
    }

    public void login()
    {
        MenuRegistro.SetActive(false);
        MenuAvatar.SetActive(false);
        MenuInicio.SetActive(false);
        MenuLogin.SetActive(true);
    }

    public void IrAlInicio()
    {
        MenuRegistro.SetActive(false);
        MenuAvatar.SetActive(false);
        MenuInicio.SetActive(true);
        MenuLogin.SetActive(false);
    }

    public List<ListaTotal> ObjetosListado = new List<ListaTotal>();

    public void ImagenSeleccionada(int id)
    {
        ListaTotal imagenSeleccionada = ObjetosListado.Find(imagen => imagen.ID == id);

        if (imagenSeleccionada != null)
        {
            Debug.Log("Imagen seleccionada: " + imagenSeleccionada.ID);

            //Temporalmente se usara este debido a un bug al seleccionar la imagen
            //idAvatarG = 2;
            idAvatarG = imagenSeleccionada.ID;
        }
        else
        {
            Debug.Log("No se encontró la imagen seleccionada");
        }
    }
}

[System.Serializable]
public class ListaTotal
{
    public Image Image;
    public int ID;
}
