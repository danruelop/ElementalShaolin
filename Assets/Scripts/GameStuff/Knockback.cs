using System;
using UnityEngine;

public class Knockback : MonoBehaviour
{

    public float thrust;
    public float knockTime;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        String tag = other.gameObject.tag;

        if (other.gameObject.CompareTag("breakable") 
            && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Pot>().Smash();
        }
        
        if (isAnEnemy(tag) || (other.gameObject.CompareTag("Player") && !other.gameObject.GetComponent<PlayerMovement>().manaShieldActivated))              // cuidado aquí
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);

                if (isAnEnemy(tag) && other.isTrigger)
                {
                    float dmg = DamageCalculator(other.gameObject, damage);
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime, dmg);
                }
                if (other.gameObject.CompareTag("Player"))
                {
                    if (other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {                                             
                        hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;                                           
                        other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                    
                }
            }
        }
    }

    private float DamageCalculator(GameObject enemy, float dmg)
    {
        float calculatedDmg = dmg;

        //earth damage
        if (this.gameObject.name == "EarthSpell(Clone)" && enemy.CompareTag("enemyAir") || 
            this.gameObject.name == "WaterSpell(Clone)" && enemy.CompareTag("enemyFire") || 
            this.gameObject.name == "AirSpell(Clone)" && enemy.CompareTag("enemyWater") ||
            this.gameObject.name == "FireSpell(Clone)" && enemy.CompareTag("enemyEarth"))
        {
            calculatedDmg = 2 * dmg;
        }
        if (this.gameObject.name == "EarthSpell(Clone)" && enemy.CompareTag("enemyFire") || 
            this.gameObject.name == "WaterSpell(Clone)" && enemy.CompareTag("enemyAir") ||
            this.gameObject.name == "AirSpell(Clone)" && enemy.CompareTag("enemyEarth") ||
            this.gameObject.name == "FireSpell(Clone)" && enemy.CompareTag("enemyWater"))
        {
            calculatedDmg = dmg / 2;
        }

        return calculatedDmg;

    }
    
    public bool isAnEnemy(String tag)
    {
        bool res = false;
        if(tag == "enemyEarth" || 
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
