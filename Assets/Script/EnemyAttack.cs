using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private Animator anim;
    private Enemy enemyMovement;
    private float cooldownTimer = Mathf.Infinity;

    //private bool inTrigger;

    private void Awake(){
        anim= GetComponent<Animator>();
        enemyMovement= GetComponent<Enemy>();
    }

    void Update(){
        if(Input.GetMouseButton(0) && cooldownTimer>attackCooldown){
            Attack();
        }
        cooldownTimer+= Time.deltaTime;
    }

    private void Attack(){
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        
        foreach (var fireball in fireballs) {
            if (!fireball.activeInHierarchy) {
                fireball.transform.position = firePoint.position;
                fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
                break;
            }
        }

        //fireballs[FindFireball()].transform.position = firePoint.position;
        //fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball(){
        for(int i=0;i<fireballs.Length;i++){
            if(!fireballs[i].activeInHierarchy){
                return i;
            }
        }
        return 0;
    }

//inTrigger
    //  void OnTriggerEnter2D(Collider2D truc)
    // {
    //     if (truc.tag == "Player") {
    //         inTrigger=true;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D truc)
    // {
    //     if (truc.tag == "Player") {
    //         inTrigger=false;
    //     }
    // }
}