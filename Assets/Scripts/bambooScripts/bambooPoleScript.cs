using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bambooPoleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
