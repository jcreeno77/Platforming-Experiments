using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacerScript : MonoBehaviour
{
    int direction;
    float timer = 1f;
    float timerSet = 1f;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        direction = -1;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(200f * direction * Time.deltaTime, 0);
    }
    void Update()
    {
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            direction = direction * -1;
        }
    }
}
