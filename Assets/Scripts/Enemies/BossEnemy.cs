using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEnemy : Enemy
{

    private float maxHealth;
    private float currentHealth;
    private bool isDead = false;

    private void Start()
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

        isBoss = true;
        
        //maxHealth = PlayerController.instance.level * 0.7f; // Set the health to current level * 0.7 | 7, 14, 21 etc... 
        maxHealth = 5;
        currentHealth = maxHealth;
        Debug.Log("BOSS HP " + maxHealth);
    }

    private void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        if (!isDead)
            MoveBoss();
        else
            StartCoroutine(DeathAnimation());
        }


        
    }
    private void MoveBoss()
    {

        //Declare variables for X and Y axis move directions, these range from -1 to 1.
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        int xDir = 0;
        int yDir = 0;

        if (target.position.x > transform.position.x)
            xDir = 1;

        else if (target.position.x < transform.position.x)
            xDir = -1;

        if (target.position.y > transform.position.y)
            yDir = 1;

        else if (target.position.y < transform.position.y)
            yDir = -1;

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<PlayerHealth>(xDir, yDir);
    }
    public void HurtBoss(int damageToGive)
    {
        if (!isDead)
        {
            
            currentHealth -= damageToGive;
            CheckIfDied();
        }
    }

    private void CheckIfDied()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
        }
        else {
            animator.SetTrigger("enemyHit");
        }
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
            OnBossCantMove(hitComponent);
    }

    void OnBossCantMove<T>(T component)
    where T : Component
    {
        double lvlAsDouble = (double)PlayerController.instance.level;//Get current lvl and cast it as double
        double dmgToDeal = Math.Floor(lvlAsDouble * 1.5f); // Scale damage according to level

        //Declare hitPlayer and set it to equal the encountered component.
        PlayerHealth hitPlayer = component as PlayerHealth;

        Debug.Log("Boss Cant move called");
        if (timeBetweenAttacks <= 0)
        {
            StartCoroutine(HitPlayer(hitPlayer));
            // hitPlayer.HurtPlayer((int)dmgToDeal);
            timeBetweenAttacks = startTimeBetweenAttacks;
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "DestroyableWall")
        {
            //Ready to attack?
            if (timeBetweenAttacks <= 0)
            {
                
                animator.SetTrigger("enemyAttack");
                StartCoroutine(DamageWall(collider));
            }
            else
            {
                timeBetweenAttacks -= Time.deltaTime; // Decrese attack time
            }
        }
    }


    IEnumerator DamageWall(Collider2D collider)
    {
        //Wait for the duration of animation
        yield return new WaitForSeconds(0.7f);

        //If wall wasnt destroyed already
        if (collider != null)
        {
            SoundManager.instance.PlaySingle (attackSound2);
            collider.GetComponent<DestroyableWall>().DamageWall(3);
        }
    }
    IEnumerator HitPlayer(PlayerHealth hitPlayer)
    {
        double lvlAsDouble = (double)PlayerController.instance.level;//Get current lvl and cast it as double
        double dmgToDeal = Math.Floor(lvlAsDouble * 1.5f); // Scale damage according to level
        if (!isDead) {

        
        //Set the attack trigger of animator to trigger Enemy attack animation.
        animator.SetTrigger("enemyAttack");
        

        yield return new WaitForSeconds(0.7f);
        SoundManager.instance.PlaySingle (attackSound1);

        if ((transform.position.x - hitPlayer.gameObject.transform.position.x) <= 2 && (transform.position.y - hitPlayer.gameObject.transform.position.y) <= 2)
        {
            //Call the HurtPlayer function of hitPlayer passing it playerDamage, the amount of healthpoints to be subtracted.
            hitPlayer.HurtPlayer((int)dmgToDeal);
        }
        }

    }
    IEnumerator DeathAnimation()
    {
        animator.SetTrigger("enemyDead");

        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);

    }

}
