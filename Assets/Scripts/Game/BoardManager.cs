using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    //Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        //Min Max values for Count class
        public int minimum;

        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    //Board size
    public int columns = 10;

    public int rows = 12;

    public Count wallCount = new Count(5, 20); //How many walls to place on the board
    public Count foodCount = new Count(0, 3); //How many foodtiles to place on the board

    public GameObject exit; //Prefab for the door to next level
    public GameObject boss; //Prefab for boss
    public GameObject kelaGod;
    private int kelaLevel = 0;
    private bool spawnKela;

    public GameObject[] floorTiles; //Floor prefabs

    public GameObject[] wallTiles; //Wall prefabs
    public GameObject[] foodTiles;  //Array of food prefabs.

    public GameObject[] enemyTiles; //Enemy prefabs

    public GameObject[] outerWallTiles; //Outer wall prefabs

    private Transform boardHolder; // Reference to board objects transform

    //Possible locations to place tiles
    private List<Vector3> gridPositions = new List<Vector3>();


    //Clear gridPositions and prepare for new board
    //Clear gridPositions and prepare for new board
    void InitialiseList()
    {
        gridPositions.Clear();

        //Loop trough columns (x axis)
        for (int x = 0; x < columns; x++)
        {
            //Loop trough rows (y axis)
            for (int y = 0; y < rows; y++)
            {
                //For each position add a new Vector3 to gridPositions with coordinates
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
        // Dont spawn objects within the 1st 2 tiles to give room for player
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                gridPositions.Remove(new Vector3(x, y, 0f));
            }
        }
    }

    //Setup outer walls and floor
    void BoardSetup()
    {



        //CLEAR ALL ITEMS AND WALLS
        List<GameObject> objectsToDestroy = new List<GameObject>(GameObject.FindGameObjectsWithTag("DestroyableWall"));
        if (objectsToDestroy.Count == 0 && (kelaLevel % 5) == 1 && kelaLevel != 1)
        {
            spawnKela = true;
        }
        objectsToDestroy.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Food")));

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        //Instantiate Board and set boardHolder to its transform
        boardHolder = new GameObject("Board").transform;

        //Loop the board and add outerwall and floor tiles, starting from -1 to fill corner
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose random floor tile to place
                GameObject toInstantiate =
                    floorTiles[Random.Range(0, floorTiles.Length)];

                //If on the edge of board, place outerwall instead
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate =
                        outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                //Instantiate GameObject instance using chosen prefab at the Vector3 corresponding to current grid position in loop, then cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate,
                    new Vector3(x, y, 0f),
                    Quaternion.identity) as
                    GameObject;

                //Set the parent of new object to boardHolder to avoid cluttering hierarchy
                instance.transform.SetParent(boardHolder);
            }
        }
        if (spawnKela)
        {
            GameObject instance =
                    Instantiate(kelaGod,
                    new Vector3(7, 7, 0f),
                    Quaternion.identity) as
                    GameObject;
            spawnKela = false;
        }
    }

    //Returns random position from gridPositions
    Vector3 RandomPosition()
    {
        //Random value between 0 and count of items in gridPositions
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove used position from list
        gridPositions.RemoveAt(randomIndex);

        //Return random position
        return randomPosition;
    }

    //Layout array of gameObjects at random places with min-max range for amount of items.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose random number of objects within min and max limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until randomly chosen limit is reached
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            //Choose random tile from array and assign it to chosen tile
            GameObject tileChoice =
                tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the random position
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes level and calls previous functions to layout tiles
    public void SetupScene(int level)
    {
        kelaLevel = level;
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset gridPosition list
        InitialiseList();


        //Are we on a boss stage
        if (level % 5 == 0)
        {
            int x = columns / 2;
            int y = rows / 2;

            //Spawn a boss enemy in the middle of the board.
            Instantiate(boss,
            new Vector3(x, y, 0f),
            Quaternion.identity);

            gridPositions.Remove(new Vector3(x, y, 0f));
        }
        //Instantiate a random number of wall tiles
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Determine number of eneies based on current level
        int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);


        //Instantiate the exit tile (always in the same position);
        Instantiate(exit,
        new Vector3(columns - 1, rows - 1, 0f),
        Quaternion.identity);
    }
}