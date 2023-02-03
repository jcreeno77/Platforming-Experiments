using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CollExpScript : MonoBehaviour
{
    [SerializeField] GameObject pole;
    [SerializeField] GameObject player1;
    Gamepad controller1;
    public bool grounded = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            controller1 = Gamepad.all[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 6));
        //detects if collided
        if (Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(),1 << 6))
        {
            //Debug.Log("Basically Here");
            if (!grounded)
            {
                //Debug.Log("Here");
                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                grounded = true;
                if (controller1.rightShoulder.isPressed)
                {
                    player1.GetComponent<bambooPlayerScript>().poleGrabbed = true;
                }
            }
        }
        else
        {
            grounded = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("coll");
    }
}
