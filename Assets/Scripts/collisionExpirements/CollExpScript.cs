using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollExpScript : MonoBehaviour
{
    [SerializeField] GameObject pole;
    public bool grounded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Physics2D.IsTouchingLayers(GetComponent<BoxCollider2D>(), 1 << 6));
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
