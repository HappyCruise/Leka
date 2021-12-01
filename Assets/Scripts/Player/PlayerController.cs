using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float levelStartDelay = 1f;
    public float speed = 4f; //Player speed

    private Rigidbody2D rb;
    private Vector2 mov;
    private Animator anim;

    public static PlayerController instance;
    
    public bool canMove; //Can the player move?
    public bool canMelee; //Can the player melee
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
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Enemy"){
            HurtPlayer(20);
        }
        else if(collider.tag == "Exit"){
            if(GameObject.FindWithTag("Enemy") != null){
                Debug.Log("Enemies alive, cant move to next level");
            }else{
                //Invoke restart function with given delay
                Invoke ("Restart", levelStartDelay);
                //Disable player since level is finished.
                enabled = false;
            }
        }
    }
    private void Restart(){
        SceneManager.LoadScene(0);
    }

    public void HurtPlayer(int damageToGive){
        health -= damageToGive;
        
        CheckIfGameOver();
    }
    public void CheckIfGameOver(){
        if(health <= 0){
            GameManager.instance.GameOver();
        }
    }
}
