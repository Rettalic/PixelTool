using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private ToolData toolData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    //Singleton
    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("More than 1 Manager");
        }
        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);   
        this.dataPersistenceObjects = FindAllDataPersitenceObjects();
        LoadProject();    
    }

    public void NewProject()
    {
        this.toolData = new ToolData();
    }

    public void LoadProject()
    {
        this.toolData = dataHandler.Load();

        if(this.toolData == null)
        {
            Debug.Log("No Data, set to defaults");
            NewProject();
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
        //Set current data => tooldata
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref toolData);
        }
        Debug.Log("Saved:" + toolData.test);


        dataHandler.Save(toolData);
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
}
