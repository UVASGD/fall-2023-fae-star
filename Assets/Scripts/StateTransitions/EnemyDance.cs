using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDance : MonoBehaviour, ITransition
{
    private int counter;

    // These reference the specific character objects to be transitioned
    [SerializeField] GameObject[] characterSprites;
    [SerializeField] Transform[] basePositionsCharacters;
    [SerializeField] Transform[] reservePositionsCharacters;
    [SerializeField] Transform forwardPositionCharacter;
    GameObject[] enemySprites;
    [SerializeField] Transform[] basePositionsEnemies;
    [SerializeField] Transform[] reservePositionsEnemies;
    [SerializeField] Transform forwardPositionEnemy;
    [SerializeField] GameObject[] characterInfos;

    // This references the display objects that will be set inactive in the transition
    [SerializeField] List<GameObject> tempTurnOff;

    // Length of the transition in frames
    [SerializeField] int transitionLength;

    private int selectedCharacter;
    public static bool reversed;
    private static int frameCount;
    private bool invoked;

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
            for (int i = 0; i < characterSprites.Length; i++)
            {
                if (i == selectedCharacter)
                {
                    characterSprites[i].transform.position = Vector3.Lerp(forwardPositionCharacter.position, basePositionsCharacters[i].position, progress);
                    float scale = 1f - 0.25f * progress;
                    characterSprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    characterSprites[i].transform.position = Vector3.Lerp(reservePositionsCharacters[at].position, basePositionsCharacters[i].position, progress);
                    at++;
                }
            }

            at = 0;
            for (int i = 0; i < enemySprites.Length; i++)
            {
                if (i == counter)
                {
                    enemySprites[i].transform.position = Vector3.Lerp(forwardPositionEnemy.position, basePositionsEnemies[i].position, progress);
                    float scale = 1f - 0.25f * progress;
                    enemySprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    enemySprites[i].transform.position = Vector3.Lerp(reservePositionsEnemies[at].position, basePositionsEnemies[i].position, progress);
                    at++;
                }
            }

            if (frameCount == transitionLength)
            {
                counter++;
                chooseEnemyAttack();
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
                    characterSprites[i].transform.position = Vector3.Lerp(basePositionsCharacters[i].position, forwardPositionCharacter.position, progress);
                    float scale = 0.75f + 0.25f * progress;
                    characterSprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    characterSprites[i].transform.position = Vector3.Lerp(basePositionsCharacters[i].position, reservePositionsCharacters[at].position, progress);
                    at++;
                }
            }

            at = 0;
            for (int i = 0; i < characterSprites.Length; i++)
            {
                if (i == counter)
                {
                    enemySprites[i].transform.position = Vector3.Lerp(basePositionsEnemies[i].position, forwardPositionEnemy.position, progress);
                    float scale = 0.75f + 0.25f * progress;
                    characterSprites[i].transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    enemySprites[i].transform.position = Vector3.Lerp(basePositionsEnemies[i].position, reservePositionsEnemies[at].position, progress);
                    at++;
                }
            }

            if (frameCount == transitionLength && !invoked)
            {
                Debug.Log(GlobalStateTracker.currentAction);
                GlobalMoveDictionary.invoke(GlobalStateTracker.currentAction);
                invoked = true;
            }
        }

        // Increment Frame Count
        if (frameCount < transitionLength)
            frameCount++;
    }

    private void chooseEnemyAttack()
    {
        counter = TransitionManager.findNextUnlocked(6, counter);
        if(counter < GlobalEntityStats.enemies.Count && GlobalEntityStats.enemies[counter] != null)
        {
            GlobalStateTracker.currentEntity = TransitionManager.enemies[counter];
            Enemy enemy = GlobalEntityStats.enemies[counter];
            GlobalStateTracker.currentAction = enemy.moves[Random.Range(0, enemy.moves.Length)];
            selectedCharacter = Random.Range(0, GlobalEntityStats.characters.Count);
            GlobalStateTracker.targetEntity = TransitionManager.characters[selectedCharacter];
            invoked = false;
            reversed = false;
            frameCount = 0;
        }
        else
        {
            changeInfoDisplays(true);
            foreach (GameObject g in tempTurnOff)
            {
                g.SetActive(true);
            }
            GlobalStateTracker.battleState = GlobalStateTracker.States.CharacterSelect;
            GlobalStateTracker.currentEntity = null;
            GlobalStateTracker.targetEntity = null;
            GlobalStateTracker.currentAction = null;
            this.gameObject.SetActive(false);
            TransitionManager.nextTurn();
        }
    }

    public static void startReverse()
    {
        reversed = true;
        frameCount = 0;
    }

    private void changeInfoDisplays(bool sliderVal)
    {
        foreach (GameObject g in characterInfos)
        {
            g.transform.GetChild(1).gameObject.SetActive(sliderVal);
            g.transform.GetChild(2).gameObject.SetActive(sliderVal);
            g.transform.GetChild(5).gameObject.SetActive(!sliderVal);
        }
    }

    public void Transition(int characterPosition)
    {
        counter = 0;
        changeInfoDisplays(false);
        foreach(GameObject g in tempTurnOff)
        {
            g.SetActive(false);
        }
        chooseEnemyAttack();
        gameObject.SetActive(true);
    }

    public void ReverseTransition()
    {
        // Never Called
    }
}
