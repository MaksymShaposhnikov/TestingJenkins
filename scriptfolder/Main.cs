using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject star1, star2, star3;
    private Player player;
    public Transform playerPos;
    public GameObject[] players;
    //public Transform playerCountDownPos; //иконки
    //public Image[] playerIcons; //иконки
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    float timer = 0f;
    public Text timeText;
    public TimeWork timeWork;
    public float countDown;
    public GameObject inventoryPan;
    public SoundEffector soundeffector;
    public AudioSource musicSource, soundSource;
    //public GameObject insideCountDown;
    //public GameObject PlayerCountDown;
    public void ReloadLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Awake()
    {
        player = Instantiate(players[PlayerPrefs.GetInt("Player")], playerPos.position, Quaternion.identity).GetComponent<Player>();
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("AllStars") >= 21)
        {
            star1.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            star1.GetComponent<Collider2D>().enabled = false;
            star2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            star2.GetComponent<Collider2D>().enabled = false;
            star3.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);
            star3.GetComponent<Collider2D>().enabled = false;
        }
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 9;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 9;


        if ((int)timeWork == 2)
        {
            Time.timeScale = 1f;
            timer = countDown;
        }

        //player = Instantiate(players[PlayerPrefs.GetInt("Player")], playerPos.position, Quaternion.identity).GetComponent<Player>();
    }
    private void Update()
    {
        coinText.text = player.GetCoins().ToString();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHP() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }

        if ((int)timeWork == 1)
        {
            //Time.timeScale = 1f;
            timer += Time.deltaTime;
            //timeText.text = timer.ToString("F2").Replace(",", ":");
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");
        }
        else if ((int)timeWork == 2)
        {
            timer -= Time.deltaTime;
            //timeText.text = timer.ToString("F2").Replace(",", ":");
            timeText.text = ((int)timer / 60).ToString() + ":" + ((int)timer - ((int)timer / 60) * 60).ToString("D2");
            if (timer <= 0)
                Lose();
        }
        else
        {
            //Time.timeScale = 1f;
            timeText.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOn();
        }
    }
    public void PauseOn()
    {   
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);

    }
    public void PauseOff()
    {   
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }
    public void Win()
    {
        soundeffector.PlayWinSound();
        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);
        //print(PlayerPrefs.GetInt("Lvl"));

        if (PlayerPrefs.HasKey("coins"))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        else
            PlayerPrefs.SetInt("coins", player.GetCoins());

        print(PlayerPrefs.GetInt("coins"));

        inventoryPan.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }
    public void Lose()
    {
        soundeffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false;
        LoseScreen.SetActive(true);

        inventoryPan.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }
    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("Menu");
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

public enum TimeWork
{
    None,
    StopWatch,
    Timer
}

