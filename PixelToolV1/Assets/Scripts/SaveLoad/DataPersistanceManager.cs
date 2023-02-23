using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Configuration")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private ToolData toolData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileID = "test";

    //Singleton
    public static DataPersistanceManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("More than 1 Manager");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);


        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);   
    }


    public void ChangeSelectedProfileID(string _newProfileID)
    {
        this.selectedProfileID = _newProfileID;
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        this.dataPersistenceObjects = FindAllDataPersitenceObjects();
        LoadProject();
        Debug.Log("SceneLoaded");
    }

    public void OnSceneUnloaded(Scene _scene)
    {
        SaveProject();
        Debug.Log("SceneUnloaded");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded   += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded   -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void NewProject()
    {
        this.toolData = new ToolData();
    }

    public void LoadProject()
    {
        this.toolData = dataHandler.Load(selectedProfileID);

        if(this.toolData == null)
        {
            Debug.Log("No Data found, New Project needs to be created first");
            return;
        }

        //set tooldata => current data
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(toolData);
        }
        Debug.Log("loaded:" + toolData.test);
    }

    public void SaveProject()
    {
        if(this.toolData == null)
        {
            Debug.LogWarning("No data found. Start new project first");
            return;
        }

        //Set current data => tooldata
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref toolData);
        }
        Debug.Log("Saved:" + toolData.test);


        dataHandler.Save(toolData, selectedProfileID);
    }

    private void OnApplicationQuit()
    {
        SaveProject();
    }


    //find all that use Interface and store in list
    private List<IDataPersistence> FindAllDataPersitenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects  = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasToolData()
    {
        return toolData != null;
    }


    public Dictionary<string, ToolData> GetAllProfilesToolData()
    {

        return dataHandler.LoadAllProfiles();
    }
}
