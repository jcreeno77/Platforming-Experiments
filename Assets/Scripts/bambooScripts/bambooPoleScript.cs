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
        gravity = 2500;
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
        startRotation = transform.rotation;

        controller1 = player.GetComponent<bambooPlayerScript>().controller1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller1 == null)
        {
            controller1 = player.GetComponent<bambooPlayerScript>().controller1;
        }
        //added Gravity
        if (body.bodyType == RigidbodyType2D.Dynamic)
        {
            body.AddForce(new Vector2(0f, -gravity*Time.deltaTime));
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
        //Debug.Log(collision.transform.tag);
        if (collision.transform.tag == "Player" && collision.transform != player.transform && root.GetComponent<CollExpScript>().playerHit)
        {
            Debug.Log("KILL");
            if(collision.transform.Find("bambooPlayer") != null)
            {
                collision.transform.Find("bambooPlayer").gameObject.GetComponent<bambooPlayerScript>().dead = true;
            }
            
        }
    }
}
