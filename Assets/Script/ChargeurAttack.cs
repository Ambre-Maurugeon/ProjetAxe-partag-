using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeurAttack : MonoBehaviour
{
    //Charge
    [Header("Charge")]
    [SerializeField] private float distance=10;
    [SerializeField] private float speedAttack=1.5f;

    //Detection du Player
    [Header("Player Detection")]
    [SerializeField] private Transform[] _detectionPoints;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _detectionLength = 0.05f;

    
    //Charge
    private Vector3 destination;

    //
    private bool playingCharge = false;

    //Physics
    private Rigidbody2D _rb;
    private SpriteRenderer skin;
    //private GameObject player;

    //Anim
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>();  
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {   
        //animCheck();

        if(GetComponent<ScreenVisibility>().OnScreen == true && !playingCharge){
            anim.SetTrigger("prepCharge");
            Debug.Log("l'attaque du chargeur commence");
            GetOrientAttack();
            Invoke("Charge",5f);
        } else if (playingCharge){
            Charge();
        }
    }

    void Charge(){
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, destination, speedAttack * Time.deltaTime);
    }

    void animCheck(){
        anim.SetFloat("run", _rb.velocity.x);
    }

    void GetOrientAttack(){
        playingCharge=true;
        float orientX = Detection();
        destination = new Vector3(transform.localPosition.x + distance * orientX,transform.localPosition.y,transform.localPosition.z);
    }

    private int Detection(){
         foreach (Transform detectionPoint in _detectionPoints){
            RaycastHit2D hitRight = Physics2D.Raycast(
                detectionPoint.position, //origine
                Vector2.right,            //direction
                _detectionLength,
                _playerMask             // type d'objets à détecter
            );

            RaycastHit2D hitLeft = Physics2D.Raycast(
                detectionPoint.position, // origine
                Vector2.left,                // direction 
                _detectionLength
                //,_playerMask                 
            );

            if(hitRight.collider != null){
                Debug.Log("joueur detecté sur la droite");
                skin.flipX=false;
                return 1;
            }else if (hitLeft.collider != null){
                Debug.Log("joueur detecté sur la gauche");
                skin.flipX=true;
                return -1;
            }
        }
        Debug.Log("Pas de joueur");
        return 0;
    }

//Voir les raycast
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red; // Change color as desired

    //     // Draw the raycast line from origin to maximum detection length
    //     Gizmos.DrawLine(detectionPoint.position, detectionPoint.position + Vector2.right * _detectionLength);

    //     // Draw a sphere at the end of the raycast line (optional)
    //     if (hitRight.collider != null)
    //     {
    //         Gizmos.DrawSphere(hitRight.point, 0.1f); // Adjust sphere size as needed
    //     }
    // }
}
