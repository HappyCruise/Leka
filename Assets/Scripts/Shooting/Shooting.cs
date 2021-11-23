using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject shot; //Projectile
    private Vector3 playerPos; //Player position
    

    // Start is called before the first frame update
    void Start()
    {
        //Get player position
        playerPos = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && PlayerController.instance.canShoot){
            Instantiate(shot, playerPos, Quaternion.identity);
        }

       /*  if(PlayerController.instance.canShoot){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                //Shoot upward
                Instantiate(shot, playerPos, Quaternion.identity);
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow)){//Shoot downward
            }
                
            else if(Input.GetKeyDown(KeyCode.LeftArrow)){//Shoot left
            }
                
            else if(Input.GetKeyDown(KeyCode.RightArrow)){//Shoot right
            }
                

        } */
        
    }
}
