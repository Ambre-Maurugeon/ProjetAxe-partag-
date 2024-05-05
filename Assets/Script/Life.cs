using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public static int InitialHealth=200;
    public static int ActualHealth;

    public bool invincible = false;


    [SerializeField] private Text _txtVie;
    private Collider2D _monColl;
    private Animator anim;


    void Awake(){
        _monColl = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        //GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Start is called before the first frame update
    void Start()
    {
        InitCurrentLife();
    }

    // Update is called once per frame
    void Update()
    {
        isALive();
        RefreshUI();
    }

    public void TakeDamage(int damage){
        if (!invincible){
            StartCoroutine(Invicibility());
            anim.SetTrigger("hit");
            ActualHealth -= damage;
            if(ActualHealth<=0){
                Debug.Log("Le joueur est mort");
            }
        }
    }

    IEnumerator Invicibility(){
        invincible = true;
        yield return new WaitForSeconds(1.5f); // tps d'invincibilité
        invincible = false;
    }

    void InitCurrentLife(){
        ActualHealth = InitialHealth;
    }

    void RefreshUI(){
        if (ActualHealth<0){
            ActualHealth=0;
        }
        _txtVie.text = ActualHealth + " / " + InitialHealth;
    }

    void isALive(){
        if (ActualHealth<=0){ 
            //changer pos en le dernier de la liste c'est à dire .Count - 1
            transform.position = CheckPoint.checkpoint[CheckPoint.checkpoint.Count-1];
            ActualHealth = InitialHealth;
        }
    }
    
}
