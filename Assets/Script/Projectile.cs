using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   [SerializeField] private float speed;
   private Vector3 direction;

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
    Vector3 movement = speed * Time.deltaTime * direction;
    transform.Translate(movement);

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
        FindObjectOfType<Life>().TakeDamage(15);
    }
   }

//Direction droite gauche (Pattern1)
   public void SetDirection(float _direction)
   {
    lifetime =0;
    direction.x = _direction;
    //Debug.Log("direction"+ direction);
    gameObject.SetActive(true);
    hit = false;
    boxCollider.enabled=true;

    float localScaleX = transform.localScale.x;
    if(Mathf.Sign(localScaleX)!=_direction){
        localScaleX=-localScaleX;
    }

    transform.localScale = new Vector3(localScaleX,transform.localScale.y,transform.localScale.z);    
   }

//Follow FirePoint (Pattern2)
   public void CalculateDirection(Vector3 enemyPosition, Vector3 firePointPosition)
    {
        lifetime = 0;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        // Direction en fonction position FirePoint2
        Vector3 difference = firePointPosition - enemyPosition;

        direction = difference.normalized;

        float movementX = speed * Time.deltaTime * direction.x;
        float movementY = speed * Time.deltaTime * direction.y;
        transform.Translate(new Vector3(movementX, movementY, 0));
    }

   private void Deactivate(){
    gameObject.SetActive(false);
   }
}
