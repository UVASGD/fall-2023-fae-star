using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists : IDataPersistence // interface needed to enable saving/loading capacities
{
    
    
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (Move, int) => (Move, Position in Selection List)

    public static List<Dictionary<string, (Move.ActionTypes, int, int)>> MoveList = new List<Dictionary<string, (Move.ActionTypes, int, int)>>
        { //Mage's list comes first
	        new Dictionary<string, (Move.ActionTypes, int, int)> 
            {
                {"Club", (GlobalMoveDictionary.MoveActionTypes["Club"], GlobalMoveDictionary.MoveManaCosts["Club"], 11)},
                {"Lesser Heal", (GlobalMoveDictionary.MoveActionTypes["Lesser Heal"], GlobalMoveDictionary.MoveManaCosts["Lesser Heal"], 21)},
                {"Heal", (GlobalMoveDictionary.MoveActionTypes["Heal"], GlobalMoveDictionary.MoveManaCosts["Heal"], 31)},
                {"Lesser Flame", (GlobalMoveDictionary.MoveActionTypes["Lesser Flame"], GlobalMoveDictionary.MoveManaCosts["Lesser Flame"], 41)},
                {"Blazing", (GlobalMoveDictionary.MoveActionTypes["Blazing"], GlobalMoveDictionary.MoveManaCosts["Blazing"], 51)},
                {"Back", (Move.ActionTypes.NA, 0, 61)}
            },
            new Dictionary<string, (Move.ActionTypes, int, int)>
            {
                {"Knife", (GlobalMoveDictionary.MoveActionTypes["Knife"], GlobalMoveDictionary.MoveManaCosts["Knife"], 11)},
                {"Backstab", (GlobalMoveDictionary.MoveActionTypes["Backstab"], GlobalMoveDictionary.MoveManaCosts["Backstab"], 21)},
                {"Back", (Move.ActionTypes.NA, 0, 31)}
            },
            new Dictionary<string, (Move.ActionTypes, int, int)>
            {
                {"Poison Touch", (GlobalMoveDictionary.MoveActionTypes["Poison Touch"], GlobalMoveDictionary.MoveManaCosts["Poison Touch"], 11)},
                {"Malware", (GlobalMoveDictionary.MoveActionTypes["Malware"], GlobalMoveDictionary.MoveManaCosts["Malware"], 21)},
                {"Back", (Move.ActionTypes.NA, 0, 31)}
            },
            new Dictionary<string, (Move.ActionTypes, int, int)>
            {
                {"Slash", (GlobalMoveDictionary.MoveActionTypes["Slash"], GlobalMoveDictionary.MoveManaCosts["Slash"], 11)},
                {"Raise Shield", (GlobalMoveDictionary.MoveActionTypes["Raise Shield"], GlobalMoveDictionary.MoveManaCosts["Raise Shield"], 21)},
                {"Back", (Move.ActionTypes.NA, 0, 31)}
            }
        };

    public void LoadData(SaveData data) {
        GlobalMoveLists.MoveList.Clear();
        foreach (SerializableList<string> list in data.MoveList)
        {
            Dictionary<string, (Move.ActionTypes, int, int)> nextList = new Dictionary<string, (Move.ActionTypes, int, int)>();
            for(int i = 0; i < list.list.Count; i++)
            {
                if (list.list[i] != "Back")
                    nextList[list.list[i]] = (GlobalMoveDictionary.MoveActionTypes[list.list[i]], GlobalMoveDictionary.MoveManaCosts[list.list[i]], (i + 1) * 10 + 1);
                else
                    nextList[list.list[i]] = (Move.ActionTypes.NA, 0, (i + 1) * 10 + 1);
        }
            GlobalMoveLists.MoveList.Add(nextList);
        }
    }

    public void SaveData(SaveData data) {
        // copies this over to the script that saves all the data
        data.MoveList = new List<SerializableList<string>>();
        foreach (Dictionary<string, (Move.ActionTypes, int, int)> list in GlobalMoveLists.MoveList)
        {
            data.MoveList.Add(new SerializableList<string>(list.Keys.ToArray()));
        }
    }
}
