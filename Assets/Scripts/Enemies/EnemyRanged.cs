using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{



    GameObject player;

    //Enemy position and player position.
    // Vector3 initialPosition, target;
    Vector3 target;
    Rigidbody2D rb;

    public bool targetPlayer = true; //If true, shots will target player, otherwise shoot in random directions.
    public int atkDirections = 1; //How many directions to shoot if targetPlayer = false;
    public GameObject projectile; //Ammo prefab
    public float atkSpeed = 2f; //How Long to wait between shots.
    public float projectileSpeed = 2; //How fast the projectile moves;
    private bool attacking; //If true, shooting in progress


    void Start()
    {
        //Find player
        player = GameObject.FindGameObjectWithTag("Player");
        //Initialize rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit;
        //Prevent shooting too early
        // target = initialPosition;
        if (targetPlayer)
        {
            //When Player is spotted, draw a Ray
            Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
            Debug.DrawRay(transform.position, forward, Color.red); //This will show up in the editor

            //Raycast and check if player was hit
            hit = Physics2D.Raycast(
            transform.position, //enemy position
            player.transform.position - transform.position //Distance between enemy and player
            // visionRadius, //Havaintoalue
            // 1 << LayerMask.NameToLayer("Default")//Olaanko eri kerroksessa kuin default
            );
            if (hit.collider.tag == "Player")
            {//Did raycast hit player? 
                target = player.transform.position;
            }
        }
        else
        {
            //TODO: add code for shooting in random directions
        }

        //Distance between player and enemy
        float distance = Vector3.Distance(target, transform.position);

        Vector3 dir = (target - transform.position).normalized;

        //If not already attacking start attacking
        if (!attacking)
        {
            StartCoroutine(Attack(atkSpeed));
        }

    }
    IEnumerator Attack(float seconds)
    {
        attacking = true;

        //If target has been set and 
        //if(target != initialPosition && projectile != null){
        if (projectile != null)
        {
            //Create projectile which starts the EnemyProjectile.cs Script in projectile prefab
            Instantiate(projectile, transform.position, transform.rotation);

            //Wait before continuing
            yield return new WaitForSeconds(seconds);
        }
        attacking = false; //Attack completed and new attack may begin.
    }
}

