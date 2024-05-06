using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piques : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            Debug.Log("Contact avec un pique");
            GameObject.FindGameObjectWithTag("Player").GetComponent<Life>().TakeDamage(5);
        }
        }
}
