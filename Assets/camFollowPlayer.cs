using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;
    float xVal;
    float yVal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(xVal, yVal+2.5f,-10f);
        xVal = Mathf.Lerp(xVal, player.transform.position.x, 2f * Time.deltaTime);
        yVal = Mathf.Lerp(yVal, player.transform.position.y, 2f * Time.deltaTime);
        GetComponent<Camera>().orthographicSize = 16;
    }
}
