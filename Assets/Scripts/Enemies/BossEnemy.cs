using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEnemy : Enemy
{

    private float maxHealth;
    private float currentHealth;

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
        maxHealth = PlayerController.instance.level * 0.7f; // Set the health to current level * 0.7 | 7, 14, 21 etc... 
        currentHealth = maxHealth;
        Debug.Log("BOSS HP " + maxHealth);
    }



    public void HurtBoss(int damageToGive)
    {
        currentHealth -= damageToGive;
        CheckIfDied();
    }

    private void CheckIfDied()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("enemyDead"); //TODO onks tää oikei :D
            Destroy(gameObject);
        }
    }

    void OnCantMove<T>(T component)
    where T : Component
    {
        double lvlAsDouble = (double)PlayerController.instance.level;//Get current lvl and cast it as double
        double dmgToDeal = Math.Floor(lvlAsDouble * 1.5f); // Scale damage according to level
        //Declare hitPlayer and set it to equal the encountered component.
        PlayerHealth hitPlayer = component as PlayerHealth;

        Debug.Log("Boss Cant move called");
        if (timeBetweenAttacks <= 0)
        {
            // StartCoroutine(HitPlayer(hitPlayer));
            hitPlayer.HurtPlayer((int)dmgToDeal);
            timeBetweenAttacks = startTimeBetweenAttacks;
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }

        //Set the attack trigger of animator to trigger Enemy attack animation.
        animator.SetTrigger("enemyAttack");

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
            collider.GetComponent<DestroyableWall>().DamageWall(3);
        }
    }
    IEnumerator HitPlayer(PlayerHealth hitPlayer)
    {
        double lvlAsDouble = (double)PlayerController.instance.level;//Get current lvl and cast it as double
        double dmgToDeal = Math.Floor(lvlAsDouble * 1.5f); // Scale damage according to level

        yield return new WaitForSeconds(0.7f);

        //Call the HurtPlayer function of hitPlayer passing it playerDamage, the amount of healthpoints to be subtracted.
        hitPlayer.HurtPlayer((int)dmgToDeal);
    }

}
