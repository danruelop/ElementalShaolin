using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle,
    meditate
}

public class PlayerMovement : MonoBehaviour{

    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;
    public Signal playerHit;
    public Signal reduceMana;
    public GameObject earthProjectile;
    public GameObject waterProjectile;
    public GameObject airProjectile;
    public GameObject fireProjectile;
    public Slider manaSlider;
    private float meditateCooldown = 5f;
    private float nextMeditate;

    // Start is called before the first frame update
    void Start(){
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }


    // Update is called once per frame
    void Update(){

        if(currentState == PlayerState.interact)
        {
            return;
        }
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());

        } else if(Input.GetButtonDown("earthSpell") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 1)
        {
            StartCoroutine(EarthSpellCo());
        } 
        else if(Input.GetButtonDown("waterSpell") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 2)
        {
            StartCoroutine(WaterSpellCo());
        }
        else if (Input.GetButtonDown("airSpell") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 3)
        {
            StartCoroutine(AirSpellCo());
        }
        else if (Input.GetButtonDown("fireSpell") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 4)
        {
            StartCoroutine(FireSpellCo());
        }
        else if(currentState == PlayerState.walk || currentState == PlayerState.idle) 
        {
            UpdateAnimationAndMove();

        }
        
        if(Input.GetButtonDown("meditate") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger && playerInventory.currentMana < 10 && nextMeditate < Time.time)
        {
            StartCoroutine(MeditateCo());
            nextMeditate = Time.time + meditateCooldown;
        }

        

    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if(currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
        
    }
    private IEnumerator EarthSpellCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeEarthSpell();
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }

    private IEnumerator WaterSpellCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeWaterSpell();
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }

    private IEnumerator AirSpellCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeAirSpell();
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }

    private IEnumerator FireSpellCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeFireSpell();
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }

    private IEnumerator MeditateCo()
    {
        animator.SetBool("meditating", true);
        currentState = PlayerState.meditate;
        while(playerInventory.currentMana < 10)
        {
            AddMana();
            yield return new WaitForSeconds(.6f);
        }
        yield return null;
        
        animator.SetBool("meditating", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }



    private void MakeEarthSpell()
    {
        if(playerInventory.currentMana > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            EarthSpell earthSpell = Instantiate(earthProjectile, transform.position, Quaternion.identity).GetComponent<EarthSpell>();
            earthSpell.Setup(temp, ChooseArrowDirection());
            reduceMana.Raise();
        }
    }

    private void MakeWaterSpell()
    {
        if (playerInventory.currentMana > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            WaterSpell waterSpell = Instantiate(waterProjectile, transform.position, Quaternion.identity).GetComponent<WaterSpell>();
            waterSpell.Setup(temp, ChooseArrowDirection());
            reduceMana.Raise();
        }
    }

    private void MakeAirSpell()
    {
        if (playerInventory.currentMana > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            AirSpell airSpell = Instantiate(airProjectile, transform.position, Quaternion.identity).GetComponent<AirSpell>();
            airSpell.Setup(temp, ChooseArrowDirection());
            reduceMana.Raise();
        }
    }

    private void MakeFireSpell()
    {
        if (playerInventory.currentMana > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            FireSpell fireSpell = Instantiate(fireProjectile, transform.position, Quaternion.identity).GetComponent<FireSpell>();
            fireSpell.Setup(temp, ChooseArrowDirection());
            reduceMana.Raise();
        }
    }

    public void AddMana()
    {
        manaSlider.value += 1;
        playerInventory.currentMana += 1;
        if (manaSlider.value > manaSlider.maxValue)
        {
            manaSlider.value = manaSlider.maxValue;
            playerInventory.currentMana = playerInventory.maxMana;
        }
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                animator.SetBool("receiveItem", true);
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receiveItem", false);
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.deltaTime);

    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        
    }

    private IEnumerator KnockCo(float knockTime)
    {
        playerHit.Raise();
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    
}
