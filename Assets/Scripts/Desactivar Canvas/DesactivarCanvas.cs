using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarCanvas : MonoBehaviour
{
    public GameObject canvas;

    public void Desactivar()
    {
        canvas.SetActive(false);
    }
}
