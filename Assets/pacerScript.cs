using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacerScript : MonoBehaviour
{
    int direction;
    float timer = 1f;
    float timerSet = 1f;
    // Start is called before the first frame update
    void Start()
    {
        direction = -1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x + 10f * direction * Time.deltaTime, transform.position.y);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Player")
        {
            direction = direction * -1;
        }
    }
}
