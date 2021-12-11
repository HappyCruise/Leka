using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    //Time to wait before starting level
    public float levelStartDelay = 1f;

    //Use static instance to let other scripts access GameManager
    public static GameManager instance = null;

    //Text to display current level number
    private Text levelText;
    private GameObject healthText; //Current health text object. ( Used to hide it during level loading )

    //Image that covers the level while its loading
    private GameObject levelImage;

    //Store a reference to BoardManager which will setup the level layout
    private BoardManager boardScript;

    private bool firstLevel = true;
    private int level;

    void Awake()
    {
        //Find the image 
        levelImage = GameObject.Find("LevelImage");
        //Find the text
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        healthText = GameObject.Find("HealthText"); //Find the food text
        //Check if instance exists
        if (instance == null)
        {
            //If not, set to this
            instance = this;
        }
        //If it does, and it is not this destroy it
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Dont destroy this when reloading scene
        DontDestroyOnLoad(gameObject);

        //Reference to BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Initialize first level
        if (firstLevel)
        {
            InitGame(1);
            firstLevel = false;

        }

    }



    //Initialize the level
    public void InitGame(int lvl)
    {


        healthText.SetActive(false); //Hide the health text while starting level
        //Display image
        levelImage.SetActive(true);

        //Set the text
        levelText.text = "Level " + lvl;


        //Hide image after delay
        Invoke("HideLevelImage", levelStartDelay);
        Debug.Log("SETTING UP LEVEL " + lvl);
        boardScript.SetupScene(lvl);
        level = lvl;

    }

    void HideLevelImage()
    {
        //Hide image
        levelImage.SetActive(false);

        healthText.SetActive(true); //Show healthText again.
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Called when the player dies
    public void GameOver()
    {
        //Set the levelText to display game over message
        levelText.text = "After " + level + " crueling levels, our hero falls to the ground.";

        //Enable the image
        levelImage.SetActive(true);

        //Disable this manager 
        enabled = false;
    }
}