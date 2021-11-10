using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Only allow one instance
    public static PlayerHealth instance;
    //Health
    public float currentHP;
    public float maxHP = 100;
    private bool firstLevel = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //If on the first level, set to max HP;
        if (firstLevel)
        {
            firstLevel = false;
            SetMaxHealth();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            //TODO: add death mechanics;
        }
    }

    //Something damages player
    public void DamagePlayer(int damageToTake)
    {
        currentHP -= damageToTake;

        //TODO: Add animation and sound for getting hit;
    }

    //Add health to player
    public void AddPlayerHealth(int hpToGive)
    {
        currentHP += hpToGive;
        if (currentHP > maxHP)
        {
            SetMaxHealth();
        }
    }

    //Set player to full HP
    public void SetMaxHealth()
    {
        currentHP = maxHP;
    }
}
