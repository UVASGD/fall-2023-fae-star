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
    public List<SerializableList<string>> MoveList; // this is what will be stored in the file
    public List<Character> Characters;
    public string CurrentScene;
    public string CurrentFight;

    public SaveData() // stores default values
    {
        this.MoveList = DefaultData.DefaultMoveLists;
        this.Characters = DefaultData.DefaultCharacters;
        this.CurrentScene = DefaultData.DefaultScene;
        this.CurrentFight = DefaultData.DefaultFight;
    }
}

