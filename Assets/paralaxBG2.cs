using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralaxBG2 : MonoBehaviour
{
    [SerializeField] GameObject cam;
    float camPosY;
    float camSize;
    // Start is called before the first frame update
    void Start()
    {
        camPosY = cam.transform.position.y;
        camSize = cam.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float posDiff = camPosY - cam.transform.position.y;
        float sizeDiff = cam.GetComponent<Camera>().orthographicSize - camSize;

        transform.localScale = new Vector2(transform.localScale.x+(sizeDiff/5.5f), transform.localScale.y + (sizeDiff / 5.5f));
        transform.position = new Vector2(transform.position.x, transform.position.y + sizeDiff*4.87f);
        transform.position = new Vector2(transform.position.x, transform.position.y + posDiff * .2f);
        
        //set vals for next frame
        camPosY = cam.transform.position.y;
        camSize = cam.GetComponent<Camera>().orthographicSize;
    }
}
