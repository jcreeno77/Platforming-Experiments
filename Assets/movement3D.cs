using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement3D : MonoBehaviour
{
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] float camSpeed;
    [SerializeField] float camMinSize;
    // Start is called before the first frame update
    void Start()
    {
        camSpeed = 5;
        camMinSize = 14;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 averageSpot = (player1.transform.position + player2.transform.position) / 2;
        float distance = Vector2.Distance(player1.transform.position, player2.transform.position);
        float x = Mathf.Lerp(transform.position.x, averageSpot.x, Time.deltaTime * camSpeed);
        float y = Mathf.Lerp(transform.position.y, averageSpot.y, Time.deltaTime * camSpeed);
        float z = Mathf.Lerp(transform.position.z, -distance*.4f, Time.deltaTime * camSpeed);
        z = Mathf.Clamp(z, -160, -camMinSize);

        transform.position = new Vector3(x, y, z);
    }
}
