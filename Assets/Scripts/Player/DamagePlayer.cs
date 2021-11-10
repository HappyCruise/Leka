using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is given to a GameObject that has the ability to hurt a player
public class DamagePlayer : MonoBehaviour
{
    public int damageToTake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().DamagePlayer(damageToTake);
        }
    }
}
