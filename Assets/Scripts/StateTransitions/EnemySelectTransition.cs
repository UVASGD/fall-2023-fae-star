using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectTransition : MonoBehaviour, ITransition
{
    GameObject[] enemySprites;
    [SerializeField] Transform[] basePositions;
    [SerializeField] Transform[] reservePositions;
    [SerializeField] Transform forwardPosition;

    // Length of the transition in frames
    [SerializeField] int transitionLength;

    // Display stuff
    [SerializeField] List<GameObject> oldDisplay;

    private int selectedCharacter;
    private int selectedEnemy;
    private bool reversed;
    private bool firstActivation;
    private int frameCount;

    void Start()
    {
        enemySprites = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            enemySprites[i] = GameObject.Find("Enemy" + (i + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Code to transition back
        if (reversed)
        {
            int at = 0;
            float progress = (float)frameCount / transitionLength;
            for (int i = 0; i < enemySprites.Length; i++)
            {
                if (i == selectedEnemy)
                {
                    enemySprites[i].transform.position = Vector3.Lerp(forwardPosition.position, basePositions[i].position, progress);
                    float scale = 1f - 0.25f * progress;
                    enemySprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    enemySprites[i].transform.position = Vector3.Lerp(reservePositions[at].position, basePositions[i].position, progress);
                    at++;
                }
            }

            if (frameCount == transitionLength)
            {

            }
        }
        else if (firstActivation)
        {
            int at = 0;
            float progress = (float)frameCount / transitionLength;
            for (int i = 0; i < enemySprites.Length; i++)
            {
                if (i == selectedEnemy)
                {
                    enemySprites[i].transform.position = Vector3.Lerp(basePositions[i].position, forwardPosition.position, progress);
                    float scale = 0.75f + 0.25f * progress;
                    enemySprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    enemySprites[i].transform.position = Vector3.Lerp(basePositions[i].position, reservePositions[at].position, progress);
                    at++;
                }
            }

            if (frameCount == transitionLength)
            {
                this.gameObject.SetActive(false);
                GlobalMoveDictionary.invoke(GlobalStateTracker.currentAction);
            }
        }

        // Increment Frame Count
        if (frameCount < transitionLength)
            frameCount++;
    }

    public void Transition(int characterPosition)
    {
        // Ping first activation
        firstActivation = true;

        foreach (GameObject g in oldDisplay)
        {
            if (g.activeSelf != false)
                g.SetActive(false);
        }

        // Start transition
        reversed = false;
        selectedEnemy = characterPosition;


        // Alters the main character info display to represent the selected character
        frameCount = 0;
    }

    public void ReverseTransition()
    {
        // Start Reverse Transition
        reversed = true;
        frameCount = 0;
    }

    public void setSelectedCharacter(int selected)
    {
        this.selectedCharacter = selected;
    }

    public void reset()
    {
        firstActivation = false;
    }
}
