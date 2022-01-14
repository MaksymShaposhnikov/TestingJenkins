using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 6f, ForceMode2D.Impulse);
            gameObject.GetComponentInParent<Enemy>().startDeath();
        }
    }
}