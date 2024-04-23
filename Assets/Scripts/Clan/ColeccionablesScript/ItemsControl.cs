using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ItemsControl : MonoBehaviour
{

    //Lista de coleccionables a usar
    public List<ListaColeccionables> Coleccionables = new List<ListaColeccionables>();
    public List<ListaColeccionables> Medallas = new List<ListaColeccionables>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

}

[System.Serializable]
public class ListaColeccionables
{
    public Sprite imgColeccionable;
    public int IDColeccionable;
    public byte active;
}


public class ListaMedallas
{
    public Sprite imgMedalla;
    public int IDMedalla;
    public byte active;
}

