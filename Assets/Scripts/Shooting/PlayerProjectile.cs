using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float xDir =0;

    public float yDir =0;

    public Rigidbody2D rb;
    public float speed =6;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = transform.position;
        if(xDir == 1){
            rb.velocity = transform.right * speed;
    
        }else if(xDir == -1 ){

            rb.velocity = -transform.right * speed;
        }
        else if(yDir == 1){
            rb.velocity = transform.up * speed;

            
        }else if(yDir == -1){
            rb.velocity = -transform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll){
        if(coll.tag == "Enemy"){
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }

        else if(coll.tag == "Wall"){
            Destroy(gameObject);
            
        }
    }

    private void OnBecameInvisible(){
        Destroy(gameObject);
    }
}
