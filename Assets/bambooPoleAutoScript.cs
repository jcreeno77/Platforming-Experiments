using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bambooPoleAutoScript : MonoBehaviour
{
    [SerializeField] float gravity;
    [SerializeField] GameObject player;
    public bool grounded = true;
    public bool alwaysTrue = true;
    public float timer;

    Vector2 startPos;
    Quaternion startRotation;
    Rigidbody2D body;
    Gamepad controller1;
    // Start is called before the first frame update
    private void Reset()
    {
        gravity = 2500;
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
        startRotation = transform.rotation;

        controller1 = player.GetComponent<bambooPlayerAutoScript>().controller1;
    }

    void Start()
    {
        gravity = 2500;
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
        startRotation = transform.rotation;

        controller1 = player.GetComponent<bambooPlayerAutoScript>().controller1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (controller1 == null)
        {
            controller1 = player.GetComponent<bambooPlayerAutoScript>().controller1;
        }
        //added Gravity
        if (body.bodyType == RigidbodyType2D.Dynamic)
        {
            body.AddForce(new Vector2(0f, -gravity * Time.deltaTime));
        }

        //reduce top speed



        if (Input.GetKeyDown(KeyCode.R) || controller1.startButton.wasPressedThisFrame)
        {
            transform.position = startPos;
            transform.rotation = startRotation;
            body.bodyType = RigidbodyType2D.Kinematic;
            body.velocity = new Vector2(0f, 0f);
            body.angularVelocity = 0f;


            //set reset player stuff
            Destroy(player.gameObject.GetComponent<Rigidbody2D>());
            player.GetComponent<bambooPlayerAutoScript>().offPole = false;
            player.transform.parent = transform;
            player.transform.localPosition = new Vector2(-.58f, 4.94f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer <= 0)
        {
            //Debug.Log(collision.transform.tag);
            for (int i = 0; i < collision.contactCount; i++)
            {
                Debug.Log(collision.GetContact(i).normal);
                Vector2 norm = collision.GetContact(i).normal;
                if (Physics2D.IsTouchingLayers(GetComponent<CircleCollider2D>(), 1 << 6))
                {
                    grounded = true;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    GetComponent<Rigidbody2D>().angularVelocity = 0f;
                    transform.rotation = Quaternion.Euler(0,0,Vector2.Angle(Vector2.up, norm));
                    
                }
            }
        }
        
    }

}

