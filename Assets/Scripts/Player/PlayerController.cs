using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int level = 2;
    public float levelStartDelay = 1f;
    private bool doingSetup = false;
    public float speed = 4f; //Player speed

    public AudioClip moveSound1;    //Audio walking
    public AudioClip moveSound2;
    public AudioClip eatSound1;     //Audio eat
    public AudioClip drinkSound1;   //Audio drink


    private Rigidbody2D rb;
    private Vector2 mov;

    private Animator anim;
    private Vector3 startPos = new Vector3(0, 0, 0f);
    public static PlayerController instance;

    public bool canMove; //Can the player move?

    public bool canMelee; //Can the player melee

    public bool canShoot; //Does the player have a ranged weapon?

    private PlayerHealth healthManager;

    private SpriteRenderer spriteRenderer;



    void Start()
    {
        //If player doesnt exist
        if (instance == null)
        {
            instance = this;
            canMove = true;
        } //If more than one player exists
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        healthManager = GetComponent<PlayerHealth>();

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
        if (mov.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (mov.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Animations()
    {
        //If player is moving
        if (canMove && mov != Vector2.zero)
        {
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Food")
        {
            healthManager.AddPlayerHealth(UnityEngine.Random.Range(4, 20));
            SoundManager.instance.RandomizeSfx(eatSound1, drinkSound1);
            Destroy(collider.gameObject);
        }
        else if (collider.tag == "Kela")
        {
            healthManager.AddPlayerMaxHealth(10);
            Destroy(collider.gameObject);
        }
        else if (collider.tag == "Exit")
        {
            if (GameObject.FindWithTag("Enemy") != null || GameObject.FindWithTag("Boss") != null)
            {
                Debug.Log("Enemies alive, cant move to next level");
            }
            else if (doingSetup == false)
            {
                doingSetup = true;
                //Invoke restart function with given delay
                Invoke("Restart", levelStartDelay);

                //Disable player since level is finished.
                enabled = false;
            }
        }
    }

    private void Restart()
    {
        Debug.Log("STARTING NEW LEVEL FROM PLAYER");
        GameManager.instance.InitGame(level);
        transform.position = startPos; //Move the player to 0,0
        enabled = true; //Re-enable player after level has been loaded
        level++;
        doingSetup = false;
    }

}
