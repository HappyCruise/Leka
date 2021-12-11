using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    //Flags for moving direction
    [HideInInspector]
    public int xDir = 0;

    [HideInInspector]
    public int yDir = 0;


    public Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    public float speed = 6; //Movement Speed
    public int damage = 10;
    public int wallDamage = 1;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = transform.position;
        if (xDir == 1)
        {
            rb.velocity = transform.right * speed;
            spriteRenderer.flipX = true;
        }
        else if (xDir == -1)
        {
            rb.velocity = -transform.right * speed;
            spriteRenderer.flipX = false;
        }
        else if (yDir == 1)
        {
            rb.velocity = transform.up * speed;
        }
        else if (yDir == -1)
        {
            rb.velocity = -transform.up * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            Destroy(coll.gameObject);
            Destroy(gameObject);
            //TODO: DAMAGE ENEMY
            /* 
            coll.GetComponent<SCRIPTNAME>.TakeDamage(damage);
             */
        }
        else if (coll.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (coll.tag == "DestroyableWall")
        {
            Destroy(gameObject);
            coll.GetComponent<DestroyableWall>().DamageWall(wallDamage);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
