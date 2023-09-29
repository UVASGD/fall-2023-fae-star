using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySwap : MonoBehaviour, Transition
{
    // This references the display objects that will be set inactive in the transition
    [SerializeField] List<GameObject> oldDisplay;

    // This references the display objects that will be set active in the transition
    [SerializeField] List<GameObject> newDisplay;

    public void Transition(int selected)
    {
        foreach (GameObject g in oldDisplay)
        {
            if (g.activeSelf != false)
                g.SetActive(false);
        }
        foreach (GameObject g in newDisplay)
        {
            if (g.activeSelf != true)
                g.SetActive(true);
        }
    }

    public void ReverseTransition()
    {
        foreach (GameObject g in newDisplay)
        {
            if (g.activeSelf != false)
                g.SetActive(false);
        }
        foreach (GameObject g in oldDisplay)
        {
            if (g.activeSelf != true)
                g.SetActive(true);
        }
    }
}
