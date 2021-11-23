using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f; //Player speed

    private Rigidbody2D rb;

    private Vector2 mov;

    private Animator anim;

    public static PlayerController instance;

    public bool canMove; //Can the player move?

    public bool canSword; //Does the player have a sword?

    public bool canShoot; //Does the player have a ranged weapon?

    public int health = 100;
    void Start()
    {
        //If player doesnt exist
        if (instance == null)
        {
            instance = this;
            canMove = true;
        }
        else //If more than one player exists
        if (instance != this)
        {
            Destroy (gameObject);
        }

        //Initialise RigidBody and Animator
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movements();
        Animations();
    }

    void Movements()
    {
        //Direction of player
        mov =
            new Vector2(Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"));
    }

    void Animations()
    {
        //If player is moving
        if (canMove && mov != Vector2.zero)
        {
            //TODO: ADD ANIMATIONS;
        }
        else
        {
            //If player cant move
            //TODO: STOP ANIMATION AND START IDLE ANIMATION
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //moving the player
            rb.MovePosition(rb.position + mov * speed * Time.deltaTime);
        }
    }
    public void HurtPlayer(int damageToGive){
        health -= damageToGive;
    }
}
