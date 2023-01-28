using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bambooPlayerScript : MonoBehaviour
{
    float speed;
    float baseSpeed;
    Vector2 grabLocation;
    bool onGround = false;
    [SerializeField] GameObject root;
    [SerializeField] GameObject pole;
    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = 2f;
        speed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Handle up and down movement
        float distance = Vector2.Distance(transform.position, root.transform.position);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (distance < 2.553)
            {
                transform.position += transform.up * Time.deltaTime * speed;
            }
            if (transform.localPosition.x > (2.553f - distance) * 4f)
            {
                transform.position -= transform.right * Time.deltaTime * speed / 2;
            }
            else if (transform.localPosition.x < -(2.553f - distance) * 4)
            {
                transform.position += transform.right * Time.deltaTime * speed / 2;
            }
            
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (distance > 0.3)
            {
                transform.position -= transform.up * Time.deltaTime * speed;
            }
            
        }
        //Handle side to side mechanics
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.localPosition.x < (2.553f - distance)*4f)
            {
                transform.position += transform.right * Time.deltaTime * speed/2;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.localPosition.x > -(2.553f - distance)*4f)
            {
                transform.position -= transform.right * Time.deltaTime * speed/2;
            }
        }
        //float xVal = transform.position.x;
        //xVal = Mathf.Clamp(xVal, 0,3f);
        //Debug.Log(xVal);
        //transform.localPosition = new Vector3(xVal, transform.position.y, transform.position.z);


        //Handle Grab Mechanics
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            grabLocation = transform.position;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = baseSpeed - Vector2.Distance(grabLocation, transform.position)*1.2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            speed = baseSpeed;
            pole.GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localPosition.x/2, 8f * Vector2.Distance(transform.position,grabLocation));
            pole.GetComponent<Rigidbody2D>().angularVelocity = transform.localPosition.x * 150f;
            pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            transform.position = grabLocation;
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 norm = collision.GetContact(i).normal;
            print(norm);
            //onGround |= (norm.y >= .9f);
        }
    } 
}
