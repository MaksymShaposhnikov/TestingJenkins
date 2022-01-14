using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverByMyself : MonoBehaviour
{
    public GameObject[] block;
    public Sprite leverSwitched;
    public SoundEffector soundeffector;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = leverSwitched;
            GetComponent<CircleCollider2D>().enabled = false;
            soundeffector.PlayLeverSound();
            foreach (GameObject obj in block)
            {
                Destroy(obj);
            }
        }
    }
}
