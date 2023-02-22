using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirectoryPath, string _dataFileName)
    {
        this.dataDirectoryPath = _dataDirectoryPath;
        this.dataFileName = _dataFileName;
    }
    
    public ToolData Load()
    {
        //use Path.Combine for different systems
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        ToolData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<ToolData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error! Error occured when trying to save data to file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(ToolData _data)
    {
        //use Path.Combine for different systems
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);

        try
        {
            //create directory if does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the C# data object into JSON
            string dataToStore = JsonUtility.ToJson(_data, true);

            // write Serialized data to file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error! Error occured when trying to save data to file: " + fullPath + "\n" +  e);
        }
    }
}
