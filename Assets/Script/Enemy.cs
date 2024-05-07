using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Defense")]
    [SerializeField] private int InitialHealth=50;

    [Header("Attaque")]
    [SerializeField] private int damageColl;

    [Header("UI")]
    [SerializeField] private Text _txtVie;

    private Life _lifeScript;   //recup script vie player pr la fonction TakeDamage()

    private bool inTrigger=false;
    
    private int ActualHealth;

    //Ennemi
    private SpriteRenderer skin;
    private Rigidbody2D rb;
    private Animator anim;
    //Player
    private GameObject player;
    private Rigidbody2D rb_player;
    private Animator animPlayer;
    

    //Mouvement
    [SerializeField]
    private Vector3[] positions;
    private int index;




    void Start()
    {
        _lifeScript=FindObjectOfType<Life>();
        InitCurrentLife();
        _txtVie.text="test";

        player = GameObject.FindGameObjectWithTag("Player");
        rb_player= player.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animPlayer = player.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        skin= GetComponent<SpriteRenderer>();   

        float posX = GetComponent<Transform>().position.x;
        float posY = GetComponent<Transform>().position.y;
        float posZ = GetComponent<Transform>().position.z;

        //Vector3[] positions = {new Vector3(posX+2, posY, posZ), new Vector3(posX-2, posY, posZ)};
        //positions[0] = new Vector3(posX+2, posY, posZ);
        //positions[1] = new Vector3(posX-2, posY, posZ);
    }

    // Update is called once per frame
    void Update()
    {
        animCheck();
        if(ActualHealth <= 0){
                // if (anim.HasTrigger("mort")){
                //     anim.SetTrigger("mort");
                // }
                //Invoke("Destroy(gameObject)",1f);

                Destroy(gameObject);
            }
        if (positions.Length != 0){
            Move();
        }
        if (Input.GetKeyDown(KeyCode.E)){
            StartCoroutine(AttackOpponent(10));
        }
        //StartCoroutine(AttackOpponent(10));
        RefreshUI();
    }

    void InitCurrentLife(){
        ActualHealth = InitialHealth;
    }

    public IEnumerator AttackOpponent(int dmg){
        //if(inTrigger && Input.GetKeyDown(KeyCode.E)){
            yield return new WaitForSeconds(0.5f);  // attendre le temps du déclenchement de l'anim
            if(inTrigger){
                StartCoroutine(CallPlayerInvincibility());
                ActualHealth -= dmg;
            } 
        //}
        
    }

    //Position
    private void Move(){
        transform.position = Vector2.MoveTowards(transform.position, positions[index], 3 * Time.deltaTime);
        if (transform.position == positions[index]){
            skin.flipX = !skin.flipX;
            // si l'index atteint la fin de la liste, recommencer
            if(index == positions.Length-1) {
                index = 0;
            }
            else{
                index++;
            }
        }
    }

    //IEnumerator
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            //while (Life.ActualHealth>0){
                if (collision.contacts[0].normal.y<0){       // contact par le haut de l'objet
                    ActualHealth -= 5;
                    rb_player.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);    
                }
                else{
                    _lifeScript.TakeDamage(damageColl);
                    //rb_player.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
                }
                if(collision.contacts[0].normal.x<0){
                    rb_player.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
                }
                else{
                    rb_player.AddForce(Vector2.left * 10f, ForceMode2D.Impulse);
                }
                //yield return new WaitForSeconds(0.5f);
            //}
            //for (int i= 0; i<6; i++){ 
            //}
        }
    }

    
    private IEnumerator CallPlayerInvincibility(){
        Debug.Log("début d'invicibilité");
        player.GetComponent<Life>().invincible = true; //tps d'invincibilité à check 
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Life>().invincible = false;
        Debug.Log("fin d'invicibilité");
    }

    //Visuals
    void animCheck() {
        if(positions.Length != 0){
            anim.SetBool("Move", positions.Length != 0);
        }   
    }

    void RefreshUI(){
        if (ActualHealth<0){
            ActualHealth=0;
        }
        _txtVie.text = ActualHealth + " / " + InitialHealth;
    }


    //inTrigger
    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=true;
        }
    }

    void OnTriggerExit2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=false;
        }
    }
}
