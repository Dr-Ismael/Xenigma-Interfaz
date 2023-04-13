using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesactivarSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider = FindObjectOfType<Slider>();
        GetComponent<Button>().onClick.AddListener(Desactivar);
    }

    private void Desactivar()
    {
        slider.interactable = false;
    }
}
