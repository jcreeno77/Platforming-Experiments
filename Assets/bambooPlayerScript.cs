using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bambooPlayerScript : MonoBehaviour
{
    float speed;
    float baseSpeed;
    float poleHeight;
    float moveWidth;
    float arialSpeed = 0f;
    float maxArialSpeed = 0f;
    int jumpsLeft = 1;
    int dashesLeft = 1;
    
    [SerializeField] float dashSpeed;
    Vector2 grabLocation;
    Vector2 globalGrabLocation;
    Vector2 flingDirect;
    public bool poleGrabbed;
    [SerializeField] GameObject root;
    [SerializeField] GameObject pole;
    [SerializeField] Sprite[] sprArrayLeft;
    [SerializeField] Sprite[] sprArrayRight;
    [SerializeField] Sprite basePole;
    Sprite poleLeft1;
    Sprite poleLeft2;
    Sprite poleLeft3;
    Sprite poleRight1;
    Sprite poleRight2;
    Sprite poleRight3;

    //Animation
    public Animator animator;
    public float flingDistance;
    Gamepad controller1;
    // Start is called before the first frame update
    void Start()
    {
        poleGrabbed = false;
        baseSpeed = 7f;
        speed = baseSpeed;
        dashSpeed = 20f;
        poleHeight = 5.38f;
        moveWidth = 1f;
        flingDirect = new Vector2(0f, 0f);

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            controller1 = Gamepad.all[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Controller testing
        //Debug.Log(controller1.leftStick.ReadValue().x);
        //Debug.Log(controller1.leftStick.ReadValue().y);

        if (root.GetComponent<CollExpScript>().grounded)
        {
            //Handle up and down movement
            float distance = Vector2.Distance(transform.position, root.transform.position);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (distance < poleHeight)
                {
                    transform.position += transform.up * Time.deltaTime * speed;
                }
                /*if (transform.localPosition.x > (poleHeight - distance) * 4f)
                {
                    transform.position -= transform.right * Time.deltaTime * speed / 2;
                }
                else if (transform.localPosition.x < -(poleHeight - distance) * 4)
                {
                    transform.position += transform.right * Time.deltaTime * speed / 2;
                }*/

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
                if (transform.localPosition.x < moveWidth)
                {
                    transform.position += transform.right * Time.deltaTime * speed / 2;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.localPosition.x > -moveWidth)
                {
                    transform.position -= transform.right * Time.deltaTime * speed / 2;
                }
            }

            //CONTROLLER VERSION
            if (!poleGrabbed)
            {
                //Handle up and down mechanics
                if (controller1.leftStick.up.isPressed)
                {
                    if (distance < poleHeight)
                    {
                        transform.position += transform.up * Time.deltaTime * speed;
                    }

                }
                else if (controller1.leftStick.down.isPressed)
                {
                    if (transform.localPosition.y - root.transform.localPosition.y > 0.3)
                    {
                        transform.position -= transform.up * Time.deltaTime * speed;
                    }
                }
                //Handle side to side mechanics
                if (controller1.leftStick.right.isPressed)
                {
                    if (transform.localPosition.x < moveWidth)
                    {
                        transform.position += transform.right * Time.deltaTime * speed / 2;
                    }
                }
                else if (controller1.leftStick.left.isPressed)
                {
                    if (transform.localPosition.x > -moveWidth)
                    {
                        transform.position -= transform.right * Time.deltaTime * speed / 2;
                    }
                }
            }
            //float xVal = transform.position.x;
            //xVal = Mathf.Clamp(xVal, 0,3f);
            //transform.localPosition = new Vector3(xVal, transform.position.y, transform.position.z);


            //Handle Grab Mechanics
            float localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));
            //Debug.Log(localFlingDistance);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                grabLocation = transform.localPosition;
                globalGrabLocation = transform.TransformPoint(grabLocation);
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));
                //Debug.Log(localFlingDistance);
                float select = localFlingDistance / baseSpeed * (sprArrayLeft.Length);
                Debug.Log((int)select);
                if (transform.localPosition.x < 0)
                {
                    pole.GetComponent<SpriteRenderer>().sprite = sprArrayLeft[(int)select];
                }
                else
                {
                    pole.GetComponent<SpriteRenderer>().sprite = sprArrayRight[(int)select];
                }


                speed = baseSpeed - localFlingDistance;    //*1.2f;
                //Debug.Log("grabLocation: " + grabLocation);
                //Debug.Log("localPosition " + transform.localPosition);
                //Debug.Log(localFlingDistance);
                //Debug.Log(Vector2.Distance(transform.localPosition, grabLocation));
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //GetComponent<SpriteRenderer>().color = Color.white;
                flingDistance = 0;
                speed = baseSpeed;
                globalGrabLocation = transform.TransformPoint(grabLocation);
                Vector2 globalPos = transform.TransformPoint(transform.localPosition);
                Vector2 globalDirectVec = new Vector2(globalGrabLocation.x - globalPos.x, globalGrabLocation.y - globalPos.y);
                Debug.Log(globalDirectVec);
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(20f * globalDirectVec.x, 10f * globalDirectVec.y);
                if (transform.localPosition.x >= 0)
                {
                    pole.GetComponent<Rigidbody2D>().angularVelocity = 200f + transform.localPosition.x * 350f;
                }
                else
                {
                    pole.GetComponent<Rigidbody2D>().angularVelocity = -200f + transform.localPosition.x * 350f;
                }

                //Handles extra details for Pole
                maxArialSpeed = 0f;
                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                pole.GetComponent<SpriteRenderer>().sprite = basePole;
                transform.localPosition = grabLocation;
                transform.localPosition = new Vector2(0f, transform.localPosition.y);
                jumpsLeft = 1;
                dashesLeft = 1;
            }

            //CONTROLLER VERSION
            if (controller1.rightShoulder.wasPressedThisFrame)
            {
                poleGrabbed = true;
                grabLocation = transform.localPosition;
                flingDirect = new Vector2(0f, 0f);
                //GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (controller1.rightShoulder.isPressed)
            {
                float controllerXVal = controller1.leftStick.ReadValue().x;
                float controllerYVal = controller1.leftStick.ReadValue().y;
                flingDirect = new Vector2(Mathf.Lerp(flingDirect.x, -controllerXVal, Time.deltaTime * 5f), Mathf.Lerp(flingDirect.y, -controllerYVal, Time.deltaTime * 5f));

                //GetAngle
                //Debug.Log(Vector2.Angle(pole.transform.up, flingDirect));

                //pole anim
                float select = Vector2.Distance(new Vector2(0, 0), flingDirect) / 1.1f * (sprArrayLeft.Length);
                //Debug.Log((int)select);
                if (controllerXVal < 0)
                {
                    pole.GetComponent<SpriteRenderer>().sprite = sprArrayLeft[(int)select];
                }
                else
                {
                    pole.GetComponent<SpriteRenderer>().sprite = sprArrayRight[(int)select];
                }

            }
            if (controller1.rightShoulder.wasReleasedThisFrame)
            {
                float angle = Vector2.Angle(pole.transform.up, flingDirect);
                maxArialSpeed = 0f;
                poleGrabbed = false;
                //GetComponent<SpriteRenderer>().color = Color.white;
                speed = baseSpeed;
                pole.GetComponent<Rigidbody2D>().velocity = flingDirect * 25f;
                
                //Handles angular Velocity
                float angDisToNinety = Mathf.Abs(90 - angle);
                float amplifier = (90 - angDisToNinety) / 90;
                float angVel = (-flingDirect.x * 700f * amplifier);
                if (pole.transform.up.y >= 0)
                {
                    if (angVel >= 0)
                    {
                        angVel = Mathf.Clamp(angVel, 300, 700);
                    }
                    else
                    {
                        angVel = Mathf.Clamp(angVel, -700, -300);
                    }
                    pole.GetComponent<Rigidbody2D>().angularVelocity = angVel;


                }
                else
                {
                    pole.GetComponent<Rigidbody2D>().angularVelocity = flingDirect.x * 250f + (550f * amplifier);
                }

                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                pole.GetComponent<SpriteRenderer>().sprite = basePole;
                jumpsLeft = 1;
                dashesLeft = 1;
            }
        }
        else
        {
            //arial Movement Mechanics
            Vector2 currPoleVel = pole.GetComponent<Rigidbody2D>().velocity;
            //arial jump
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft -= 1;
                    pole.GetComponent<Rigidbody2D>().velocity = new Vector2(currPoleVel.x, 35f);
                }
            }
            maxArialSpeed = Mathf.Max(maxArialSpeed, Mathf.Abs(currPoleVel.x));
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(currPoleVel.x-(10f*Time.deltaTime),-maxArialSpeed,maxArialSpeed),currPoleVel.y);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(currPoleVel.x + (12f * Time.deltaTime),-maxArialSpeed,maxArialSpeed), currPoleVel.y);
            }          
   
            //AERIAL DASH MECHANICS
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
                if (dashesLeft > 0)
                {
                    dashesLeft -= 1;
                    Vector2 dashDirect = new Vector2(horizontal, vertical);
                    pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
                    pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                    float angle = Vector2.Angle(new Vector2(horizontal, vertical), Vector3.up);
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
            //Controller Version
            if (controller1.rightShoulder.wasPressedThisFrame)
            {
                if (controller1.leftStick.up.isPressed)
                {
                    vertical += 1;
                }
                if (controller1.leftStick.down.isPressed)
                {
                    vertical -= 1;
                }
                if (controller1.leftStick.right.isPressed)
                {
                    horizontal += 1;
                }

                if (controller1.leftStick.left.isPressed)
                {
                    horizontal -= 1;
                }
                
                if (horizontal == 0 && vertical == 0)
                {
                    vertical = -1;
                }

                Vector2 dashDirect = new Vector2(horizontal, vertical);
                
                pole.GetComponent<Rigidbody2D>().velocity = dashDirect * dashSpeed;
                pole.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                float angle = Vector2.Angle(new Vector2(horizontal, vertical), Vector3.up);
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
