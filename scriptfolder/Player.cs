using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight = 3.5f;
    public Transform groundCheck;
    public bool isGrounded;
    Animator anim;
    public int curHp;
    int maxHp = 5;
    bool isHit = false;
    private Main main;
    public bool key = false;
    bool canTP = true;
    public bool inWater = false;
    bool isClimbing = false;
    int coins = 0;
    bool canHit = true;
    public GameObject blueGem, greenGem, redGem;
    int gemCount = 0;
    public bool onCarpet = false;
    float hitTimer = 0f;
    private Image PlayerCountDown;
    float insideTimer = -1f;
    public float insideTimerUp;
    private Image insideCountDown;
    private Inventory inventory;
    private SoundEffector soundeffector;
    //public Joystick joystick;
 
    void Start()
    {
        rb = GetComponent <Rigidbody2D>();
        anim = GetComponent<Animator>();
        main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Main>();
        inventory = main.gameObject.GetComponent<Inventory>();
        soundeffector = main.gameObject.GetComponent<SoundEffector>();
        insideCountDown = GameObject.FindGameObjectWithTag("InsideCountDown").GetComponent<Image>();
        //PlayerCountDown = GameObject.FindGameObjectWithTag("IconCountDown").GetComponent<Image>();
        //-------------------------------------------------------------------------------------------------------------------
        if(PlayerPrefs.GetInt("Player") == 0)
            PlayerCountDown = GameObject.FindGameObjectWithTag("BeigeIcon").GetComponent<Image>();

        if (PlayerPrefs.GetInt("Player") == 1)
            PlayerCountDown = GameObject.FindGameObjectWithTag("BlueIcon").GetComponent<Image>();

        if (PlayerPrefs.GetInt("Player") == 2)
            PlayerCountDown = GameObject.FindGameObjectWithTag("GreenIcon").GetComponent<Image>();

        if (PlayerPrefs.GetInt("Player") == 3)
            PlayerCountDown = GameObject.FindGameObjectWithTag("PinkIcon").GetComponent<Image>();

        if (PlayerPrefs.GetInt("Player") == 4)
            PlayerCountDown = GameObject.FindGameObjectWithTag("YellowIcon").GetComponent<Image>();

        //-------------------------------------------------------------------------------------------------------------------
        curHp = maxHp;
        print("Current hp = " + curHp);
    }

    // Update is called once per frame
    void Update()
    {   
        if(inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (Input.GetAxis("Horizontal") != 0)
            //if (joystick.Horizontal >= 0.3f || joystick.Horizontal <= -0.3f)
                Flip();

        }
        else
        {
            if (!isGrounded && !isClimbing)
                anim.SetInteger("State", 3);

            if (Input.GetAxis("Horizontal") == 0 && (isGrounded) && !isClimbing)
            //if(joystick.Horizontal < 0.3f && joystick.Horizontal > -0.3f && (isGrounded) && !isClimbing)
            {
                //canHit = true;
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing && !onCarpet)
                    anim.SetInteger("State", 2);
            }
        }
        //Прыжок для компа
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.JoystickButton2) && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundeffector.PlayJumpSound();
        }
        //if (Input.GetAxis("Vertical") >= 0 && isGrounded && Input.GetAxis("Horizontal") == 0 && !isClimbing)
        if (Input.GetAxisRaw("Vertical") < 0 && isGrounded && Input.GetAxis("Horizontal") == 0 && !isClimbing && !inWater)
            anim.SetInteger("State", 7);
        if(insideTimer >= 0f)
        {
            insideTimer += Time.deltaTime;
            if (insideTimer >= insideTimerUp)
            {
                insideTimer = 0f;
                RecountHp(-1);
            }
            else
                insideCountDown.fillAmount = 1 - (insideTimer / insideTimerUp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && curHp != maxHp|| Input.GetKeyDown(KeyCode.Joystick1Button3) && curHp != maxHp)
        {
            inventory.Use_hp();
            soundeffector.HotBarSound();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            inventory.Use_bg();
            soundeffector.HotBarSound();
        }
        /*if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            inventory.Use_gg();
            soundeffector.HotBarSound();
        } */
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Joystick1Button9))
        {
            inventory.Use_rg();
            soundeffector.HotBarSound();
        }
    }
    /*public void Jump() //для жостика
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundeffector.PlayJumpSound();
        }
    } */
    void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        //if (joystick.Horizontal >= 0.3f)
            //rb.velocity = new Vector2(speed, rb.velocity.y);
        //else if (joystick.Horizontal <= -0.3f)
            //rb.velocity = new Vector2(-speed, rb.velocity.y);
        //else
            //rb.velocity = new Vector2(0f, rb.velocity.y);
        //if (Input.GetKeyDown(KeyCode.Space))
            //rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse); 
    }
    void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        //if(joystick.Horizontal >= 0.3f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
        //if(joystick.Horizontal <= -0.3f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
    public void RecountHp(int deltaHp)
    {
        //curHp = curHp + deltaHp; //перемещает в курсе отсюда вниз
        if (deltaHp < 0 && canHit)
        {
            curHp = curHp + deltaHp;
            //canHit = false; //- я так думал
            StopCoroutine(OnHit());
            canHit = false; //- по курсу
            isHit = true;
            StartCoroutine(OnHit());
        }
        // Чтобы сердечко съедалось но хп сверхмаксимального не прибавлялось.
        else if (deltaHp > 0)

            if (curHp >= maxHp)
                curHp = maxHp;
            else curHp = curHp + deltaHp;
        
        if(curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }
    IEnumerator OnHit()
    {
        if(isHit) 
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        else
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        
        if (GetComponent<SpriteRenderer>().color.g >= 1f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            canHit = true;
            //StopCoroutine(OnHit());
            yield break;
        }
        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
            GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
        }
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit());
    } 

    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //----------------------------------------
        if (collision.gameObject.tag == "Star")
        {
            Destroy(collision.gameObject);
            PlayerPrefs.SetInt("AllStars", PlayerPrefs.GetInt("AllStars") + 1);
            print(PlayerPrefs.GetInt("AllStars"));
            soundeffector.PlayItemsSound();
        }




        //----------------------------------------

        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();
            soundeffector.PlayItemsSound();
        }
        if(collision.gameObject.tag == "Door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTP)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTP = false;
                StartCoroutine(TPwait());
            }
                
            else if (key)
                collision.gameObject.GetComponent<Door>().Unlock();
        }
        if (collision.gameObject.tag == "Coin")
        {
            Destroy(collision.gameObject);
            coins++;
            soundeffector.PlayCoinSound();
            //print("You get a coin! Coins count: " + coins);
        }

        if (collision.gameObject.tag == "CoinSilver")
        {
            Destroy(collision.gameObject);
            coins +=3 ;
            soundeffector.PlayCoinSound();
            //print("You get a coin! Coins count: " + coins);
        }

        if (collision.gameObject.tag == "CoinGold")
        {
            Destroy(collision.gameObject);
            coins += 5;
            soundeffector.PlayCoinSound();
            //print("You get a coin! Coins count: " + coins);
        }

        if (collision.gameObject.tag == "Heart" && inventory.hp < 9)
        {

            Destroy(collision.gameObject);
            //RecountHp(1);
            inventory.Add_hp();
            soundeffector.PlayItemsSound();

        }
        if (collision.gameObject.tag == "Mushroom")
        {
            //Destroy(collision.gameObject);
            RecountHp(-1);
            //print("You get a poisoned mushroom... " + curHp);
        }

        if (collision.gameObject.tag == "BlueGem" && inventory.bg < 9)
        {
            Destroy(collision.gameObject);
            //StartCoroutine(NoHit());
            inventory.Add_bg();
            soundeffector.PlayItemsSound();

        }
        if (collision.gameObject.tag == "GreenGem" /*&& inventory.gg < 9*/)
        {
            Destroy(collision.gameObject);
            StartCoroutine(SpeedBonus());
            //inventory.Add_gg();
            //soundeffector.PlayItemsSound();
        }
        if (collision.gameObject.tag == "RedGem" && inventory.rg < 9)
        {
            Destroy(collision.gameObject);
            //StartCoroutine(JumpBonus());
            inventory.Add_rg();
            soundeffector.PlayItemsSound();
        }
        if(collision.gameObject.tag == "Lava")
        {
            RecountHp(-5);
        }
        if(collision.gameObject.tag == "TimerButtonStart")
        {
            //Destroy(collision.gameObject);
            //RecountHp(-1);
            insideTimer = 0f;
        }
        if (collision.gameObject.tag == "TimerButtonStop")
        {
            Destroy(collision.gameObject);
            insideTimer = -1f;
            insideCountDown.fillAmount = 0;
            soundeffector.PlayItemsSound();
        }
    }
    IEnumerator TPwait()
    {
        yield return new WaitForSeconds(2.5f);
        canTP = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(0, 0);
            if(Input.GetAxis("Vertical") == 0)
            //if(joystick.Vertical == 0)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * Input.GetAxis("Vertical") * speed * 0.3f * Time.deltaTime);
                //transform.Translate(Vector3.up * joystick.Vertical * speed * 0.5f * Time.deltaTime);
            }
        }
        if(collision.gameObject.tag == "Icy")
        {   if (rb.gravityScale == 1)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
        }
        if(collision.gameObject.tag == "Carpet")
        {
            onCarpet = true;
            anim.SetInteger("State", 1);
        }

        if(collision.gameObject.tag == "PoisonArea") 
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 10f)
            {
                hitTimer = 0f;
                PlayerCountDown.fillAmount = 1f;
                RecountHp(-3);
            }
            else
                PlayerCountDown.fillAmount = 1 - (hitTimer / 10f);
        }
        if (collision.gameObject.tag == "Lava")
        {
            RecountHp(-5);
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ladder")
        {
            isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (collision.gameObject.tag == "Icy")
        {   if (rb.gravityScale == 7)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }
        if (collision.gameObject.tag == "Carpet")
            onCarpet = false;
        if (collision.gameObject.tag == "PoisonArea")
        {
            PlayerCountDown.fillAmount = 0f;
            hitTimer = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trampoline")
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        if(collision.gameObject.tag == "Quicksand")
        {
            speed *= 0.25f;
            //speed = speed * 0.25f;
            rb.mass *= 100f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Quicksand")
        {
            speed *= 4f;
            //speed = speed * 0.25f;
            rb.mass *= 0.01f;
        }
    }
    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);

    }
    IEnumerator NoHit()
    {
        gemCount++;
        blueGem.SetActive(true);
        CheckGems(blueGem);
        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        print("Invulnerability!");
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;
        gemCount--;
        blueGem.SetActive(false);
        CheckGems(greenGem);
        CheckGems(redGem);
        print("Invulnerability lost.");
    }
    IEnumerator SpeedBonus()
    {
        gemCount++;
        greenGem.SetActive(true);
        CheckGems(greenGem);
        speed *= 1.3f;
        print("Haste up!");
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(6f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed /= 1.3f;
        gemCount--;
        greenGem.SetActive(false);
        CheckGems(blueGem);
        CheckGems(redGem);
        print("Haste down.");
    }
    IEnumerator JumpBonus()
    {
        gemCount++;
        redGem.SetActive(true);
        CheckGems(redGem);
        jumpHeight = jumpHeight * 1.3f;
        print("Jumpforce!");
        redGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(9f);
        StartCoroutine(Invis(redGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        jumpHeight = jumpHeight / 1.3f;
        gemCount--;
        redGem.SetActive(false);
        CheckGems(blueGem);
        CheckGems(greenGem);
        print("Jumpforce down.");
    }
    void CheckGems(GameObject obj)
    {
        if (gemCount == 1)
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);
        else if(gemCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
        }
        else if(gemCount == 3)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, blueGem.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, greenGem.transform.localPosition.z);
            redGem.transform.localPosition = new Vector3(0f, 1.17f, greenGem.transform.localPosition.z);
        }
    }
    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(Invis(spr, time));
    }
    public int GetCoins()
    {
        return coins;
    }
    public int GetHP()
    {
        return curHp;
    }

    //Методы для вызора Корутины в скрипте Инвентаря:
    public void BlueGem()
    {
        StartCoroutine(NoHit());
    }

    /*public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    } */

    public void RedGem()
    {
        StartCoroutine(JumpBonus());
    }
}
