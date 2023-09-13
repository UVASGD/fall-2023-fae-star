using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectTransition : MonoBehaviour
{
    // These reference the specific character objects to be transitioned
    [SerializeField] GameObject[] characterPortraits;
    [SerializeField] GameObject[] characterSprites;
    [SerializeField] Transform[] basePositions;
    [SerializeField] Transform[] reservePositions;
    [SerializeField] Transform forwardPosition;
    [SerializeField] RectTransform[] portraitBasePositions;
    [SerializeField] RectTransform portraitSelectedPosition;

    // This references the display objects that will be set inactive in the transition
    [SerializeField] List<GameObject> oldDisplay;

    // This references the display objects that will be set active in the transition
    [SerializeField] List<GameObject> newDisplay;

    // Length of the transition in frames
    [SerializeField] int transitionLength;

    private int selectedCharacter;
    private bool reversed;
    private int frameCount;

    // Update is called once per frame
    void Update()
    {
        // Code to transition back
        if(reversed)
        {
            int at = 0;
            float progress = (float)frameCount / transitionLength;
            for (int i = 0; i < characterSprites.Length; i++)
            {
                if (i == selectedCharacter)
                {
                    characterSprites[i].transform.position = Vector3.Lerp(forwardPosition.position, basePositions[i].position, progress);
                    float scale = 1f - 0.25f * progress;
                    characterSprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    characterSprites[i].transform.position = Vector3.Lerp(reservePositions[at].position, basePositions[i].position, progress);
                    at++;
                }
            }

            characterPortraits[selectedCharacter].transform.position = Vector3.Lerp(portraitSelectedPosition.position, portraitBasePositions[selectedCharacter].position, progress);

            if (frameCount == transitionLength)
            {
                foreach (GameObject g in oldDisplay)
                    g.SetActive(true);
                for (int i = 0; i < characterPortraits.Length; i++)
                {
                    if (i != selectedCharacter)
                        characterPortraits[i].SetActive(true);
                }
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            int at = 0;
            float progress = (float)frameCount / transitionLength;
            for (int i = 0; i < characterSprites.Length; i++)
            {
                if (i == selectedCharacter)
                {
                    characterSprites[i].transform.position = Vector3.Lerp(basePositions[i].position, forwardPosition.position, progress);
                    float scale = 0.75f + 0.25f * progress;
                    characterSprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    characterSprites[i].transform.position = Vector3.Lerp(basePositions[i].position, reservePositions[at].position, progress);
                    at++;
                }
            }

            characterPortraits[selectedCharacter].transform.position = Vector3.Lerp(portraitBasePositions[selectedCharacter].position, portraitSelectedPosition.position, progress);

            if(frameCount == transitionLength)
            {
                foreach (GameObject g in newDisplay)
                    g.SetActive(true);
                this.gameObject.SetActive(false);
            }
        }

        // Increment Frame Count
        if(frameCount < transitionLength)
            frameCount++;
    }

    void Transition(int characterPosition)
    {
        reversed = false;
        selectedCharacter = characterPosition;

        foreach (GameObject g in oldDisplay)
        {
            if (g.activeSelf != false)
                g.SetActive(false);
        }
        for(int i = 0; i < characterPortraits.Length; i++)
        {
            if (i != selectedCharacter)
                characterPortraits[i].SetActive(false);
        }
        frameCount = 0;
    }

    public void ReverseTransition()
    {
        reversed = true;
        foreach (GameObject g in newDisplay)
        {
            if(g.activeSelf != false)
                g.SetActive(false);
        }
        frameCount = 0;
    }
}
