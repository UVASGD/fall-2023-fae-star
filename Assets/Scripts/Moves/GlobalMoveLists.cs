using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists
{
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    public static List<Dictionary<string, (int, Action)>> MoveList = new List<Dictionary<string, (int, Action)>>
        { //Mage's list comes first
	        new Dictionary<string, (int, Action)> 
            {
                {"Club", (0, DoNothing)},
                {"Lesser Heal", (3, DoNothing)},
                {"Heal", (5, DoNothing)},
                {"Lesser Flame", (4, DoNothing)},
                {"Blazing", (6, DoNothing)},
                {"Back", (-1, null)},
            },
            new Dictionary<string, (int, Action)>
            {
                {"Club", (0, DoNothing)},
                {"Lesser Heal", (3, DoNothing)},
                {"Heal", (5, DoNothing)},
                {"Lesser Flame", (4, DoNothing)},
                {"Blazing", (6, DoNothing)},
                {"Back", (-1, null)},
            },
            new Dictionary<string, (int, Action)>
            {
                {"Club", (0, DoNothing)},
                {"Lesser Heal", (3, DoNothing)},
                {"Heal", (5, DoNothing)},
                {"Lesser Flame", (4, DoNothing)},
                {"Blazing", (6, DoNothing)},
                {"Back", (-1, null)},
            },
            new Dictionary<string, (int, Action)>
            {
                {"Club", (0, DoNothing)},
                {"Lesser Heal", (3, DoNothing)},
                {"Heal", (5, DoNothing)},
                {"Lesser Flame", (4, DoNothing)},
                {"Blazing", (6, DoNothing)},
                {"Back", (-1, null)},
            }
        };


    // Temp function that does nothing
    public static void DoNothing() { }
}
