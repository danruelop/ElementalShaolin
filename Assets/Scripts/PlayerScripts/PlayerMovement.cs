using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public bool manaShieldActivated;

    public GameObject cdMeditateObject;
    private float cdMeditate;

    public GameObject cdEarthObject;
    private float cdEarth;

    public GameObject cdWaterObject;
    private float cdWater;

    public GameObject cdAirObject;
    private float cdAir;

    public GameObject cdFireObject;
    private float cdFire;
    /*
    private float cdManaShield;

    private float manaShieldDuration;
    */
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

        /*
        if (cdManaShield > 0)
        {
            cdManaShield -= 1 * Time.deltaTime;
        }
        if (manaShieldDuration > 0)
        {
            manaShieldDuration -= 1 * Time.deltaTime;
        }
        if (manaShieldActivated && manaShieldDuration <= 0)
        {
            manaShieldActivated = false;
            //aqui viene la animación del escudo de maná
            animator.SetBool("meditate", false);
        }

        */
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
        }
        // SHOW CD MEDITATE
        if (cdMeditate > 0)
        {
            cdMeditate -= 1 * Time.deltaTime;
            cdMeditateObject.GetComponentInChildren<Text>().text = ((int)cdMeditate).ToString();
            cdMeditateObject.GetComponent<Image>().enabled = true;
            cdMeditateObject.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            cdMeditateObject.GetComponent<Image>().enabled = false;
            cdMeditateObject.GetComponentInChildren<Text>().enabled = false;
        }
        // SHOW CD EARTH
        if (cdEarth > 0)
        {
            cdEarth -= 1 * Time.deltaTime;
            cdEarthObject.GetComponentInChildren<Text>().text = ((int) cdEarth).ToString();
            cdEarthObject.GetComponent<Image>().enabled = true;
            cdEarthObject.GetComponentInChildren<Text>().enabled = true;
        } else
        {
            cdEarthObject.GetComponent<Image>().enabled = false;
            cdEarthObject.GetComponentInChildren<Text>().enabled = false;
        }

        // SHOW CD WATER
        if (cdWater > 0)
        {
            cdWater -= 1 * Time.deltaTime;
            cdWaterObject.GetComponentInChildren<Text>().text = ((int)cdWater).ToString();
            cdWaterObject.GetComponent<Image>().enabled = true;
            cdWaterObject.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            cdWaterObject.GetComponent<Image>().enabled = false;
            cdWaterObject.GetComponentInChildren<Text>().enabled = false;
        }

      
        // SHOW CD AIR
        if (cdAir > 0)
        {
            cdAir -= 1 * Time.deltaTime;
            cdAirObject.GetComponentInChildren<Text>().text = ((int)cdAir).ToString();
            cdAirObject.GetComponent<Image>().enabled = true;
            cdAirObject.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            cdAirObject.GetComponent<Image>().enabled = false;
            cdAirObject.GetComponentInChildren<Text>().enabled = false;
        }

        //SHOW CD FIRE
        if (cdFire > 0)
        {
            cdFire -= 1 * Time.deltaTime;
            cdFireObject.GetComponentInChildren<Text>().text = ((int)cdFire).ToString();
            cdFireObject.GetComponent<Image>().enabled = true;
            cdFireObject.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            cdFireObject.GetComponent<Image>().enabled = false;
            cdFireObject.GetComponentInChildren<Text>().enabled = false;
        }

        




        if (currentState == PlayerState.interact)
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
            && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 1 && cdEarth <= 0.5f)
        {
            cdEarth = 2f;
            StartCoroutine(EarthSpellCo());
        } 
        else if(Input.GetButtonDown("waterSpell") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 2 && cdWater <= 0.5f)
        {
            cdWater = 2f;
            StartCoroutine(WaterSpellCo());
        }
        else if (Input.GetButtonDown("airSpell") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 3 && cdAir <= 0.5f)
        {
            cdAir = 2f;
            StartCoroutine(AirSpellCo());
        }
        else if (Input.GetButtonDown("fireSpell") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 4 && cdFire <= 0.5f)
        {
            cdFire = 2f;
            StartCoroutine(FireSpellCo());
        }
        else if(currentState == PlayerState.walk || currentState == PlayerState.idle) 
        {
            UpdateAnimationAndMove();

        }
        
        if(Input.GetButtonDown("meditate") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger && playerInventory.currentMana < 10 && cdMeditate <= 0.5f)
        {
            cdMeditate = 30f;
            StartCoroutine(MeditateCo());
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

           /* if (Input.GetButton("manaShield") && cdManaShield <= 0)
            {
                manaShieldActivated = true;
                //aqui viene la animación del escudo de maná
                animator.SetBool("meditate", true);
                cdManaShield = 15f;
                manaShieldDuration = 5f;
            } else
            {
                animator.SetBool("moving", true);
            }
           */
            animator.SetBool("moving", true);

        }
        else
        {

            /*if (!manaShieldActivated)
            {
                animator.SetBool("moving", false);
            }
            */
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.fixedDeltaTime);

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
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
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
