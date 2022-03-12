using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opssum : MonoBehaviour
{

    [SerializeField] private float leftcap;
    [SerializeField] private float Rifhtcap;
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask ground;
    private bool facingleft = true;

    private Rigidbody2D rb;
    private Collider2D coll;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    
    private void update()
    {
        if(facingleft){
            if (transform.position.x > leftcap){
                if(transform.position.x != 1){
                    transform.localScale = new Vector3(1, 1);
                }

                if(coll.IsTouchingLayers(ground)){
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
            
            }else{
                facingleft = false;
            
            }
        }

        else{
            if (transform.position.x < Rifhtcap){
                if(transform.localScale.x != -1){
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    transform.localScale = new Vector3(-1, 1);
                }
                if(coll.IsTouchingLayers(ground)){
                rb.velocity = new Vector2(speed, rb.velocity.y);
                }
            }
            else{
                facingleft = true;
            }
       
        }
    }
    
}
