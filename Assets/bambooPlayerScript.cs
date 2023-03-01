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
    float deathCount = 0;
    int scoreIndex;
    bool deathAdded;
    //float arialSpeed = 0f;
    float maxArialSpeed = 0f;
    int jumpsLeft = 1;
    int dashesLeft = 1;
    [System.NonSerialized] public bool offPole;
    [System.NonSerialized] public bool dead;

    [SerializeField] int airDashes;
    [SerializeField] float dashSpeed;
    [SerializeField] float flingForce;
    [SerializeField] float flingPullSpeed = 15f;
    Vector2 grabLocation;
    Vector2 globalGrabLocation;
    Vector2 flingDirect;
    [System.NonSerialized] public bool poleGrabbed;
    [SerializeField] GameObject root;
    [SerializeField] GameObject pole;
    [SerializeField] GameObject controllerRef;
    [SerializeField] GameObject partEff;
    [SerializeField] GameObject scoreObj;
    [SerializeField] PhysicMaterial frictionless;
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
    Animator poleAnimator;
    public float flingDistance;
    public Gamepad controller1;

    // Start is called before the first frame update
    private void Reset()
    {
        deathCount = 0;
        deathAdded = false;
        flingForce = 50f;
        dashSpeed = 50f;
        poleGrabbed = false;
        baseSpeed = 7f;
        speed = baseSpeed;
        
        poleHeight = 5.38f;
        moveWidth = 1f;
        flingDirect = new Vector2(0f, 0f);

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (i >= 4)
            {
                break;
            }
            //controller1 = Gamepad.all[i];
            Debug.Log(controllerRef.GetComponent<controllerUsedArray>().controllerArray[0]);
            Debug.Log(i);
            bool iInArr = false;
            foreach (int item in controllerRef.GetComponent<controllerUsedArray>().controllerArray)
            {
                if (i == item)
                {
                    iInArr = true;
                }
            }
            if (!iInArr)
            {
                controllerRef.GetComponent<controllerUsedArray>().controllerArray[i] = i;
                controller1 = Gamepad.all[i];
                break;
            }
        }
    }
    void Start()
    {
        deathCount = 0;
        airDashes = 2;
        flingForce = 50f;
        dashSpeed = 50f;
        poleGrabbed = false;
        baseSpeed = 7f;
        speed = baseSpeed;
        flingPullSpeed = 15f;

        scoreObj.GetComponent<keepCount>().scores.Add((int)deathCount);
        scoreIndex = scoreObj.GetComponent<keepCount>().scores.Count-1;


        poleHeight = 5.38f;
        moveWidth = 1f;
        flingDirect = new Vector2(0f, 0f);

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            if (i >= 4)
            {
                break;
            }
            //controller1 = Gamepad.all[i];
            Debug.Log(controllerRef.GetComponent<controllerUsedArray>().controllerArray[0]);
            Debug.Log(i);
            bool iInArr = false;
            foreach (int item in controllerRef.GetComponent<controllerUsedArray>().controllerArray)
            {
                if (i == item)
                {
                    iInArr = true;
                }
            }
            if (!iInArr)
            {
                controllerRef.GetComponent<controllerUsedArray>().controllerArray[i] = i;
                controller1 = Gamepad.all[i];
                break;
            }
        }
        if (controller1 == null)
        {
            Destroy(transform.parent.gameObject);
        }
        poleAnimator = transform.parent.gameObject.GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        //Ground Collision
        if (Physics2D.IsTouchingLayers(GetComponent<CircleCollider2D>(), 1 << 6) && !root.GetComponent<CollExpScript>().grounded)
        {
            if (gameObject.GetComponent<Rigidbody2D>() == null)
            {
                transform.parent = null;
                gameObject.AddComponent<Rigidbody2D>();
                root.SetActive(false);
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<Rigidbody2D>().angularDrag = 1f;
                GetComponent<Rigidbody2D>().velocity = new Vector2(pole.GetComponent<Rigidbody2D>().velocity.x, pole.GetComponent<Rigidbody2D>().velocity.y);
                jumpsLeft = 0;
                dashesLeft = 0;
                dead = true;
                GameObject pF = Instantiate(partEff);
                pF.transform.position = transform.position;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            
            if (!deathAdded)
            {
                deathAdded = true;
                deathCount += 1;
                scoreObj.GetComponent<keepCount>().scores[scoreIndex] = (int)deathCount;
            }
            GetComponent<SpriteRenderer>().enabled = true;
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            root.SetActive(false);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Rigidbody2D>().angularDrag = 1f;
            GetComponent<Rigidbody2D>().velocity = new Vector2(pole.GetComponent<Rigidbody2D>().velocity.x, pole.GetComponent<Rigidbody2D>().velocity.y);
            jumpsLeft = 0;
            dashesLeft = 0;
            GetComponent<SpriteRenderer>().color = Color.red;
            pole.GetComponent<SpriteRenderer>().sprite = basePole;
            pole.GetComponent<Animator>().enabled = false;
        }
        else
        {
            deathAdded = false;
        }
        
        if (root.GetComponent<CollExpScript>().grounded)
        {
            //Handle up and down movement
            float distance = Vector2.Distance(transform.position, root.transform.position);


            //CONTROLLER VERSION
            //handle left right
            if (!poleGrabbed)
            {
                if (controller1.leftStick.left.isPressed)
                {
                    poleAnimator.SetBool("Left_of_Pole", true);
                    poleAnimator.SetBool("Right_of_Pole", false);
                }
                else if (controller1.leftStick.right.isPressed)
                {
                    poleAnimator.SetBool("Left_of_Pole", false);
                    poleAnimator.SetBool("Right_of_Pole", true);
                }
            }

                //handle up down
                /*
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

                }
                */
                //Handle Grab Mechanics
                float localFlingDistance = (Mathf.Pow((grabLocation.x - transform.localPosition.x) * pole.transform.localScale.x, 2) + Mathf.Pow((grabLocation.y - transform.localPosition.y) * pole.transform.localScale.y, 2));
            //Debug.Log(localFlingDistance);
            //CONTROLLER VERSION
            if (controller1.rightShoulder.wasPressedThisFrame)
            {
                poleGrabbed = true;
                //grabLocation = transform.localPosition;
                flingDirect = new Vector2(0f, 0f);
                //GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (controller1.rightShoulder.isPressed && poleGrabbed)
            {
                float controllerXVal = controller1.leftStick.ReadValue().x;
                float controllerYVal = controller1.leftStick.ReadValue().y;
                flingDirect = new Vector2(Mathf.Lerp(flingDirect.x, -controllerXVal, Time.deltaTime * flingPullSpeed), Mathf.Lerp(flingDirect.y, -controllerYVal, Time.deltaTime * flingPullSpeed));

                //if (Mathf.Abs(controller1.leftStick.ReadValue().x) > )

                //GetAngle
                //Debug.Log(Vector2.Angle(pole.transform.up, flingDirect));

                //pole anim
                float select = Vector2.Distance(new Vector2(0, 0), flingDirect) / 1f * 4f;

                if((int)select > 0)
                {
                    poleAnimator.SetBool("BeginFling", true);
                    //GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    poleAnimator.SetBool("BeginFling", false);
                    //GetComponent<SpriteRenderer>().enabled = true;
                }

                //Debug.Log((int)select);
                float flingAngle = Vector2.Angle(transform.right, -flingDirect);
                if (flingAngle > 90)
                {
                    poleAnimator.SetBool("FlingAngleLeft", true);
                    poleAnimator.SetBool("FlingAngleRight", false);
                    //pole.GetComponent<SpriteRenderer>().sprite = sprArrayLeft[(int)select];
                }
                else
                {
                    poleAnimator.SetBool("FlingAngleLeft", false);
                    poleAnimator.SetBool("FlingAngleRight", true);
                    //pole.GetComponent<SpriteRenderer>().sprite = sprArrayRight[(int)select];
                }
                
            }
            if (controller1.rightShoulder.wasReleasedThisFrame && poleGrabbed)
            {
                poleAnimator.SetBool("FlingAngleLeft", false);
                poleAnimator.SetBool("FlingAngleRight", false);
                poleAnimator.SetBool("BeginFling", false);
                poleAnimator.SetBool("ReleaseFling", true);
                poleAnimator.SetBool("grounded", false);
                
                float angle = Vector2.Angle(pole.transform.up, flingDirect);
                maxArialSpeed = 0f;
                poleGrabbed = false;
                //GetComponent<SpriteRenderer>().color = Color.white;
                speed = baseSpeed;
                pole.GetComponent<Rigidbody2D>().velocity = new Vector2(flingDirect.x * flingForce, flingDirect.y * flingForce * 1.25f);

                //Handles angular Velocity
                float flingAngle = Vector2.Angle(transform.right, new Vector2(-flingDirect.x, -flingDirect.y));
                float amplifier;
                //Sets amplifier

                if (flingAngle > 90)
                {
                    amplifier = -flingAngle/180;
                }
                else
                {
                    amplifier = (180-flingAngle)/180;
                }
                //Sets Anglular Velocity
                float angVel = (flingDirect.magnitude * 700f * amplifier);
                //Adjusts min and max clamp
                if (angVel >= 0)
                {
                    angVel = Mathf.Clamp(angVel, 300, 700);
                }
                else
                {
                    angVel = Mathf.Clamp(angVel, -700, -300);
                }
                //Activates da spin
                pole.GetComponent<Rigidbody2D>().angularVelocity = angVel;

                pole.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                
                //reset to straight pole: PASS TO ANIMATOR
                // timers stagger reflex animation -> basePole
                //setBool in Animator BEGIN Release
                
                jumpsLeft = 1;
                dashesLeft = airDashes;
                flingDirect = new Vector2(0f, 0f);
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
            

            //Controller Version
            //aerial Jump
            if (controller1.buttonSouth.wasPressedThisFrame && !GetComponent<fightMovement>().inAttack)
            {
                if (jumpsLeft > 0)
                {
                    jumpsLeft -= 1;
                    pole.GetComponent<Rigidbody2D>().velocity = new Vector2(currPoleVel.x, 35f);
                }
            }

            //aerial Dash
            if (controller1.rightShoulder.wasPressedThisFrame && !GetComponent<fightMovement>().inAttack)
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
                if (dashesLeft > 0 /*&& vertical <= 0*/)
                {
                    poleAnimator.SetBool("lungeRight", true);
                    dashesLeft -= 1;
                    Vector2 dashDirect = new Vector2(horizontal, vertical).normalized;
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

            //aerial rotation
            if (controller1.rightStick.right.isPressed)
            {
                pole.GetComponent<Rigidbody2D>().angularVelocity -= 500 * Time.deltaTime;
            }
            else if (controller1.rightStick.left.isPressed)
            {
                pole.GetComponent<Rigidbody2D>().angularVelocity += 500 * Time.deltaTime;
            }
            pole.GetComponent<Rigidbody2D>().angularVelocity = Mathf.Clamp(pole.GetComponent<Rigidbody2D>().angularVelocity, -700f, 700f);

        }
    }

}

//Extra stuff for keyboard
/*if (Input.GetKey(KeyCode.UpArrow))
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
                //refer to animator
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
             */



//Handle side to side mechanics
/*if (controller1.leftStick.right.isPressed)
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
}*/

//controller stuff again
/*
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
                
            }*/