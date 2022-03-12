dusing System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Start variabel
    private Rigidbody2D player;
    private Animator anim;
    private Collider2D coll;

    //Statement
    private enum State {idle, Run, Jumping, falling, hurt}
    private State state = State.idle;

    //inspector Variabel
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private int Cherrys = 0;
    [SerializeField] private Text CherrysText;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float hurtforce = 3f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Method Movement
        if(state != State.hurt){
            MovementSet();
        }
        // Method Animation
        VelocityState();
        anim.SetInteger("state",(int)state);
         
    }

    //pertambahan cherrys
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Colletable"){
            Destroy(collision.gameObject);
            Cherrys += 1;
            CherrysText.text = Cherrys.ToString();
        }
    }

    private void MovementSet(){

          // Script bergerak
        float Horizontal = Input.GetAxis("Horizontal");

        if (Horizontal < 0){
            player.velocity = new Vector2(-speed, player.velocity.y);
            transform.localScale = new Vector2(-1, 1);  
        }

        else if (Horizontal > 0){
            player.velocity = new Vector2(speed, player.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        // Script Lompat
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)){
            player.velocity = new Vector2(player.velocity.x, JumpForce);
            state = State.Jumping;
        }

        // script lambat
        if(Mathf.Abs(Horizontal) < 0.4f && coll.IsTouchingLayers(ground)){
            player.drag = linearDrag;
        }else{
            player.drag = 0f;
        }

    }
 
    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy"){
            if (state == State.falling){
                Destroy(other.gameObject);
            }else{
                state = State.hurt;
                if(other.gameObject.transform.position.x > transform.position.x){
                    player.velocity = new Vector2(-hurtforce, player.velocity.y);
                }else{
                    player.velocity = new Vector2(hurtforce, player.velocity.y);
                }
            }
        }
    }

    // Script pergantian Animation
    private void VelocityState(){
        if (state == State.Jumping){
            if (player.velocity.y < .1f )
            {
                state = State.falling;
            }
        }else if (state == State.falling){
            if (coll.IsTouchingLayers(ground)){
                state = State.idle;
            }
        }else if(state == State.hurt) {
            if(Mathf.Abs(player.velocity.x) < .1f){
                state = State.idle;
            }
        }else if (Mathf.Abs(player.velocity.x) > 2f){
            state = State.Run;
        }else{
            state = State.idle;
        }
    }
}
