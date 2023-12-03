using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] GameObject[] characterObjects;
    private static List<GameObject> characters;

    /*
     * Transition Orders:
     * 0: Character Select -> Action Select
     * 1-4: Action Select -> Respective Character Actions
     * 5: Character Actions -> Single Target Enemy Select
     * 6-9: Character Actions -> Single Target Assist Select
     * 10-13: Item Select -> Respective Character Items
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
    // change later to use global storage
    [SerializeField] Slider manabarObject;
    private static Slider manabar;

    private static int characterIndex;

    void Awake()
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
        foreach (GameObject g in characterObjects)
        {
            characters.Add(g);
        }
        manabar = manabarObject;
    }
    
    public static void processTransition(int selected)
    {
        switch (GlobalStateTracker.battleState)
        {
            case GlobalStateTracker.States.CharacterSelect:
                if (selected != 4)
                {
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                    characterIndex = selected;
                    transitions[0].Transition(selected);
                }
                else
                {
                    // This will later be turned into a valid "Skip turn" block which actually works, but I am lazy right now and am just going to do this
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                    characterIndex = 0;
                    transitions[0].Transition(0);
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
                KeyValuePair<string, (Item, int)> selectedItem = GlobalItemLists.ItemList[characterIndex].ElementAt(selected);
                if (selectedItem.Value.Item1 == null)
                {
                    reverseTransition();
                    break;
                }
                switch (selectedItem.Value.Item1.getActionType())
                {
                    case Item.ActionTypes.SE: // Single target enemy action selected
                        GlobalStateTracker.battleState = GlobalStateTracker.States.PostActionEntitySelect;
                        GlobalStateTracker.currentAction = selectedItem.Key;
                        transitions[5].Transition(selected);

                        break;

                    case Item.ActionTypes.SA: // Single target ally action selected
                        GlobalStateTracker.battleState = GlobalStateTracker.States.PostActionEntitySelect;
                        GlobalStateTracker.currentAction = selectedItem.Key;
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

                    case Item.ActionTypes.PB:
                    case Item.ActionTypes.ME:
                    case Item.ActionTypes.MA:
                        // Should just Act here as there is no need to select anyone
                        reverseTransition();
                        break;

                }
                break;

            case GlobalStateTracker.States.ActionMenuing:
                KeyValuePair<string, (Move, int)> selectedMove = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                if(selectedMove.Value.Item1 == null)
                {
                    reverseTransition();
                    break;
                }
                switch (selectedMove.Value.Item1.getActionType())
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
                        reverseTransition();
                        break;

                }
                break;

            // Should eventually move this code to GlobalStateTracker.States.PostActionEntitySelect
            case GlobalStateTracker.States.PostActionEntitySelect:
                //KeyValuePair<string, (int, GlobalMoveLists.ActionTypes, Action, int)> move = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                //manabar.value -= (float) move.Value.Item1 / 10;
                reverseTransition();
                break;

            default:
                reverseTransition();
                break;
        }


        Debug.Log(GlobalStateTracker.toString());
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
                    if(GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item1.getActionType() == Move.ActionTypes.SE)
                    {
                        GlobalStateTracker.currentAction = null;
                        transitionsObjects[5].SetActive(true);
                        transitions[5].ReverseTransition();
                    }
                    else if(GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item1.getActionType() == Move.ActionTypes.SA)
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
        Debug.Log(GlobalStateTracker.toString());
    }
}
