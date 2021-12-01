using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    public int hp;
    private int initHp = 3;
    public Sprite damageSprite;
    private SpriteRenderer spriteRenderer;

    

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = initHp;
    }

    

    // Update is called once per frame
    void Update()
    {
    }

    //Damage wall by given damage amount
    public void DamageWall(int damageToTake)
    {
        if(hp == initHp){
            hp -= damageToTake;
            spriteRenderer.sprite = damageSprite;
        }else{
            hp -= damageToTake;
        }
        CheckIfDestroyed();
    }

    //Check if wall should be destroyed
    void CheckIfDestroyed()
    {
        if (hp <= 0)
        {
            Destroy (gameObject);
        }
    }
}
