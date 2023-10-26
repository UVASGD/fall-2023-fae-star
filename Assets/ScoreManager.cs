using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI actionCounter;

    private void Awake()
    {
        instance = this;
    }

    int actionCount = 3;
    // Start is called before the first frame update
    void Start()
    {
        actionCounter.text = ("Moves remaining: " + actionCount).ToString();
    }

    // Update is called once per frame
    public void subtractActions()
    {
        actionCount -= 1;
        if(actionCount == 0)
        {
            actionCounter.text = "No Moves Left!";
        }
        else
        {
            actionCounter.text = ("Moves remaining: " + actionCount).ToString();
        }
    }

    public void resetCounter()
    {
        actionCount = 3;
    }
}
