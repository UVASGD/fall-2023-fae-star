using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStateTracker
{
    public enum States
    {
        enterState, // This state represents the introduction to the battle, before control is given to the player
        CharacterSelect, // This state represents the state when choosing a character to act with
        ActionSelect, // This state represents the state when the four buttons Action, Item, Guard, and Back are on the screen
        ActionMenuing, // This state represents the state when clicked into the Action button
        ItemMenuing, // This state represents the state when clicked into the Item button
        PostActionEntitySelect, // This state represents the selctions state once a player has chosen an action or item
        EnemyActions // This state represents the enemy's turn
    }

    public static States battleState = States.CharacterSelect;

    public static GameObject currentEntity = null;

    public static string currentAction = null;

    public static string currentItem = null;

    public static string toString()
    {
        return $"State: {battleState}\t" + (currentEntity != null ? $"Current Entity: {currentEntity}\t" : "") + (currentAction != null ? $"CurrentAction: {currentAction}" : "");
    }
}
