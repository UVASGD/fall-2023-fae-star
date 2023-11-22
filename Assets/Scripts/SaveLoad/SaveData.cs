using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveData // holds the variables to be saved
{
    public List<SerializableDictionary<string, (Move, int)>> MoveList; // this is what will be stored in the file

    public SaveData(){ // stores default values
            this.MoveList = new List<SerializableDictionary<string, (Move, int)>>();
    }
}

