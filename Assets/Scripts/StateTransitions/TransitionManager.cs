using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] GameObject[] characterObjects;
    private static List<GameObject> characters;
    [SerializeField] GameObject[] transitionsObjects;
    private static List<Transition> transitions;

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
    }
    
    public static void processTransition(int selected)
    {
        switch((GlobalStateTracker.battleState, GlobalStateTracker.currentAction))
        {
            case (GlobalStateTracker.States.CharacterSelect, null):
                GlobalStateTracker.battleState = GlobalStateTracker.States.ActionSelect;
                transitions[0].Transition(selected);
                break;

            case (GlobalStateTracker.States.ActionSelect, null):
                if (selected == 0)
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
                else if (selected == 1)
                {
                    //GlobalStateTracker.battleState = GlobalStateTracker.States.ItemMenuing;
                    // TODO Implement Item Menu, for now we will just do nothing
                }
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
        }
        Debug.Log(GlobalStateTracker.toString());
    }
}
