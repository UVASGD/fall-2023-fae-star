using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists
{
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (int, Action, int) => (Mana Cost, Action on Select, Position in Selection List)
    public static List<Dictionary<string, (int, Action, int)>> MoveList = new List<Dictionary<string, (int, Action, int)>>
        { //Mage's list comes first
	        new Dictionary<string, (int, Action, int)> 
            {
                {"Club", (0, DoNothing, 11)},
                {"Lesser Heal", (3, DoNothing, 21)},
                {"Heal", (5, DoNothing, 31)},
                {"Lesser Flame", (4, DoNothing, 41)},
                {"Blazing", (6, DoNothing, 51)},
                {"Back", (0, null, 61)},
            },
            new Dictionary<string, (int, Action, int)>
            {
                {"Knife", (0, DoNothing, 11)},
                {"Backstab", (3, DoNothing, 21)},
                {"Back", (0, null, 31)},
            },
            new Dictionary<string, (int, Action, int)>
            {
                {"Poison Touch", (0, DoNothing, 11)},
                {"Malware", (3, DoNothing, 21)},
                {"Back", (0, null, 31)},
            },
            new Dictionary<string, (int, Action, int)>
            {
                {"Slash", (0, DoNothing, 11)},
                {"Raise Shield", (3, DoNothing, 21)},
                {"Back", (0, null, 31)},
            }
        };


    // Temp function that does nothing
    public static void DoNothing() { }
}
