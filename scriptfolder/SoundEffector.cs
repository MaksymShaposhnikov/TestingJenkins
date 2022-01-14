using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound, coinSound, winSound, loseSound, itemsSound, leverSwitchSound, hotBarSound;

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
    public void PlayItemsSound()
    {
        audioSource.PlayOneShot(itemsSound);
    }
    public void PlayLeverSound()
    {
        audioSource.PlayOneShot(leverSwitchSound);
    }
    public void HotBarSound()
    {
        audioSource.PlayOneShot(hotBarSound);
    }
}
