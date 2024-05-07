using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [Header("Horizontal Mvt")]
    [Range(2,10)]
    public float NormalSpeed;

    [Header("Vertical Mvt")]
    [Range(1,10)]
    public float jump ;

    [Header("Grounded")]
    public float deccalageGroundcheck = -1;

    [Header("Dash")]
    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D _rb;
    private Collider2D _monColl;
    private SpriteRenderer _skin;
    private Animator anim;

    //OrientX
    private float _orientX =1;
    private float _moveDirX = 0f;

    //Grounded
    private bool grounded;
    private Collider2D[] colls;

    //Jump 
    private int bonusJump;

    //WallJump
    private bool IsTouchingWall=false;
    [HideInInspector]
    public bool IsWallJumping=false ;

    //Dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 30f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private float horizontal;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _monColl= GetComponent<Collider2D>();
        _skin = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        groundCheck();
        _ApplyWallDetection();

        GetInputMoveX();
        _ChangeOrientFromHorizontalMovement();

        controlCheck();
        //flipCheck();
        animCheck();
    }
        
    void FixedUpdate(){
        if (isDashing){
            return;
        }

       _rb.velocity = new Vector2(horizontal * NormalSpeed, _rb.velocity.y);

    }

    void controlCheck(){
        if (isDashing){
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump")){
            if(IsSliding){
                IsWallJumping = true;
                WallJumpStart();
            }
            else if(grounded){
                _rb.velocity = new Vector2(_rb.velocity.x, jump);
            }
            else if(bonusJump>0){
                _rb.velocity = new Vector2(_rb.velocity.x, jump*2/3);
                bonusJump=0;
            }
        }

        // if(grounded && Input.GetButtonDown("Jump")){
        // }
        // if(!grounded && Input.GetButtonDown("Jump") && bonusJump>0){
        //     _rb.velocity = new Vector2(_rb.velocity.x, jump*2/3);
        //     bonusJump=0;
        // }

        if(Input.GetKeyDown(KeyCode.LeftShift)&& canDash){
            StartCoroutine(Dash());
        }

        // if(IsSliding && Input.GetKeyDown(KeyCode.Space)){
            
        // }

        flipCheck();
    }

//OrientX
    private void _ChangeOrientFromHorizontalMovement(){
        if(_moveDirX == 0f) return ; // et si pas d'accolades le if execute juste la ligne d'apres 
        _orientX = Mathf.Sign(_moveDirX);
    }

    private void GetInputMoveX(){
        _moveDirX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q)){
            _moveDirX = -1f;
        }
        if (Input.GetKey(KeyCode.D)){
            //Debug.Log(_orientX);
            _moveDirX = 1f;
        }
    }

    void groundCheck(){
        grounded = false;
        colls = Physics2D.OverlapCircleAll(transform.position + Vector3.up * deccalageGroundcheck, _monColl.bounds.extents.x * 0.9f);
        foreach(Collider2D coll in colls) {
            if(coll != _monColl && !coll.isTrigger) {
                grounded = true;
                bonusJump=1;
                break;
            }
        }
    }

//Dash
    private IEnumerator Dash(){
        canDash = false;
        isDashing=true;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.velocity = new Vector2(horizontal * dashingPower, 0f);
        tr.emitting= true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        _rb.gravityScale=originalGravity;
        isDashing = false; 
        yield return new WaitForSeconds(dashingCooldown);
        canDash=true;
    }


//Sliding
    private void _ApplyWallDetection(){
            IsTouchingWall = GetComponent<WallDetector>().DetectWallNearBy();
        }
    public bool IsSliding => IsTouchingWall && !grounded;

//Wall jump01
//Wall Jump
    private float _wallJumpTimer;

    public void WallJumpStart(){
        _orientX = -WallDetector.orientDetection;
        _wallJumpTimer = 0f; 
        _UpdateWallJump();
    }

    private void _UpdateWallJump(){
        _wallJumpTimer += Time.deltaTime;
        if(_wallJumpTimer < 0.5f){
            Vector2 velocity = _rb.velocity;
            
            horizontal = 9*_orientX;
            velocity.y = 6;
            //Debug.Log("orientX " + _orientX);

            //_rb.velocity = velocity;
            _rb.velocity = new Vector2(horizontal, 6f);

        } else if(!grounded) {
            //Debug.Log("tombe"); 
        }
        else if (grounded){
            IsWallJumping = false;
        }
    }

//wall Jump02

    private float jumpForce = 10f; // Force de saut vertical lors du wall jump
    private float wallJumpHorizontalForce = 50f; // Force horizontale lors du wall jump

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("wall"))
    //     // && Input.GetButtonDown("Jump"))
    //     {
    //         Debug.Log("contact");
    //         RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 3f, wallLayer);
    //         RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 3f, wallLayer);
    //         Debug.Log("hit calculé");

    //         if (hitRight.collider != null && hitLeft.collider == null)
    //         {
    //             Debug.Log("contact droit");
    //             WallJump(Vector2.left);
    //             Debug.Log("Wall jump vers la gauche");
    //         }
    //         else if (hitLeft.collider != null && hitRight.collider == null)
    //         {
    //             Debug.Log("contact gauche");
    //             WallJump(Vector2.right);
    //             Debug.Log("Wall jump vers la droite");
    //         }
    //     }
    // }

    // void WallJump(Vector2 direction)
    // {
    //     _rb.velocity = Vector2.zero; // Réinitialise la vélocité
    //     _rb.AddForce(direction * wallJumpHorizontalForce, ForceMode2D.Impulse); // Ajoute la force horizontale
    //     _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Ajoute la force verticale
    // }


//Anim
    void flipCheck() {
        if(Input.GetAxisRaw("Horizontal") < 0) {
            _skin.flipX = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0) {
            _skin.flipX = false;
        }
    }

    void animCheck() {
        anim.SetFloat("velocityX", Mathf.Abs(_rb.velocity.x));
        anim.SetFloat("velocityY", _rb.velocity.y);
        anim.SetBool("grounded", grounded);
        if (Input.GetKeyDown(KeyCode.E)){
            anim.SetTrigger("attack");
        }
    }

    private void OnDrawGizmos() {
        if(_monColl == null) {
            _monColl = GetComponent<CapsuleCollider2D>();
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * deccalageGroundcheck, _monColl.bounds.extents.x * 0.9f);
    }
}
