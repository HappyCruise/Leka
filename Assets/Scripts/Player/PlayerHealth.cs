using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //Only allow one instance
    public static PlayerHealth instance;
    public Text healthText; //Health text
    public float currentHP; //Current health
    public float maxHP = 100; //Max health
    private bool firstLevel = true;
    private Animator animator;
    public AudioClip gameOverSound;				//Audio clip to play when player dies.

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        instance = this;

        //If on the first level, set to max HP;
        if (firstLevel)
        {
            firstLevel = false;
            SetMaxHealth();
            UpdateHealthText();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Enemies call this function to deal damage.
    public void HurtPlayer(int damageToTake)
    {
        animator.SetTrigger("playerHit");

        currentHP -= damageToTake;


        if (!CheckIfGameOver()) //Check if player died. 
            UpdateHealthText(); //Update healthText if didnt.

        //TODO: Add animation and sound for getting hit;
    }

    //Add health to player
    public void AddPlayerHealth(int hpToGive)
    {

        currentHP += hpToGive;

        //If health is more than the maximum value, set it to the maximum value.
        if (currentHP > maxHP)
        {
            SetMaxHealth();
        }
        UpdateHealthText();
    }

    //Increase max hp
    public void AddPlayerMaxHealth(int hpToAdd)
    {
        maxHP += hpToAdd;
        AddPlayerHealth(10);
    }
    //Set player to full HP
    public void SetMaxHealth()
    {
        currentHP = maxHP;
        UpdateHealthText();
    }

    //Check if player is dead.
    public bool CheckIfGameOver()
    {
        if (currentHP <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            GameManager.instance.GameOver();
            return true;
        }
        return false;
    }

    private void UpdateHealthText()
    {
        healthText.text = "Health: " + currentHP;
    }
}
