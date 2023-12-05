using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private SaveData saveData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() 
    {
        if (instance != null) 
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene.");
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath);
        // Fuck it we hard code this shit
        this.dataPersistenceObjects = new List<IDataPersistence>();
        this.dataPersistenceObjects.Add(new GlobalMoveLists());
        this.dataPersistenceObjects.Add(new GlobalEntityStats());
        this.saveData = new SaveData();
    }

    public void NewGame() 
    {
        this.saveData = new SaveData();
        this.saveData.Characters[0] = new Character("Leoht", 20, 25, 6, 8, 4, 6, 9, 0, 5, 0);
        this.saveData.Characters[1] = new Character("Assassin", 22, 15, 9, 3, 8, 5, 4, 0, 5, 0);
        this.saveData.Characters[2] = new Character("Hacker", 27, 30, 3, 8, 12, 5, 6, 0, 5, 0);
        this.saveData.Characters[3] = new Character("Knight", 35, 10, 9, 4, 5, 12, 5, 0, 5, 0);
        dataHandler.Save(new SaveData());
        LoadGame();
    }

    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.saveData = dataHandler.Load();
        
        // if no data can be loaded, initialize to a new game
        if (this.saveData == null) 
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            this.saveData = new SaveData();
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(saveData);
        }
        DialogueScript.initialScriptName = this.saveData.CurrentScene;
        DialogueScript.nextScene = this.saveData.CurrentFight;
        SceneManager.LoadScene("DialogueScene");
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(this.saveData);
        }

        // save that data to a file using the data handler
        dataHandler.Save(this.saveData);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    public SaveData getSaveData()
    {
        return saveData;
    }
}