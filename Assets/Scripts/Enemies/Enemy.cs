using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int playerDamage; //The amount of health points to subtract from the player when attacking.

    public bool isBoss = false;

    public bool isGhost = false;

    public AudioClip attackSound1; //First of two audio clips to play when attacking the player.

    public AudioClip attackSound2;

    public float moveTime = 0.1f; //Time it will take object to move, in seconds.

    public LayerMask blockingLayer; //Layer on which collision will be checked.

    public float speed = 1f;

    public Animator animator; //Variable of type Animator to store a reference to the enemy's Animator component.

    public Transform target;

    public bool canMove;

    public BoxCollider2D boxCollider; //The BoxCollider2D component attached to this object.

    public Rigidbody2D rb2D; //The Rigidbody2D component attached to this object.

    public float inverseMoveTime; //Used to make movement more efficient.

    public bool isMoving; //Is the object currently moving.

    public float startTimeBetweenAttacks = 1.0f;

    public float timeBetweenAttacks;

    private PlayerHealth healthManager;

    public SpriteRenderer spriteRenderer;

    public float startTime = 0.5f;

    //Protected, virtual functions can be overridden by inheriting classes.
    void Start()
    {
        GameManager.instance.AddEnemyToList(this);

        spriteRenderer = GetComponent<SpriteRenderer>();

        //Get and store a reference to the attached Animator component.
        animator = GetComponent<Animator>();

        //Find the Player GameObject using it's tag and store a reference to its transform component.
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    private void FixedUpdate()
    {
    }

    void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime; //Decrease attack time
            MoveEnemy();
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        if (isBoss)
        {
            if (xDir == -1)
                spriteRenderer.flipX = true;
            else if (xDir == 1) spriteRenderer.flipX = false;
        }
        else
        {
            if (xDir == 1)
                spriteRenderer.flipX = true;
            else if (xDir == -1) spriteRenderer.flipX = false;
        }

        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);

        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;

        //Check if nothing was hit and that the object isn't already moving.
        if (hit.transform == null && !isMoving)
        {
            //Start SmoothMovement co-routine passing in the Vector2 end as destination
            StartCoroutine(SmoothMovement(end));

            //Return true to say that Move was successful
            return true;
        }

        //If something was hit, return false, Move was unsuccesful.
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //The object is now moving.
        isMoving = true;

        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter.

        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion =
                Vector3
                    .MoveTowards(rb2D.position,
                    end,
                    inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition (newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }

        //Make sure the object is exactly at the end of its movement.
        rb2D.MovePosition (end);

        //The object is no longer moving.
        isMoving = false;
    }

    void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        //Hit will store whatever our linecast hits when Move is called.
        RaycastHit2D hit;

        //Set canMove to true if Move was successful, false if failed.
        bool canMove = Move(xDir, yDir, out hit);

        //Check if nothing was hit by linecast
        if (hit.transform == null)
            //If nothing was hit, return and don't execute further code.
            return;

        //Get a component reference to the component of type T attached to the object that was hit
        T hitComponent = hit.transform.GetComponent<T>();

        //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
        if (!canMove && hitComponent != null)
            //Call the OnCantMove function and pass it hitComponent as a parameter.
            OnCantMove(hitComponent);
    }

    public void MoveEnemy()
    {
        //Declare variables for X and Y axis move directions, these range from -1 to 1.
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        int xDir = 0;
        int yDir = 0;

        if (target.position.x > transform.position.x)
            xDir = 1;
        else if (target.position.x < transform.position.x) xDir = -1;

        if (target.position.y > transform.position.y)
            yDir = 1;
        else if (target.position.y < transform.position.y) yDir = -1;

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<PlayerHealth> (xDir, yDir);
    }

    void OnCantMove<T>(T component)
        where T : Component
    {
        //Declare hitPlayer and set it to equal the encountered component.
        PlayerHealth hitPlayer = component as PlayerHealth;

        if (timeBetweenAttacks <= 0)
        {
            //Call the HurtPlayer function of hitPlayer passing it playerDamage, the amount of healthpoints to be subtracted.
            hitPlayer.HurtPlayer (playerDamage);
            timeBetweenAttacks = startTimeBetweenAttacks;
			//Set the attack trigger of animator to trigger Enemy attack animation.
		SoundManager.instance.PlaySingle (attackSound1);
        animator.SetTrigger("enemyAttack");
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isGhost)
        {
            if (collider.tag == "DestroyableWall")
            {
                //If ready to attack
                if (timeBetweenAttacks <= 0)
                {
					
                    animator.SetTrigger("enemyAttack");
					SoundManager.instance.PlaySingle (attackSound2);
                    // StartCoroutine(DamageWall(collider));
                    if (collider != null)
                    {
                        collider.GetComponent<DestroyableWall>().DamageWall(1);
                    }
                    timeBetweenAttacks = startTimeBetweenAttacks; //Reset attack time
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (isGhost)
        {
            Debug.Log(collider.gameObject.tag);

            if (collider.gameObject.tag == "Player")
            {
                Debug.Log(collider.gameObject.layer);
                if (timeBetweenAttacks <= 0)
                {
					animator.SetTrigger("enemyAttack");
					
                    //Call the HurtPlayer function of hitPlayer passing it playerDamage, the amount of healthpoints to be subtracted.
                    collider
                        .gameObject
                        .GetComponent<PlayerHealth>()
                        .HurtPlayer(playerDamage);
                    timeBetweenAttacks = startTimeBetweenAttacks;
					SoundManager.instance.PlaySingle (attackSound2);
                }
            }
        }
    }
}
