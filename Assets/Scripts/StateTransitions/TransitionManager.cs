using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] Transform battleContainer;
    public static List<GameObject> characters;
    public static GameObject[] enemies;

    [SerializeField] GameObject[] portraitObjects;
    public static Dictionary<GameObject, GameObject> portraits;

    /*
     * Transition Orders:
     * 0: Character Select -> Action Select
     * 1-4: Action Select -> Respective Character Actions
     * 5: Character Actions -> Single Target Enemy Select
     * 6-9: Character Actions -> Single Target Assist Select
     * 10-13: Item Select -> Respective Character Items
     * 14 Single Target Enemy Select -> invoke()
     * 15 Reset the FSM
     * 16 Start the Enemy turn
     */
    [SerializeField] GameObject[] serializedTransitionsObject;
    private static List<GameObject> transitionsObjects;
    private static List<ITransition> transitions;
    /* 
     * Selection Orders:
     * 0: Character Select
     * 1: Action Select
     * 2-5: Action List Selects
     * 6: Enemy Select
     * 7: Ally Select
     * 8-11: Item List Selects
     */
    [SerializeField] Selection[] selectionObjects;
    private static List<Selection> selections;
    // Literally only used in one place :(
    private static int enterForCharacterSelection;
    private static int enterForEnemySelection;

    [SerializeField] TextMeshProUGUI turnCounterObject;
    private static TextMeshProUGUI turnCounter;
    private static int turn;

    [SerializeField] GameObject battleFinishObject;
    private static GameObject battleFinish;

    private static int characterIndex;

    void Start()
    {
        transitionsObjects = new List<GameObject>();
        foreach (GameObject g in serializedTransitionsObject)
        {
            transitionsObjects.Add(g);
        }
        transitions = new List<ITransition>();
        for (int i = 0; i < transitionsObjects.Count; i++)
        {
            transitions.Add(transitionsObjects[i].GetComponent(typeof(ITransition)) as ITransition);
        }
        selections = new List<Selection>();
        foreach (Selection s in selectionObjects)
        {
            selections.Add(s);
        }
        characters = new List<GameObject>();
        for(int i = 0; i < 4; i++)
        {
            characters.Add(battleContainer.GetChild(i).gameObject);
        }
        enemies = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            enemies[i] = GameObject.Find("Enemy" + (i + 1));
        }
        portraits = new Dictionary<GameObject, GameObject>();
        for(int i = 0; i < characters.Count; i++)
        {
            portraits[characters[i]] = portraitObjects[i];
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            portraits[enemies[i]] = portraitObjects[i + 4];
        }
        turnCounter = turnCounterObject;
        battleFinish = battleFinishObject;
        turn = 1;
        enterForCharacterSelection = 11;
        enterForEnemySelection = 11;
    }
    
    public static void processTransition(int selected)
    {
        switch (GlobalStateTracker.battleState)
        {
            case GlobalStateTracker.States.CharacterSelect:
                if (selected != 4)
                {
                    if (selections[0].checkLock(enterForCharacterSelection))
                    {
                        selections[0].gameObject.SetActive(true);
                        return;
                    }
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                    characterIndex = selected;
                    transitions[0].Transition(selected);
                }
                else
                {
                    enemyTurn();
                }
                break;

            case GlobalStateTracker.States.ActionSelect:
                if (selected == 0) // Action button is selected
                {
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionMenuing;
                    if (GlobalStateTracker.currentEntity == characters[0]) 
                        transitions[1].Transition(selected);
                    else if (GlobalStateTracker.currentEntity == characters[1]) 
                        transitions[2].Transition(selected);
                    else if (GlobalStateTracker.currentEntity == characters[2]) 
                        transitions[3].Transition(selected);
                    else 
                        transitions[4].Transition(selected);
                }
                else if (selected == 1) // Item button is selected
                {
                    // TODO Implement Item Menu, for now we will just do nothing
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ItemMenuing;
                    if (GlobalStateTracker.currentEntity == characters[0])
                        transitions[10].Transition(selected);
                    else if (GlobalStateTracker.currentEntity == characters[1])
                        transitions[11].Transition(selected);
                    else if (GlobalStateTracker.currentEntity == characters[2])
                        transitions[12].Transition(selected);
                    else
                        transitions[13].Transition(selected);
                }
                else if (selected == 2) // Guard button is selected
                {

                }
                else // Back button is selected
                {
                    reverseTransition();
                }
                break;
            case GlobalStateTracker.States.ItemMenuing:
                KeyValuePair<string, (Item, int)> selectedItem = GlobalItemLists.ItemList.ElementAt(selected);
                if (selectedItem.Value.Item1 == null)
                {
                    reverseTransition();
                    break;
                }
                switch (selectedItem.Value.Item1.getActionType())
                {
                    case Item.ActionTypes.PB:
                    case Item.ActionTypes.MA:
                        // Should just Act here as there is no need to select anyone
                        GlobalStateTracker.battleState = GlobalStateTracker.States.Acting;
                        GlobalStateTracker.currentAction = selectedItem.Key;
                        selectedItem.Value.Item1.invoke();
                        reverseTransition();
                        break;

                }
                break;

            case GlobalStateTracker.States.ActionMenuing:
                KeyValuePair<string, (Move.ActionTypes, int, int)> selectedMove = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                if(selectedMove.Key == "Back")
                {
                    reverseTransition();
                    break;
                }
                switch (selectedMove.Value.Item1)
                {
                    case Move.ActionTypes.SE: // Single target enemy action selected
                        GlobalStateTracker.battleState = GlobalStateTracker.States.PostActionEntitySelect;
                        GlobalStateTracker.currentAction = selectedMove.Key;
                        transitions[5].Transition(selected);

                        break;

                    case Move.ActionTypes.SA: // Single target ally action selected
                        GlobalStateTracker.battleState = GlobalStateTracker.States.PostActionEntitySelect;
                        GlobalStateTracker.currentAction = selectedMove.Key;
                        if (GlobalStateTracker.currentEntity == characters[0])
                        {
                            transitions[6].Transition(selected);
                        }
                        else if (GlobalStateTracker.currentEntity == characters[1])
                        {
                            transitions[7].Transition(selected);
                        }
                        else if (GlobalStateTracker.currentEntity == characters[2])
                        {
                            transitions[8].Transition(selected);
                        }
                        else
                        {
                            transitions[9].Transition(selected);
                        }

                        break;

                    case Move.ActionTypes.PB:
                    case Move.ActionTypes.ME:
                    case Move.ActionTypes.MA:
                        // Should just Act here as there is no need to select anyone
                        GlobalStateTracker.battleState = GlobalStateTracker.States.Acting;
                        GlobalStateTracker.currentAction = selectedMove.Key;
                        GlobalMoveDictionary.invoke(GlobalStateTracker.currentAction);
                        reverseTransition();
                        break;

                }
                break;

            case GlobalStateTracker.States.PostActionEntitySelect:
                if (selections[6].checkLock(enterForEnemySelection))
                {
                    selections[6].gameObject.SetActive(true);
                    return;
                }
                if(selected == 4)
                {
                    reverseTransition();
                    return;
                }
                GlobalStateTracker.battleState = GlobalStateTracker.States.Acting;
                switch (GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item1)
                {
                    case Move.ActionTypes.SE:
                        GlobalStateTracker.targetEntity = enemies[selected];
                        ((EnemySelectTransition)transitions[14]).setSelectedCharacter(characterIndex);
                        transitions[14].Transition(selected);
                        break;
                    case Move.ActionTypes.SA:
                        GlobalStateTracker.targetEntity = characters[selected];
                        GlobalMoveDictionary.invoke(GlobalStateTracker.currentAction);
                        break;
                    default:
                        break;
                }
                //KeyValuePair<string, (int, GlobalMoveLists.ActionTypes, Action, int)> move = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                //manabar.value -= (float) move.Value.Item1 / 10;
                reverseTransition();
                break;

            default:
                reverseTransition();
                break;
        }


        //Debug.Log(GlobalStateTracker.toString());
    }

    public static void reverseTransition()
    {
        switch (GlobalStateTracker.battleState)
        {
            case GlobalStateTracker.States.ActionSelect:
                GlobalStateTracker.battleState = GlobalStateTracker.States.CharacterSelect;
                GlobalStateTracker.currentEntity = null;
                transitionsObjects[0].SetActive(true);
                transitions[0].ReverseTransition();
                break;

            case GlobalStateTracker.States.ActionMenuing:
                GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                if (GlobalStateTracker.currentEntity == characters[0])
                {
                    transitionsObjects[1].SetActive(true);
                    transitions[1].ReverseTransition();
                }
                else if (GlobalStateTracker.currentEntity == characters[1])
                {
                    transitionsObjects[2].SetActive(true);
                    transitions[2].ReverseTransition();
                }
                else if (GlobalStateTracker.currentEntity == characters[2])
                {
                    transitionsObjects[3].SetActive(true);
                    transitions[3].ReverseTransition();
                }
                else
                {
                    transitionsObjects[4].SetActive(true);
                    transitions[4].ReverseTransition();
                }
                break;

            case GlobalStateTracker.States.ItemMenuing:
                GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                // TODO Implement Item Menu
                if (GlobalStateTracker.currentEntity == characters[0])
                {
                    transitionsObjects[10].SetActive(true);
                    transitions[10].ReverseTransition();
                }
                else if (GlobalStateTracker.currentEntity == characters[1])
                {
                    transitionsObjects[11].SetActive(true);
                    transitions[11].ReverseTransition();
                }
                else if (GlobalStateTracker.currentEntity == characters[2])
                {
                    transitionsObjects[12].SetActive(true);
                    transitions[12].ReverseTransition();
                }
                else
                {
                    transitionsObjects[13].SetActive(true);
                    transitions[13].ReverseTransition();
                }
                break;

            case GlobalStateTracker.States.PostActionEntitySelect:
                if (GlobalStateTracker.currentAction != null)
                {
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionMenuing;
                    if(GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item1 == Move.ActionTypes.SE)
                    {
                        GlobalStateTracker.currentAction = null;
                        transitionsObjects[5].SetActive(true);
                        transitions[5].ReverseTransition();
                    }
                    else if(GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item1 == Move.ActionTypes.SA)
                    {
                        if (GlobalStateTracker.currentEntity == characters[0])
                        {
                            transitionsObjects[6].SetActive(true);
                            transitions[6].ReverseTransition();
                        }
                        else if (GlobalStateTracker.currentEntity == characters[1])
                        {
                            transitionsObjects[7].SetActive(true);
                            transitions[7].ReverseTransition();
                        }
                        else if (GlobalStateTracker.currentEntity == characters[2])
                        {
                            transitionsObjects[8].SetActive(true);
                            transitions[8].ReverseTransition();
                        }
                        else
                        {
                            transitionsObjects[9].SetActive(true);
                            transitions[9].ReverseTransition();
                        }
                    }
                }
                break;
        }
        //Debug.Log(GlobalStateTracker.toString());
    }

    public static void ResetFSM()
    {
        selections[0].setLock(10 + characters.IndexOf(GlobalStateTracker.currentEntity) + 1, true);
        transitions[15].Transition(0);
    }

    public static void FSMResetCallback()
    {
        bool allLocked = true;
        for (int i = 0; i < 4; i++)
        {
            allLocked = allLocked && selections[6].checkLock(10 + i + 1);
        }
        if (allLocked)
        {
            finishBattle();
            return;
        }
        allLocked = true;
        for (int i = 0; i < 4; i++)
        {
            allLocked = allLocked && selections[0].checkLock(10 + i + 1);
        }
        if (allLocked)
        {
            enemyTurn();
        }
        int nextCharacterSelect = 11;
        while (selections[0].checkLock(nextCharacterSelect))
            nextCharacterSelect++;
        enterForCharacterSelection = nextCharacterSelect;
        selections[0].SetSelectSecret(nextCharacterSelect);
        int nextEnemySelect = 11;
        while (selections[6].checkLock(nextEnemySelect))
            nextEnemySelect++;
        enterForEnemySelection = nextEnemySelect;
        selections[6].SetSelectSecret(nextEnemySelect);
    }

    public static void enemyTurn()
    {
        GlobalStateTracker.battleState = GlobalStateTracker.States.EnemyActions;
        transitions[16].Transition(0);
    }

    public static void nextTurn()
    {
        selections[0].clearLocks();
        selections[0].SetSelectSecret(11);
        selections[6].SetSelectSecret(11);
        turn++;
        turnCounter.text = "Turn " + turn;
        selections[0].gameObject.SetActive(true);
    }

    public static void finishBattle()
    {
        battleFinish.SetActive(true);
    }

    public static void setLock(int selector, int lockval, bool value)
    {
        selections[selector].setLock(lockval, value);
    }


    // Stupid fucking functions for the stupidest functionality you will ever see
    public static void setEnterForCharacterSelection(int xy)
    {
        if(GlobalStateTracker.battleState == GlobalStateTracker.States.CharacterSelect)
            enterForCharacterSelection = xy;
    }
    // Stupid fucking functions for the stupidest functionality you will ever see
    public static void setEnterForEnemySelection(int xy)
    {
        enterForEnemySelection = xy;
    }

    public static int findNextUnlocked(int selection, int start)
    {
        int nextSelect = 10 + start + 1;
        while (selections[selection].checkLock(nextSelect))
            nextSelect++;
        return nextSelect % 10 - 1;
    }
}
