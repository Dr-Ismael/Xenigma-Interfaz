using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lr_LineController : MonoBehaviour
{
    public Transform playerTarget;
    public GameObject spawneable;
    public LineRenderer lineRenderer;
    public TextMesh distanceText;
    public Toggle btnsinruta;
    public GameObject spawnObject;

    public void Btnsinruta(bool toggle)
    {
        lineRenderer.enabled = toggle;
        distanceText.gameObject.SetActive(toggle);
    }

    void Update()
    {
        GameObject[] spawneables = GameObject.FindGameObjectsWithTag("Spawnable");
        GameObject nearestSpawn = null;
        float minDistance = Mathf.Infinity;

        // Busca el objeto "Spawnable" más cercano
        foreach (GameObject spawn in spawneables)
        {
            float distance = Vector3.Distance(playerTarget.position, spawn.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSpawn = spawn;
            }
        }

        // Actualiza el texto de la distancia
        if (nearestSpawn != null)
        {
            distanceText.text = minDistance.ToString("F2") + "m";
            lineRenderer.SetPosition(0, playerTarget.position);
            lineRenderer.SetPosition(1, nearestSpawn.transform.position);

            // Actualiza la posición del objeto "Spawn" al final de la línea
            spawnObject.transform.position = nearestSpawn.transform.position;
            spawnObject.SetActive(true);
        }
        else
        {
            distanceText.text = "No Spawnable found";
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);

            // Desactiva el objeto "Spawn"
            spawnObject.SetActive(false);
        }
    }
}
