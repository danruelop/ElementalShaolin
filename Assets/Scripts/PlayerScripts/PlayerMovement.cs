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
    meditate,
    manaShield
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
    public GameObject finalMenu;


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

    public GameObject cdManaShieldObject;
    private float cdManaShield;

    [Header("Sound Variables")]
    public AudioClip hitSound;
    public AudioClip takeInventorySound;
    public AudioClip fightNoMagicSound;
    public AudioClip spellAirSound;
    public AudioClip spellEarthSound;
    public AudioClip spellFireSound;
    public AudioClip spellWaterSound;
    public AudioClip meditateSound;
    public AudioClip magicShieldSound;
    public AudioClip gameOverSound;
    private AudioSource audioPlayer;
   






    // Start is called before the first frame update
    void Start(){
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
        audioPlayer = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update(){


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

        // SHOW CD MANA SHIELD
        if (cdManaShield > 0)
        {
            cdManaShield -= 1 * Time.deltaTime;
            cdManaShieldObject.GetComponentInChildren<Text>().text = ((int)cdManaShield).ToString();
            cdManaShieldObject.GetComponent<Image>().enabled = true;
            cdManaShieldObject.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            cdManaShieldObject.GetComponent<Image>().enabled = false;
            cdManaShieldObject.GetComponentInChildren<Text>().enabled = false;
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

        if(currentState != PlayerState.meditate && currentState != PlayerState.manaShield)
        {
            if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
            {
                StartCoroutine(AttackCo());

            }
            else if (Input.GetButtonDown("earthSpell") && currentState != PlayerState.attack
              && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 1 && cdEarth <= 0.5f && playerInventory.currentMana > 0)
            {
                cdEarth = 2f;
                StartCoroutine(EarthSpellCo());
            }
            else if (Input.GetButtonDown("waterSpell") && currentState != PlayerState.attack
                && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 2 && cdWater <= 0.5f && playerInventory.currentMana > 0)
            {
                cdWater = 2f;
                StartCoroutine(WaterSpellCo());
            }
            else if (Input.GetButtonDown("airSpell") && currentState != PlayerState.attack
               && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 3 && cdAir <= 0.5f && playerInventory.currentMana > 0)
            {
                cdAir = 2f;
                StartCoroutine(AirSpellCo());
            }
            else if (Input.GetButtonDown("fireSpell") && currentState != PlayerState.attack
               && currentState != PlayerState.stagger && playerInventory.numberOfElements >= 4 && cdFire <= 0.5f && playerInventory.currentMana > 0)
            {
                cdFire = 2f;
                StartCoroutine(FireSpellCo());
            }
            else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
            {
                UpdateAnimationAndMove();

            }
        }
        
        
        if(Input.GetButtonDown("meditate") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger && playerInventory.currentMana < 10 && cdMeditate <= 0.5f && currentState != PlayerState.manaShield)
        {
            cdMeditate = 30f;
            StartCoroutine(MeditateCo());

        } else if (Input.GetButtonDown("manaShield") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger  && cdManaShield <= 0.5f && currentState != PlayerState.meditate && playerInventory.manaShieldObtained == true)
        {
            cdManaShield = 10f;
            StartCoroutine(ManaShieldCo());
        }

        



    }

    

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        audioPlayer.clip = fightNoMagicSound;
        audioPlayer.Play();
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
        audioPlayer.clip = spellEarthSound;
        audioPlayer.Play();
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
        audioPlayer.clip = spellWaterSound;
        audioPlayer.Play();
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
        audioPlayer.clip = spellAirSound;
        audioPlayer.Play();
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
        audioPlayer.clip = spellFireSound;
        audioPlayer.Play();
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
            audioPlayer.clip = meditateSound;
            audioPlayer.Play();
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

    private IEnumerator ManaShieldCo()
    {

        animator.SetBool("manashield", true);
        currentState = PlayerState.manaShield;
        manaShieldActivated = true;
        audioPlayer.clip = magicShieldSound;
        audioPlayer.Play();
        yield return new WaitForSeconds(2f);
        animator.SetBool("manashield", false);
        yield return new WaitForSeconds(0.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
            
        }
        yield return new WaitForSeconds(0.5f);
        manaShieldActivated = false;

    }

    private void MakeEarthSpell()
    {
         Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
         Spell earthSpell = Instantiate(earthProjectile, transform.position, Quaternion.identity).GetComponent<Spell>();
         earthSpell.Setup(temp, ChooseArrowDirection());
         reduceMana.Raise();
    }

    private void MakeWaterSpell()
    {
         Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
         Spell waterSpell = Instantiate(waterProjectile, transform.position, Quaternion.identity).GetComponent<Spell>();
         waterSpell.Setup(temp, ChooseArrowDirection());
         reduceMana.Raise();
        
    }

    private void MakeAirSpell()
    {   
         Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
         Spell airSpell = Instantiate(airProjectile, transform.position, Quaternion.identity).GetComponent<Spell>();
         airSpell.Setup(temp, ChooseArrowDirection());
         reduceMana.Raise();    
    }

    private void MakeFireSpell()
    {  
         Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
         Spell fireSpell = Instantiate(fireProjectile, transform.position, Quaternion.identity).GetComponent<Spell>();
         fireSpell.Setup(temp, ChooseArrowDirection());
         reduceMana.Raise();  
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
                audioPlayer.clip = takeInventorySound;
                audioPlayer.Play();
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
            transform.position + change * speed * Time.fixedDeltaTime);

    }

    public void Knock(float knockTime, float damage)
    {
        
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        audioPlayer.clip = hitSound;
        audioPlayer.Play();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            audioPlayer.clip = gameOverSound;
            audioPlayer.Play();          
            finalMenu.SetActive(true);
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
