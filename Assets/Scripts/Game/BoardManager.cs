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

    public Count wallCount = new Count(5, 9); //How many walls to place on the board

    public GameObject exit; //Prefab for the door to next level
    public GameObject[] floorTiles; //Floor prefabs
    public GameObject[] wallTiles; //Wall prefabs
    public GameObject[] enemyTiles; //Enemy prefabs
    public GameObject[] outerWallTiles; //Outer wall prefabs

    private Transform boardHolder; // Reference to board objects transform

    //Possible locations to place tiles
    private List<Vector3> gridPositions = new List<Vector3>();

    //Clear gridPositions and prepare for new board
    void InitialiseList()
    {
        gridPositions.Clear();

        //Loop trough columns (x axis)
        for (int x = 1; x < columns; x++)
        {
            //Loop trough rows (y axis)
            for (int y = 1; y < rows; y++)
            {
                //For each position add a new Vector3 to gridPositions with coordinates
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Setup outer walls and floor
    void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform
        boardHolder = new GameObject("Board").transform;

        //Loop the board and add outerwall and floor tiles, starting from -1 to fill corner
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose random tile to place
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                //If on the edge of board, place outerwall instead
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                //Instantiate GameObject instance using chosen prefab at the Vector3 corresponding to current grid position in loop, then cast it to GameObject.
                GameObject instance = toInstantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identify) as GameObject;

                //Set the parent of new object to boardHolder to avoid cluttering hierarchy
                instance.transfrom.SetParent(boardHolder);
            }
        }
    }

    //Returns random position from gridPositions
    Vector3 RandomPosition()
    {

    }
}
