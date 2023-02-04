using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bambooPoleScript : MonoBehaviour
{
    [SerializeField] float gravity;
    [SerializeField] GameObject root;
    [SerializeField] GameObject player;
    Vector2 startPos;
    Quaternion startRotation;
    Rigidbody2D body;
    Gamepad controller1;
    // Start is called before the first frame update
    void Start()
    {
        gravity = 20f;
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
        startRotation = transform.rotation;

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            controller1 = Gamepad.all[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //added Gravity
        if (body.bodyType == RigidbodyType2D.Dynamic)
        {
            body.AddForce(new Vector2(0f, -gravity));
        }

        //reduce top speed
        
        

        if (Input.GetKeyDown(KeyCode.R) || controller1.startButton.wasPressedThisFrame)
        {
            transform.position = startPos;
            transform.rotation = startRotation;
            body.bodyType = RigidbodyType2D.Kinematic;
            body.velocity = new Vector2(0f, 0f);
            body.angularVelocity = 0f;
            root.SetActive(true);

            //set reset player stuff
            Destroy(player.gameObject.GetComponent<Rigidbody2D>());
            player.GetComponent<bambooPlayerScript>().offPole = false;
            player.transform.parent = transform;
            player.transform.localPosition = new Vector2(-.58f, 4.94f);
            player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 norm = collision.GetContact(i).normal;
            //print(norm);
            //onGround |= (norm.y >= .9f);
        }
    }
}
