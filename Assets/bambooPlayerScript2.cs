using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bambooPlayerScript2 : MonoBehaviour
{
    float speed;
    float baseSpeed;
    [SerializeField] float dashSpeed;
    Vector2 grabLocation;
    bool onGround = false;
    [SerializeField] GameObject root;
    [SerializeField] GameObject pole;

    Gamepad controller2;
    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = 2f;
        speed = baseSpeed;
        dashSpeed = 8;

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            controller2 = Gamepad.all[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (root.GetComponent<CollExpScript>().grounded)
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
            //Controller version
            if (controller2.leftStick.up.isPressed)
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
            else if (controller2.leftStick.down.isPressed)
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
            //Controller version
            if (controller2.leftStick.right.isPressed)
            {
                if (transform.localPosition.x < (2.553f - distance) * 4f)
                {
                    transform.position += transform.right * Time.deltaTime * speed / 2;
                }
            }
            else if (controller2.leftStick.left.isPressed)
            {
                if (transform.localPosition.x > -(2.553f - distance) * 4f)
                {
                    transform.position -= transform.right * Time.deltaTime * speed / 2;
                }
            }
            //float xVal = transform.position.x;
            //xVal = Mathf.Clamp(xVal, 0,3f);
            //Debug.Log(xVal);
            //transform.localPosition = new Vector3(xVal, transform.position.y, transform.position.z);


            //Handle Grab Mechanics
        
            float localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                grabLocation = transform.localPosition;
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));

                speed = baseSpeed - localFlingDistance;    //*1.2f;
                Debug.Log("grabLocation: " + grabLocation);
                Debug.Log("localPosition " + transform.localPosition);
                Debug.Log(localFlingDistance);
                Debug.Log(Vector2.Distance(transform.localPosition, grabLocation));
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                speed = baseSpeed;
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localPosition.x / 2 * pole.transform.right.x, (pole.transform.right.y+1) * 8f * localFlingDistance);
                pole.GetComponent<Rigidbody2D>().angularVelocity = transform.localPosition.x * 150f;
                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                transform.localPosition = grabLocation;
            }
            //Controller Version
            if (controller2.rightShoulder.wasPressedThisFrame)
            {
                grabLocation = transform.localPosition;
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (controller2.rightShoulder.isPressed)
            {
                localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));

                speed = baseSpeed - localFlingDistance;    //*1.2f;
                Debug.Log("grabLocation: " + grabLocation);
                Debug.Log("localPosition " + transform.localPosition);
                Debug.Log(localFlingDistance);
                Debug.Log(Vector2.Distance(transform.localPosition, grabLocation));
            }
            if (controller2.rightShoulder.wasReleasedThisFrame)
            {
                GetComponent<SpriteRenderer>().color = Color.green;
                speed = baseSpeed;
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(-transform.localPosition.x / 2 * pole.transform.right.x, (pole.transform.right.y + 1) * 8f * localFlingDistance);
                pole.GetComponent<Rigidbody2D>().angularVelocity = transform.localPosition.x * 150f;
                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                transform.localPosition = grabLocation;
            }
        }
        else
        {
            //arial mechanics
            float horizontal = 0;
            float vertical = 0;
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    vertical += 1;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    vertical -= 1;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    horizontal += 1;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontal -= 1;
                }
                Vector2 dashDirect = new Vector2(horizontal, vertical);
                pole.GetComponent<Rigidbody2D>().velocity = dashDirect*dashSpeed;
                pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                float angle = Vector2.Angle(new Vector2(horizontal,vertical),Vector3.up);
                //Quaternion angles = Quaternion.LookRotation(dashDirect, Vector2.up);
                Debug.Log(angle);
                if(horizontal < 0)
                {
                    pole.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
                }
                else
                {
                    pole.transform.rotation = Quaternion.Euler(0f, 0f, 180 - angle);
                }
            }
            //Controller Version
            if (controller2.rightShoulder.wasPressedThisFrame)
            {
                if (controller2.leftStick.up.isPressed)
                {
                    vertical += 1;
                }
                if (controller2.leftStick.down.isPressed)
                {
                    vertical -= 1;
                }
                if (controller2.leftStick.right.isPressed)
                {
                    horizontal += 1;
                }

                if (controller2.leftStick.left.isPressed)
                {
                    horizontal -= 1;
                }
                Vector2 dashDirect = new Vector2(horizontal, vertical);
                pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
                pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                float angle = Vector2.Angle(new Vector2(horizontal, vertical), Vector3.up);
                //Quaternion angles = Quaternion.LookRotation(dashDirect, Vector2.up);
                Debug.Log(angle);
                if (horizontal < 0)
                {
                    pole.transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
                }
                else
                {
                    pole.transform.rotation = Quaternion.Euler(0f, 0f, 180 - angle);
                }
            }
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
