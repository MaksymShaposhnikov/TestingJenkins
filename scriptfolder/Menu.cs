using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Button[] lvls;
    public Button bluePlayer,greenPlayer,pinkPlayer,yellowPlayer;
    public Button shopBluePlayer, shopGreenPlayer;
    public GameObject yellowPlayerTip, pinkPlayerTip, greenPlayerTip, bluePlayerTip;
    public GameObject[] lvlsButtons;
    public Text coinText;
    public Slider musicSlider, soundSlider;
    public Text musicText, soundText;
    public AudioSource musicSource, soundSource;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 9;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 9;

        if (PlayerPrefs.GetInt("Lvl") == 8)
            yellowPlayer.interactable = true;

        if (PlayerPrefs.HasKey("Lvl"))
            for (int i = 0; i < lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                {
                    lvls[i].interactable = true;
                    lvlsButtons[i].SetActive(true);
                }
                else
                {
                    lvlsButtons[i].SetActive(false);
                    lvls[i].interactable = false;
                }
                    
            }
        if (!PlayerPrefs.HasKey("AllStars"))
        {
            PlayerPrefs.SetInt("AllStars", 0);
        }

        if (!PlayerPrefs.HasKey("hp"))
        {
            PlayerPrefs.SetInt("hp", 0);
        }

        if (!PlayerPrefs.HasKey("bg"))
        {
            PlayerPrefs.SetInt("bg", 0);
        }

        if (!PlayerPrefs.HasKey("gg"))
        {
            PlayerPrefs.SetInt("gg", 0);
        }

        if (!PlayerPrefs.HasKey("rg"))
        {
            PlayerPrefs.SetInt("rg", 0);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetInt("MusicVolume", 7);
        }

        if (!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetInt("SoundVolume", 7);
        }
        musicSlider.value = PlayerPrefs.GetInt("MusicVolume");
        soundSlider.value = PlayerPrefs.GetInt("SoundVolume");
        

        if (PlayerPrefs.GetInt("AllStars") >= 21)
        { 
            pinkPlayer.interactable = true;
            pinkPlayerTip.SetActive(false);
        } 


        if (!PlayerPrefs.HasKey("Player"))
            PlayerPrefs.SetInt("Player", 0);

        if (PlayerPrefs.GetString("BluePlayerUnlocked") == "true")
        {
            bluePlayer.interactable = true;
            shopBluePlayer.interactable = false;
        }
        if (PlayerPrefs.GetString("GreenPlayerUnlocked") == "true")
        {
            greenPlayer.interactable = true;
            shopGreenPlayer.interactable = false;
        }
        if (yellowPlayer.interactable == false)
        {
            yellowPlayerTip.SetActive(true);
        }

        if (pinkPlayer.interactable == false)
        {
            pinkPlayerTip.SetActive(true);
        }

        if (greenPlayer.interactable == false)
        {
            greenPlayerTip.SetActive(true);
        }

        if (bluePlayer.interactable == false)
        {
            bluePlayerTip.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";

        PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);
        PlayerPrefs.SetInt("SoundVolume", (int)soundSlider.value);
        musicText.text = musicSlider.value.ToString();
        soundText.text = soundSlider.value.ToString();
    }
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Buy_hp(int cost)
    {
        if(PlayerPrefs.GetInt("coins") >= cost && PlayerPrefs.GetInt("hp") < 9)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }

    public void Buy_bg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost && PlayerPrefs.GetInt("bg") < 9)
        {
            PlayerPrefs.SetInt("bg", PlayerPrefs.GetInt("bg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }

    /*public void Buy_gg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost && PlayerPrefs.GetInt("gg") < 9)
        {
            PlayerPrefs.SetInt("gg", PlayerPrefs.GetInt("gg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    } */

    public void Buy_rg(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost && PlayerPrefs.GetInt("rg") < 9)
        {
            PlayerPrefs.SetInt("rg", PlayerPrefs.GetInt("rg") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }

    public void Buy_BluePlayer(int cost)
    {
        if(PlayerPrefs.GetInt("coins") >= cost)
        {
            bluePlayer.interactable = true;
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
            PlayerPrefs.SetString("BluePlayerUnlocked", "true");
            shopBluePlayer.interactable = false;
            bluePlayerTip.SetActive(false);
        }
    }
    public void Buy_GreenPlayer(int cost)
    {
        if(PlayerPrefs.GetInt("coins") >= cost)
        {
            greenPlayer.interactable = true;
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
            PlayerPrefs.SetString("GreenPlayerUnlocked", "true");
            shopGreenPlayer.interactable = false;
            greenPlayerTip.SetActive(false);
        }
    }

    public void SetPlayer(int index)
    {
        PlayerPrefs.SetInt("Player", index);
    }
}
