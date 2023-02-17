using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralaxBG : MonoBehaviour
{
    [SerializeField] GameObject cam;
    Vector2 camPos;
    // Start is called before the first frame update
    void Start()
    {
        camPos = cam.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 posDiff = new Vector2(cam.transform.position.x, cam.transform.position.y) - camPos;
        transform.position = (posDiff/2.5f) + new Vector2(transform.position.x, transform.position.y);

        //Sets sizes for next update
        camPos = cam.transform.position;
    }
}
