using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] sceneMusic; // Array de música para cada escena
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject); // Para que el objeto persista entre escenas
        SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex;

        // Comprobar si hay música asignada para la escena actual
        if (sceneIndex < sceneMusic.Length && sceneMusic[sceneIndex] != null)
        {
            // Detener la música anterior y reproducir la nueva música
            audioSource.Stop();
            audioSource.clip = sceneMusic[sceneIndex];
            audioSource.Play();
        }
    }
}
