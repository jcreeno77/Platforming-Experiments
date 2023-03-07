using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smashBrosCam : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] float camSpeed;
    [SerializeField] float camMinSize;
    // Start is called before the first frame update
    void Start()
    {
        camSpeed = 5;
        camMinSize = -20;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 averageSpot = (player1.transform.position + player2.transform.position) / 2;
        float distance = Vector2.Distance(player1.transform.position, player2.transform.position);
        float x = Mathf.Lerp(transform.position.x, averageSpot.x, Time.deltaTime * camSpeed);
        float y = Mathf.Lerp(transform.position.y, averageSpot.y, Time.deltaTime * camSpeed);

        //took out orhtographic camera for testing need way to control zoom via perspective camera

        transform.position = new Vector3(x, y, camMinSize);
    }

    /*transform.position = new Vector3(x, y, -10);
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, distance/2, Time.deltaTime* camSpeed);
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, camMinSize, 160);
    }*/

}
