using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class keepCount : MonoBehaviour
{
    public List<int> scores = new List<int>();
    //[SerializeField] TextMeshProUGUI obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = "";
        int player = 0;
        foreach (int score in scores)
        {
            player++;
            GetComponent<TextMeshProUGUI>().text = GetComponent<TextMeshProUGUI>().text + "P"+player.ToString()+": " + score + System.Environment.NewLine;
        }
    }
}
