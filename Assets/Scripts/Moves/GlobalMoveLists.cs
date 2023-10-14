using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists
{
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (int, Action, int) => (Mana Cost, ActionType, Action on Select, Position in Selection List)
    /*
     * Action Type codex
     * SE = Single Target Enemy Focused Action
     * ME = AOE Target Enemy Focused Action
     * SA = Single Target Ally Focused Action
     * MA = AOE Target Ally Focused Action
     * PB = Personal Buff
     */

    public enum ActionTypes
    {
        SE,
        ME,
        SA,
        MA,
        PB,
        none,
    }
    
    public static List<Dictionary<string, (int, ActionTypes, Action, int)>> MoveList = new List<Dictionary<string, (int, ActionTypes, Action, int)>>
        { //Mage's list comes first
	        new Dictionary<string, (int, ActionTypes, Action, int)> 
            {
                {"Club", (0, ActionTypes.SE, DoNothing, 11)},
                {"Lesser Heal", (3, ActionTypes.SA, DoNothing, 21)},
                {"Heal", (5, ActionTypes.SA, DoNothing, 31)},
                {"Lesser Flame", (4, ActionTypes.SE, DoNothing, 41)},
                {"Blazing", (6, ActionTypes.PB, DoNothing, 51)},
                {"Back", (0, ActionTypes.none, null, 61)}
            },
            new Dictionary<string, (int, ActionTypes, Action, int)>
            {
                {"Knife", (0, ActionTypes.SE, DoNothing, 11)},
                {"Backstab", (3, ActionTypes.SE, DoNothing, 21)},
                {"Back", (0, ActionTypes.none, null, 31)}
            },
            new Dictionary<string, (int, ActionTypes, Action, int)>
            {
                {"Poison Touch", (0, ActionTypes.SE, DoNothing, 11)},
                {"Malware", (3, ActionTypes.SE, DoNothing, 21)},
                {"Back", (0, ActionTypes.none, null, 31)}
            },
            new Dictionary<string, (int, ActionTypes, Action, int)>
            {
                {"Slash", (0, ActionTypes.SE, DoNothing, 11)},
                {"Raise Shield", (3, ActionTypes.PB, DoNothing, 21)},
                {"Back", (0, ActionTypes.none, null, 31)}
            }
        };


    // Temp function that does nothing
    public static void DoNothing() { }
}
