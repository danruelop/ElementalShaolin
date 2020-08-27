using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretEnemy : EnemyEarth
{
    public Sprite[] forms;
    public string[] types;
    public GameObject projectile;
    public float fireDelay;
    private float fireDelaySeconds;
    private int lastI;
    public bool canFire = true;

    private float cdChangeForm;

    private void Update()
    {
        if(cdChangeForm > 0)
        {
            cdChangeForm -= 1 * Time.deltaTime;
        }
        
        if(cdChangeForm <= 0)
        {

            int i = UnityEngine.Random.Range(0, 4);

            if (i == lastI)
            {
                Sprite[] forms_copy = forms.Where(val => val != forms[lastI]).ToArray();
                string[] types_copy = types.Where(val => val != types[lastI]).ToArray();
                i = UnityEngine.Random.Range(0, 3);
                this.gameObject.GetComponent<SpriteRenderer>().sprite = forms_copy[i];
                this.gameObject.tag = types_copy[i];
                i = Array.IndexOf(forms, forms_copy[i]);
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = forms[i];
                this.gameObject.tag = types[i];
            }
            
            cdChangeForm = 1;
            lastI = i;

        }

        fireDelaySeconds -= Time.deltaTime;
        if (fireDelaySeconds <= 0)
        {
            canFire = true;
            fireDelaySeconds = fireDelay;
        }

    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position)
            <= chaseRadius && Vector3.Distance(target.position,
            transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger)
            {
                if (canFire)
                {
                    Vector3 tempVector = target.transform.position - transform.position;
                    GameObject current = Instantiate(projectile, 
                        transform.position, Quaternion.identity);
                    current.GetComponent<Projectile>().Launch(tempVector);
                    canFire = false;
                    ChangeState(EnemyState.walk);
                    anim.SetBool("wakeUp", true);
                }
            }
        }
        else if (Vector3.Distance(target.position,
            transform.position) > chaseRadius)
        {
            anim.SetBool("wakeUp", false);
        }
    }
}
