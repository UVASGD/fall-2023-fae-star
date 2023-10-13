using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class SaveData // holds the variables to be saved
{
    public string MoveList1, MoveList2, MoveList3, MoveList4; // this is what will be stored in the file
}

public class PlayerData : MonoBehaviour
{

    string mageMoveList, knifeMoveList, hackerMoveList, knightMoveList; // the lists being used for the current playthrough

    private string FileLocation;

    public void Awake(){
        FileLocation = String.Concat(Application.persistentDataPath,"/SaveData.dat");
        //mageMoveList = GlobalMoveLists.MoveList; gonna have to figure out how to do this lol
    }

    public void SaveGame(){
    
        BinaryFormatter format = new BinaryFormatter();
        FileStream data = File.Create(FileLocation); // saves to C:\Users\[user]\AppData\LocalLow\[whatever the company is]\SaveData.dat
        SaveData s = new SaveData();
        s.MoveList1 = mageMoveList;
        format.Serialize(data, s); // encrypts the data so it can't be read as text
        data.Close();
        Debug.Log("Data saved");
    }

    public void LoadGame(){ // overrides the variables at the top of the script so they can be read by other scripts

        if(File.Exists(FileLocation))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream data = File.Open(FileLocation, FileMode.Open);
            SaveData s = (SaveData)format.Deserialize(data); // reads the data back and turns it into type SaveData
            data.Close();
            mageMoveList = s.MoveList1; // overrides the variable used in the current playthrough
            Debug.Log("Data loaded");
        }
        else
        Debug.Log("Save data not found");
    }

    void ClearData(){

        if(File.Exists(FileLocation)){
            File.Delete(FileLocation); // file is gone
            mageMoveList = ""; // now to reset all the variables so they don't get put back if the player tries to save
            // MAKE SURE TO RESET THIS TO THE DEFAULT VALUES, NOT BLANK
            Debug.Log("File deleted");
        }
        else
            Debug.Log("No file to delete");
    }
}
