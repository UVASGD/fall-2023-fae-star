using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{ // this file takes care of reading and writing things to a file
    private string FileLocation = String.Concat(Application.persistentDataPath,"/SaveData.dat");  // saves to C:\Users\[user]\AppData\LocalLow\[whatever the company is]\SaveData.dat

    public FileDataHandler(string FileLocation){
        this.FileLocation = FileLocation;
    }

    public void Save(SaveData data){
        Directory.CreateDirectory(FileLocation);

        try {
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(FileLocation, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        }
        catch {
        Debug.LogError("Could not save data");
        }
    }

    public SaveData Load(){
        SaveData loadedData = null;
        if (File.Exists(FileLocation)){
            try {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(FileLocation, FileMode.Open)){
                    using(StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                } 
                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad); // deserializes
            }
            catch {
                Debug.LogError("Could not save data");

            }
        }
        return loadedData;
    }
}
