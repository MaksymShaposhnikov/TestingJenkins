using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform point1, point2;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(point1.position.x, point1.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, point2.position, speed * Time.deltaTime);
        if (transform.position == point2.position)
        {
            Transform t = point1;
            point1 = point2;
            point2 = t;
        }
    }
}
