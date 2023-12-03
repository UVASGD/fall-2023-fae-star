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
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        this.dataPersistenceObjects.Add(new GlobalMoveLists());
        this.saveData = new SaveData();
    }

    public void NewGame() 
    {
        this.saveData = new SaveData();
        dataHandler.Save(saveData);
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
        SceneManager.LoadScene("BattleScene");
    }

    public void SaveGame()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(saveData);
        }

        // save that data to a file using the data handler
        dataHandler.Save(saveData);
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> persistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(persistenceObjects);
    }
}