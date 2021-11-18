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

    //Image that covers the level while its loading
    private GameObject levelImage;

    //Store a reference to BoardManager which will setup the level layout
    private BoardManager boardScript;

    //Current level
    private int level = 1;

    //Check if level is being loaded, prevent player from moving.
    private bool doingSetup = true;

    void Awake()
    {
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
        InitGame();
    }

    //This is called everytime a new level is loaded
    void OnLevelWasLoaded(int index)
    {
        level++;
        //Initialize board
        InitGame();
    }

    //Initialize the level
    void InitGame()
    {
        //prevent player from moving
        doingSetup = true;
        //Find the image and text
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        //Set the text
        levelText.text = "Level " + level;
        //Display image
        levelImage.SetActive(true);

        //Hide image after delay
        Invoke("HideLevelImage", levelStartDelay);

        boardScript.SetupScene(level);
    }

    void HideLevelImage()
    {
        //Hide image
        levelImage.SetActive(false);
        //Allow player to move
        doingSetup = false;
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
