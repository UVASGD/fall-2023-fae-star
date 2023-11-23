using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveData // holds the variables to be saved
{
    private readonly List<SerializableList<string>> DefaultMoveLists = new List<SerializableList<string>>
        { //Mage's list comes first
	        new SerializableList<string>(new string[]
            {
                "Club",
                "Lesser Heal",
                "Back"
            }),
            new SerializableList<string>(new string[]
            {
                "Knife",
                "Backstab",
                "Back"
            }),
            new SerializableList<string>(new string[]
            {
                "Poison Touch",
                "Malware",
                "Back"
            }),
            new SerializableList<string>(new string[]
            {
                "Slash",
                "Raise Shield",
                "Back"
            })
        };

    public List<SerializableList<string>> MoveList; // this is what will be stored in the file

    public SaveData() // stores default values
    {
        this.MoveList = DefaultMoveLists;
    }
}

