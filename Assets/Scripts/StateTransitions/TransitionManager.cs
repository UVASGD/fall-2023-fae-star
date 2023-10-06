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
     * 0. Character Select -> Action Select
     * 1-4: Action Select -> Respective Character Actions
     * 5: ????
     */
    [SerializeField] GameObject[] transitionsObjects;
    private static List<Transition> transitions;
    // change later to use global storage
    [SerializeField] Slider manabarObject;
    private static Slider manabar;

    private static int characterIndex;

    void Awake()
    {
        transitions = new List<Transition>();
        for (int i = 0; i < transitionsObjects.Length; i++)
        {
            transitions.Add(transitionsObjects[i].GetComponent(typeof(Transition)) as Transition);
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
                GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                characterIndex = selected;
                transitions[0].Transition(selected);
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
                KeyValuePair<string, (int, GlobalMoveLists.ActionTypes, Action, int)> selectedMove = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                switch (selectedMove.Value.Item2)
                {
                    case GlobalMoveLists.ActionTypes.SE: // Single target enemy action selected
                        GlobalStateTracker.battleState = GlobalStateTracker.States.PostActionEntitySelect;
                        GlobalStateTracker.currentAction = selectedMove.Key;
                        transitions[5].Transition(selected);

                        break;

                    case GlobalMoveLists.ActionTypes.SA: // Single target ally action selected
                        GlobalStateTracker.battleState = GlobalStateTracker.States.PostActionEntitySelect;
                        GlobalStateTracker.currentAction = selectedMove.Key;

                        break;

                    case GlobalMoveLists.ActionTypes.PB:
                    case GlobalMoveLists.ActionTypes.ME:
                    case GlobalMoveLists.ActionTypes.MA:
                        // Should just Act here as there is no need to select anyone
                        reverseTransition();
                        break;

                    case GlobalMoveLists.ActionTypes.none:
                        // Return back to Action Select Menu
                        reverseTransition();
                        break;

                }
                break;

            // Should eventually move this code to GlobalStateTracker.States.PostActionEntitySelect
            case GlobalStateTracker.States.PostActionEntitySelect:
                KeyValuePair<string, (int, GlobalMoveLists.ActionTypes, Action, int)> move = GlobalMoveLists.MoveList[characterIndex].ElementAt(selected);
                manabar.value -= (float) move.Value.Item1 / 10;
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
                if(GlobalStateTracker.currentAction != null)
                {
                    GlobalStateTracker.battleState = GlobalStateTracker.States.ActionMenuing;
                    GlobalStateTracker.currentAction = null;
                    transitions[5].ReverseTransition();
                }
                break;
        }
        Debug.Log(GlobalStateTracker.toString());
    }
}
