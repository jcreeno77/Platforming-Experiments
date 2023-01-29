using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bambooPoleScript : MonoBehaviour
{
    Vector2 startPos;
    Quaternion startRotation;
    Rigidbody2D body;
    Gamepad controller1;
    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.R) || controller1.buttonWest.wasPressedThisFrame)
        {
            transform.position = startPos;
            transform.rotation = startRotation;
            body.bodyType = RigidbodyType2D.Kinematic;
            body.velocity = new Vector2(0f, 0f);
            body.angularVelocity = 0f;
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
