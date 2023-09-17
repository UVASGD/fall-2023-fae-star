using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] GameObject[] characterInfos;
    [SerializeField] GameObject selectedcharacterInfo;

    // This references the display objects that will be set inactive in the transition
    [SerializeField] List<GameObject> oldDisplay;

    // This references the display objects that will be set active in the transition
    [SerializeField] List<GameObject> newDisplay;

    // Length of the transition in frames
    [SerializeField] int transitionLength;

    private Slider[] selectedInfoSliders;
    private TextMeshProUGUI selectedName;
    private Slider[,] characterSliders;
    private TextMeshProUGUI[] characterNames;
    private int selectedCharacter;
    private bool reversed;
    private int frameCount;

    void Awake()
    {
        // Grabs info for the main character info display and individual character info displays
        selectedInfoSliders = new Slider[3];
        for(int i = 0; i < 3; i++)
        {
            selectedInfoSliders[i] = selectedcharacterInfo.transform.GetChild(i).GetComponent<Slider>();
        }
        selectedName = selectedcharacterInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>();

        characterSliders = new Slider[characterInfos.Length, 3];
        characterNames = new TextMeshProUGUI[characterInfos.Length];
        for (int i = 0; i < characterInfos.Length; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                characterSliders[i, j] = characterInfos[i].transform.GetChild(j).GetComponent<Slider>();
            }
            characterNames[i] = characterInfos[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        }
    }

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
                foreach (GameObject g in characterInfos)
                {
                    g.SetActive(true);
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
        foreach(GameObject g in characterInfos)
        {
            if (g.activeSelf != false)
                g.SetActive(false);
        }


        // Alters the main character info display to represent the selected character
        selectedName.text = characterNames[selectedCharacter].text;
        for(int i = 0; i < 3; i++)
        {
            selectedInfoSliders[i].value = characterSliders[selectedCharacter, i].value;
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
