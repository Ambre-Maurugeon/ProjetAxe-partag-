using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static List<Vector3> checkpoint = new List<Vector3>();
    private GameObject player;
    private bool AlreadyTaken=false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player" && !AlreadyTaken) {
            // ajouter à la liste de position la position de ce gameobject
            checkpoint.Add(transform.position);
            for(int i = 0; i < checkpoint.Count; i++)
            {
                Debug.Log("Element " + i + ": " + checkpoint[i]);
            }
            AlreadyTaken = true;
        }
    }
}