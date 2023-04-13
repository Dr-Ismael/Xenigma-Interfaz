using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositioning : MonoBehaviour

    
{
    public GameObject player;
    public GameObject[] spawnables;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnables.Length > 0) {
            GameObject closestSpawnable = spawnables[0];
            foreach (GameObject spawnable in spawnables) {
                if (Vector3.Distance(player.transform.position, spawnable.transform.position) < Vector3.Distance(player.transform.position, closestSpawnable.transform.position)) {
                    closestSpawnable = spawnable;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, closestSpawnable.transform.position, speed * Time.deltaTime);
        }
        
    }
}
