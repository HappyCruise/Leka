using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public GameObject shot;
    private Vector2 playerPos;

    private float timeBetweenShots;
    public float startTimeBetweenShots;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>().position;
        timeBetweenShots = startTimeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && timeBetweenShots <= 0)
        {
            GameObject go = (GameObject)Instantiate(shot, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
            go.GetComponent<PlayerProjectile>().xDir = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && timeBetweenShots <= 0)
        {
            GameObject go = (GameObject)Instantiate(shot, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
            go.GetComponent<PlayerProjectile>().xDir = -1;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && timeBetweenShots <= 0)
        {
            GameObject go = (GameObject)Instantiate(shot, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
            go.GetComponent<PlayerProjectile>().yDir = 1;
        }

        else if (Input.GetKey(KeyCode.DownArrow) && timeBetweenShots <= 0)
        {
            GameObject go = (GameObject)Instantiate(shot, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
            go.GetComponent<PlayerProjectile>().yDir = -1;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
        
    }
}
