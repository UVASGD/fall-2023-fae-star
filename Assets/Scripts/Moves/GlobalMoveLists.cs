using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists : IDataPersistence // interface needed to enable saving/loading capacities
{
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (Move, int) => (Move, Position in Selection List)

    public static List<SerializableDictionary<string, (Move, int)>> MoveList = new List<SerializableDictionary<string, (Move, int)>>
        { //Mage's list comes first
	        new SerializableDictionary<string, (Move, int)> 
            {
                {"Club", (new Club(), 11)},
                {"Lesser Heal", (new LesserHeal(), 21)},
                {"Heal", (new Heal(), 31)},
                {"Lesser Flame", (new LesserFlame(), 41)},
                {"Blazing", (new Blazing(), 51)},
                {"Back", (null, 61)}
            },
            new SerializableDictionary<string, (Move, int)>
            {
                {"Knife", (new Knife(), 11)},
                {"Backstab", (new Backstab(), 21)},
                {"Back", (null, 31)}
            },
            new SerializableDictionary<string, (Move, int)>
            {
                {"Poison Touch", (new PoisonTouch(), 11)},
                {"Malware", (new Malware(), 21)},
                {"Back", (null, 31)}
            },
            new SerializableDictionary<string, (Move, int)>
            {
                {"Slash", (new Slash(), 11)},
                {"Raise Shield", (new RaiseShield(), 21)},
                {"Back", (null, 31)}
            }
        };

    public void LoadData(SaveData data) {
        MoveList = data.MoveList; 
    }

    public void SaveData(SaveData data) {
        data.MoveList = MoveList; // copies this over to the script that saves all the data
    }
}
