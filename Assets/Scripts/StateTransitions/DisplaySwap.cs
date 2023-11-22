using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySwap : MonoBehaviour, ITransition
{
    // This references the display objects that will be set inactive in the transition
    [SerializeField] List<GameObject> oldDisplay;

    // This references the display objects that will be set active in the transition
    [SerializeField] List<GameObject> newDisplay;

    // This references the position changing objects along with their new and old positions
    [SerializeField] List<GameObject> positionChangingObjects;
    [SerializeField] List<Transform> newPositions;
    private List<Vector3> oldPositions;

    private void Awake()
    {
        oldPositions = new List<Vector3>(new Vector3[newPositions.Count]);
    }


    public void Transition(int selected)
    {
        // Visual swaps
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

        // Position changes
        for(int i = 0; i < positionChangingObjects.Count; i++)
        {
            oldPositions[i] = positionChangingObjects[i].transform.position;
            positionChangingObjects[i].transform.position = newPositions[i].position;
        }
    }

    public void ReverseTransition()
    {
        // Visual swaps
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

        // Position changes
        // Position changes
        for (int i = 0; i < positionChangingObjects.Count; i++)
        {
            positionChangingObjects[i].transform.position = oldPositions[i];
        }
    }
}
