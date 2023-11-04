using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists : IDataPersistence // interface needed to enable saving/loading capacities
{
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (int, Action, int) => (Mana Cost, Action on Select, Position in Selection List)
    public static List<SerializableDictionary<string, (int, Action, int)>> MoveList = new List<SerializableDictionary<string, (int, Action, int)>>
        { //Mage's list comes first
	        new SerializableDictionary<string, (int, Action, int)> 
            {
                {"Club", (0, DoNothing, 11)},
                {"Lesser Heal", (3, DoNothing, 21)},
                {"Heal", (5, DoNothing, 31)},
                {"Lesser Flame", (4, DoNothing, 41)},
                {"Blazing", (6, DoNothing, 51)},
                {"Back", (0, null, 61)},
            },
            new SerializableDictionary<string, (int, Action, int)>
            {
                {"Knife", (0, DoNothing, 11)},
                {"Backstab", (3, DoNothing, 21)},
                {"Back", (0, null, 31)},
            },
            new SerializableDictionary<string, (int, Action, int)>
            {
                {"Poison Touch", (0, DoNothing, 11)},
                {"Malware", (3, DoNothing, 21)},
                {"Back", (0, null, 31)},
            },
            new SerializableDictionary<string, (int, Action, int)>
            {
                {"Slash", (0, DoNothing, 11)},
                {"Raise Shield", (3, DoNothing, 21)},
                {"Back", (0, null, 31)},
            }
        };


    // Temp function that does nothing
    public static void DoNothing() { }

    public void LoadData(SaveData data) {
        MoveList = data.MoveList; 
    }

    public void SaveData(SaveData data) {
        data.MoveList = MoveList; // copies this over to the script that saves all the data
    }
}
