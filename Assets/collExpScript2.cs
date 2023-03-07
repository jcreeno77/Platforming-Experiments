using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class collExpScript2 : MonoBehaviour
{
    [SerializeField] GameObject pole;
    [SerializeField] GameObject player;
    [SerializeField] GameObject root;
    Gamepad controller1;


    // Start is called before the first frame update
    void Start()
    {
        controller1 = player.GetComponent<bambooPlayerScript>().controller1;

    }

    // Update is called once per frame
    private void Update()
    {
        if (controller1 == null)
        {
            controller1 = player.GetComponent<bambooPlayerScript>().controller1;
        }
    }
    void FixedUpdate()
    {
        //if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 6))
        //{
        //    pole.transform.rotation = Quaternion.Euler(pole.transform.rotation.x, pole.transform.rotation.y, pole.transform.rotation.z);
        //}
            

        /*
        //Debug.Log(Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 6));
        //detects if collided
        if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 6))
        {
            //Debug.Log("Basically Here");
            if (!root.GetComponent<CollExpScript>().grounded)
            {
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                pole.GetComponent<Animator>().SetBool("grounded", true);
                pole.GetComponent<Animator>().SetBool("ReleaseFling", false);
                pole.GetComponent<Animator>().SetBool("BeginFling", false);
                pole.GetComponent<Animator>().SetBool("lungeRight", false);
                
                if (!Physics2D.IsTouchingLayers(player.GetComponent<CircleCollider2D>(), 1 << 6))
                {
                    root.GetComponent<CollExpScript>().grounded = true;
                }

                //if (controller1.rightShoulder.isPressed)
                //{
                //    player.GetComponent<bambooPlayerScript>().poleGrabbed = true;
                //}
            }
        }
        else
        {
            grounded = false;
            pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        */

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        //Debug.Log(collision.transform.tag);
        Vector2 norm = collision.GetContact(0).normal;
        for (int i = 0; i < collision.contactCount; i++)
        {
            norm = collision.GetContact(i).normal;
            
        }
        if (collision.gameObject.tag == "ground" && !player.GetComponent<bambooPlayerScript>().dead)
        {
            Debug.Log("hit Ground");
            //pole.transform.rotation = Quaternion.Euler(pole.transform.rotation.x, pole.transform.rotation.y, pole.transform.rotation.z);
            //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            pole.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            pole.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, norm));

        }

    }



}

