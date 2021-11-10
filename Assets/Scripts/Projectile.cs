using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed; //Projectile speed

    GameObject player;
    Rigidbody2D rb;
    Vector3 target, dir; //Target location and direction

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody2D>();

        //Does player exist?
        if (player != null)
        {
            target = player.transform.position; //Set target to player location
            dir = (target - transform.position).normalized; //Direction vector between player and projectile;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Does the target exist?
        if (target != Vector3.zero)
        {
            rb.MovePosition(transform.position + dir * speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Did the projectile hit player?
        if (collider.transform.tag == "Player")
        {
            //Yes, destroy projectile
            Destroy(gameObject);
        }
    }

    //If projectile goes out of screen and is no longer visible, destroy it.
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
