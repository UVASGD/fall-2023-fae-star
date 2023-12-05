using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultData
{
    public static readonly List<SerializableList<string>> DefaultMoveLists = new List<SerializableList<string>>
        { //Mage's list comes first
	        new SerializableList<string>(new string[]
            {
                "Club",
                //"Lesser Heal",
                "Back"
            }),
            new SerializableList<string>(new string[]
            {
                "Knife",
                //"Backstab",
                "Back"
            }),
            new SerializableList<string>(new string[]
            {
                "Poison Touch",
                //"Malware",
                "Back"
            }),
            new SerializableList<string>(new string[]
            {
                "Slash",
                //"Raise Shield",
                "Back"
            })
        };

    public static readonly List<Character> DefaultCharacters = new List<Character>
        {
            new Character("Leoht", 20, 25, 6, 8, 4, 6, 9, 0, 5, 0),
            new Character("Assassin", 22, 15, 9, 3, 8, 5, 4, 0, 5, 0),
            new Character("Hacker", 27, 30, 3, 8, 12, 5, 6, 0, 5, 0),
            new Character("Knight", 35, 10, 9, 4, 5, 12, 5, 0, 5, 0),
        };

    public static readonly string DefaultScene = "SC1";
}
