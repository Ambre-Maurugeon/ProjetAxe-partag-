using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    private Vector3 _playerPos;
    [SerializeField] private GameObject destination;

    //inTrigger
    private bool inTrigger;
    
    void Start()
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(inTrigger && Input.GetKeyDown(KeyCode.E)){
            GameObject.FindGameObjectWithTag("Player").transform.position = destination.transform.position;
            Debug.Log("Prend la porte");
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag =="Player"){
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag =="Player"){
            inTrigger = false;
        }
    }
}
