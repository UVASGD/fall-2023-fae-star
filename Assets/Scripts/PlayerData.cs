using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour
{

    int intToSave; // the general variable that it should be able to access, later this will be replaced with actual code
    private string FileLocation;

    public void Awake(){
        FileLocation = String.Concat(Application.persistentDataPath,"/SaveData.dat");
    }

    public void SaveGame(){
    
        BinaryFormatter format = new BinaryFormatter();
        FileStream data = File.Create(FileLocation); // saves to C:\Users\[user]\AppData\LocalLow\[whatever the company is]\SaveData.dat
        SaveData s = new SaveData();
        s.SavedInt = intToSave;
        format.Serialize(data, s); // encrypts the data so it can't be read as text
        data.Close();
        Debug.Log("Data saved");
    }

    public void LoadGame(){

        if(File.Exists(FileLocation))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream data = File.Open(FileLocation, FileMode.Open);
            SaveData s = (SaveData)format.Deserialize(data); // reads the data back and turns it into type SaveData
            data.Close();
            intToSave = s.SavedInt; // overrides the variable used in the current playthrough
            Debug.Log("Data loaded");
        }
        else
        Debug.Log("Save data not found");
    }

    void ClearData(){

        if(File.Exists(FileLocation)){
            File.Delete(FileLocation); // file is gone
            intToSave = 0; // now to reset all the variables so they don't get put back if the player tries to save
            Debug.Log("File deleted");
        }
        else
            Debug.Log("No file to delete");
    }

    [Serializable]
public class SaveData // holds the variables to be saved
{
    public int SavedInt; // placeholder to save the example int, should save individual move lists instead. this is what will be used in game
}
}
