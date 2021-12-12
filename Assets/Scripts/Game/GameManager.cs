using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //Time to wait before starting level
    public int levelStartDelay = 1;

    //Use static instance to let other scripts access GameManager
    public static GameManager instance = null;

    private GameObject healthText; //Current health text object. ( Used to hide it during level loading )

    //Text to display current level number
    private Text levelText;

    //Image that covers the level while its loading
    private GameObject levelImage;

    private GameObject deathImage; //Image displayed on death
    private Text deathText; //Death text

    private List<Enemy> enemies;

    //Store a reference to BoardManager which will setup the level layout
    private BoardManager boardScript;

    private bool firstLevel = true;
    private int level;

    void Awake()
    {
        instance = this;
        //Find the image 
        levelImage = GameObject.Find("LevelImage");
        deathImage = GameObject.Find("DeathImage");
        //Find the text
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText");
        deathText = GameObject.Find("DeathText").GetComponent<Text>();

        deathImage.SetActive(false); //Hide the death image untill player is killed.


        //Dont destroy this when reloading scene
        // DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        //Reference to BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Initialize first level
        // if (firstLevel)
        // {
        //     InitGame(1);
        //     firstLevel = false;

        // }
        InitGame(1);
    }



    //Initialize the level
    public void InitGame(int lvl)
    {
        healthText.SetActive(false); //Hide the health text while starting level
        //Display image
        levelImage.SetActive(true);

        //Set the text
        levelText.text = "Level " + lvl;


        //Set the text
        levelText.text = "Level " + lvl;

        //Hide image after delay
        StartCoroutine(HideLevelImage());
        Debug.Log("SETTING UP LEVEL " + lvl);
        boardScript.SetupScene(lvl);
        Debug.Log("LEVEL SETUP COMPLETED");
        level = lvl;




    }

    IEnumerator HideLevelImage()
    {
        Debug.Log("Waiting to hide image.");
        yield return new WaitForSeconds(1f);

        //Hide image
        levelImage.SetActive(false);
        Debug.Log("Hiding Image");

        healthText.SetActive(true); //Show healthText again.
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies.
        enemies.Add(script);
    }
    //Called when the player dies
    public void GameOver()
    {
        // //Set the levelText to display game over message
        deathText.text = "YOU DIED AFTER " + level + " LEVELS";

        // //Enable the image
        deathImage.SetActive(true);

        //Disable this manager 
        // enabled = false;
    }

    public void OpenMainMenu()
    {
        //Open main menu
        SceneManager.LoadScene("MainMenu");
        deathImage.SetActive(false);
    }
}
