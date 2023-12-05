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
}
