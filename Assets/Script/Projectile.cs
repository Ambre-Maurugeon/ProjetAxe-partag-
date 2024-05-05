using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private int fireDmg = 15;
   private float direction;
   private bool hit;
   private float lifetime;

   private BoxCollider2D boxCollider;
   private Animator anim;

   void Awake(){
    anim=GetComponent<Animator>();
    boxCollider=GetComponent<BoxCollider2D>();
   }

   void Update()
   {
    if(hit) return;
    float movementSpeed = speed *Time.deltaTime* direction;
    transform.Translate(movementSpeed,0,0);

    lifetime += Time.deltaTime;
    if(lifetime > 2.5f){
        anim.SetTrigger("explode");
    }
    if(lifetime > 3){
        gameObject.SetActive(false);
    }
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
    if (collision.tag == "Player"){
        hit = true;
        boxCollider.enabled=false;
        anim.SetTrigger("explode");
        //Life.ActualHealth -= fireDmg;
        FindObjectOfType<Life>().TakeDamage(15);
    }
   }

   public void SetDirection(float _direction)
   {
    lifetime =0;
    direction = _direction;
    Debug.Log("direction"+ direction);
    gameObject.SetActive(true);
    hit = false;
    boxCollider.enabled=true;

    float localScaleX = transform.localScale.x;
    if(Mathf.Sign(localScaleX)!=_direction){
        localScaleX=-localScaleX;
    }

    transform.localScale = new Vector3(localScaleX,transform.localScale.y,transform.localScale.z);    
   }

   private void Deactivate(){
    gameObject.SetActive(false);
   }
}
