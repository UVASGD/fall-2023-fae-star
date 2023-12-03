using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{ // this file takes care of reading and writing things to a file
    private string FileLocation;  // saves to C:\Users\[user]\AppData\LocalLow\[whatever the company is]\SaveData.dat

    public FileDataHandler(string FileLocation){
        this.FileLocation = FileLocation + "/SaveData.dat";
    }

    public void Save(SaveData data)
    {
        if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            FileLocation = FileLocation.Replace('/', '\\');
        }
        Directory.CreateDirectory(Application.persistentDataPath);
        
        try {
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(FileLocation, FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e){
            Debug.LogError("Could not save data: " + e.ToString());
            Debug.Log(FileLocation);
        }
    }

    public SaveData Load()
    {
        SaveData loadedData = null;
        if (SystemInfo.operatingSystem.Contains("Windows"))
        {
            FileLocation = FileLocation.Replace('/', '\\');
        }
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
