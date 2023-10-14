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
                    //GlobalStateTracker.battleState = GlobalStateTracker.States.ItemMenuing;
                    // TODO Implement Item Menu, for now we will just do nothing
                }
                else if (selected == 2) // Guard button is selected
                {

                }
                else // Back button is selected
                {
                    reverseTransition();
                }
                break;

            case GlobalStateTracker.States.ActionMenuing:
                KeyValuePair<string, (int, Move.ActionTypes, Action, int)> selectedMove = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                switch (selectedMove.Value.Item2)
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
                            selections[7].reverseActivation = transitionsObjects[6];
                            transitions[6].Transition(selected);
                        }
                        else if (GlobalStateTracker.currentEntity == characters[1])
                        {
                            selections[7].reverseActivation = transitionsObjects[7];
                            transitions[7].Transition(selected);
                        }
                        else if (GlobalStateTracker.currentEntity == characters[2])
                        {
                            selections[7].reverseActivation = transitionsObjects[8];
                            transitions[8].Transition(selected);
                        }
                        else
                        {
                            selections[7].reverseActivation = transitionsObjects[9];
                            transitions[9].Transition(selected);
                        }

                        break;

                    case Move.ActionTypes.PB:
                    case Move.ActionTypes.ME:
                    case Move.ActionTypes.MA:
                        // Should just Act here as there is no need to select anyone
                        reverseTransition();
                        break;

                    case Move.ActionTypes.none:
                        // Return back to Action Select Menu
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
                transitions[0].ReverseTransition();
                break;

            case GlobalStateTracker.States.ActionMenuing:
                GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                if (GlobalStateTracker.currentEntity == characters[0]) 
                    transitions[1].ReverseTransition();
                else if (GlobalStateTracker.currentEntity == characters[1]) 
                    transitions[2].ReverseTransition();
                else if (GlobalStateTracker.currentEntity == characters[2]) 
                    transitions[3].ReverseTransition();
                else 
                    transitions[4].ReverseTransition();
                break;

            case GlobalStateTracker.States.ItemMenuing:
                GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                // TODO Implement Item Menu
                break;

            case GlobalStateTracker.States.PostActionEntitySelect:
                if (GlobalStateTracker.currentAction != null)
                {
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionMenuing;
                    if(GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item2 == Move.ActionTypes.SE)
                    {
                        GlobalStateTracker.currentAction = null;
                        transitions[5].ReverseTransition();
                    }
                    else if(GlobalMoveLists.MoveList[characterIndex][GlobalStateTracker.currentAction].Item2 == Move.ActionTypes.SA)
                    {
                        if (GlobalStateTracker.currentEntity == characters[0])
                            transitions[6].ReverseTransition();
                        else if (GlobalStateTracker.currentEntity == characters[1])
                            transitions[7].ReverseTransition();
                        else if (GlobalStateTracker.currentEntity == characters[2])
                            transitions[8].ReverseTransition();
                        else
                            transitions[9].ReverseTransition();
                    }
                }
                break;
        }
        Debug.Log(GlobalStateTracker.toString());
    }
}
