using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMoveLists : IDataPersistence // interface needed to enable saving/loading capacities
{
    
    
    // For now I am going to hard code this, but we might want to change it sometime in the future.
    // The setup for this list is (Move, int) => (Move, Position in Selection List)

    public static List<Dictionary<string, (Move, int)>> MoveList = new List<Dictionary<string, (Move, int)>>
        { //Mage's list comes first
	        new Dictionary<string, (Move, int)> 
            {
                {"Club", (new Club(), 11)},
                {"Lesser Heal", (new LesserHeal(), 21)},
                {"Heal", (new Heal(), 31)},
                {"Lesser Flame", (new LesserFlame(), 41)},
                {"Blazing", (new Blazing(), 51)},
                {"Back", (null, 61)}
            },
            new Dictionary<string, (Move, int)>
            {
                {"Knife", (new Knife(), 11)},
                {"Backstab", (new Backstab(), 21)},
                {"Back", (null, 31)}
            },
            new Dictionary<string, (Move, int)>
            {
                {"Poison Touch", (new PoisonTouch(), 11)},
                {"Malware", (new Malware(), 21)},
                {"Back", (null, 31)}
            },
            new Dictionary<string, (Move, int)>
            {
                {"Slash", (new Slash(), 11)},
                {"Raise Shield", (new RaiseShield(), 21)},
                {"Back", (null, 31)}
            }
        };

    public void LoadData(SaveData data) {
        GlobalMoveLists.MoveList.Clear();
        foreach (SerializableList<string> list in data.MoveList)
        {
            Dictionary<string, (Move, int)> nextList = new Dictionary<string, (Move, int)>();
            for(int i = 0; i < list.list.Count; i++)
            {
                nextList[list.list[i]] = (moveDictionary(list.list[i]), (i + 1) * 10 + 1);
            }
            GlobalMoveLists.MoveList.Add(nextList);
        }
    }

    public void SaveData(SaveData data) {
        // copies this over to the script that saves all the data
        data.MoveList = new List<SerializableList<string>>();
        foreach (Dictionary<string, (Move, int)> list in GlobalMoveLists.MoveList)
        {
            data.MoveList.Add(new SerializableList<string>(list.Keys.ToArray()));
        }
    }

    // This is just a one-to-one for all names and moves for linking sakes
    private Move moveDictionary(string moveName)
    {
        Move toReturn = null;
        switch (moveName)
        {
            // Mage moves
            case "Club":
                toReturn =  new Club();
                break;
            case "Lesser Heal":
                toReturn =  new LesserHeal();
                break;
            case "Heal":
                toReturn =  new Heal();
                break;
            case "Lesser Flame":
                toReturn =  new LesserFlame();
                break;
            case "Blazing":
                toReturn =  new Blazing();
                break;

            // Assassin Moves
            case "Knife":
                toReturn =  new Knife();
                break;
            case "Backstab":
                toReturn =  new Backstab();
                break;

            // Hacker Moves
            case "Poison Touch":
                toReturn =  new PoisonTouch();
                break;
            case "Malware":
                toReturn =  new Malware();
                break;

            // Knight Moves
            case "Slash":
                toReturn =  new Slash();
                break;
            case "Raise Shield":
                toReturn =  new RaiseShield();
                break;

            // Generic
            default:
                toReturn =  null;
                break;
        }

        return toReturn;
    }
}
