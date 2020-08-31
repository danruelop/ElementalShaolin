using System;
using UnityEngine;


public class Spell : MonoBehaviour
{
    public float speed;
    public Rigidbody2D myRigidbody;

    

    public void Setup(Vector2 velocity, Vector3 direction)
    {
        myRigidbody.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        String tag = other.gameObject.tag;
        if (isAnEnemy(tag) || other.gameObject.CompareTag("spellCollision"))
        {
            Destroy(this.gameObject);
        }
        
    }

    public bool isAnEnemy(String tag)
    {
        
        bool res = false;
        if (tag == "enemyEarth" ||
           tag == "enemyWater" ||
           tag == "enemyAir" ||
           tag == "enemyFire" ||
           tag == "enemy")
        {
            res = true;
        }

        return res;
    }

}
