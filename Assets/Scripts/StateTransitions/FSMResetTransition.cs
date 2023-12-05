using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMResetTransition : MonoBehaviour, ITransition
{
    [SerializeField] GameObject[] characterSprites;
    [SerializeField] Transform[] characterBasePositions;
    private Vector3[] currentCharacterPositions;
    GameObject[] enemySprites;
    [SerializeField] Transform[] enemyBasePositions;
    private Vector3[] currentEnemyPositions;
    [SerializeField] GameObject[] characterPortraits;
    [SerializeField] RectTransform[] portraitBasePositions;

    [SerializeField] CharacterSelectTransition cst;
    [SerializeField] EnemySelectTransition est;

    [SerializeField] List<GameObject> oldDisplay;
    [SerializeField] List<GameObject> newDisplay;

    [SerializeField] int transitionLength;

    private int frameCount;
    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        currentCharacterPositions = new Vector3[4];
        currentEnemyPositions = new Vector3[4];
        enemySprites = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            enemySprites[i] = GameObject.Find("Enemy" + (i + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            float progress = (float)frameCount / transitionLength;
            for (int i = 0; i < characterSprites.Length; i++)
            {
                characterSprites[i].transform.position = Vector3.Lerp(currentCharacterPositions[i], characterBasePositions[i].position, progress);
                if (characterSprites[i].transform.localScale.x > 0.75f)
                {
                    float scale = 1f - 0.25f * progress;
                    characterSprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
            }

            for (int i = 0; i < enemySprites.Length; i++)
            {
                enemySprites[i].transform.position = Vector3.Lerp(currentEnemyPositions[i], enemyBasePositions[i].position, progress);
                if (enemySprites[i].transform.localScale.x > 0.75f)
                {
                    float scale = 1f - 0.25f * progress;
                    enemySprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
            }


            if (frameCount == transitionLength)
            {
                for(int i = 0; i < characterPortraits.Length; i++)
                {
                    characterPortraits[i].transform.position = portraitBasePositions[i].position;
                }
                cst.reset();
                est.reset();
                foreach (GameObject g in oldDisplay)
                    g.SetActive(false);
                foreach (GameObject g in newDisplay)
                    g.SetActive(true);
                GlobalStateTracker.battleState = GlobalStateTracker.States.CharacterSelect;
                GlobalStateTracker.currentEntity = null;
                GlobalStateTracker.currentAction = null;
                GlobalStateTracker.currentItem = null;
                GlobalStateTracker.targetEntity = null;
                activated = false;
                TransitionManager.FSMResetCallback();
            }

            // Increment Frame Count
            if (frameCount < transitionLength)
                frameCount++;
        }
    }

    public void Transition(int characterPosition)
    {
        for(int i = 0; i < characterSprites.Length; i++)
        {
            currentCharacterPositions[i] = characterSprites[i].transform.position;
        }
        for (int i = 0; i < enemySprites.Length; i++)
        {
            currentEnemyPositions[i] = enemySprites[i].transform.position;
        }
        frameCount = 0;
        activated = true;
    }

    public void ReverseTransition() { /* DO NOTHING */ }
}
