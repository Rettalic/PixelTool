using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "ToolsProject";

    public FileDataHandler(string _dataDirectoryPath, string _dataFileName, bool _useEncryption)
    {
        this.dataDirectoryPath = _dataDirectoryPath;
        this.dataFileName = _dataFileName;
        this.useEncryption = _useEncryption;    
    }
    
    public ToolData Load(string _profileID)
    {
        //use Path.Combine for different systems
        string fullPath = Path.Combine(dataDirectoryPath, _profileID, dataFileName);

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

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
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

    public void Save(ToolData _data, string _profileID)
    {
        //use Path.Combine for different systems
        string fullPath = Path.Combine(dataDirectoryPath, _profileID, dataFileName);
        try
        {
            //create directory if does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the C# data object into JSON
            string dataToStore = JsonUtility.ToJson(_data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

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
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" +  e);
        }
    }

    public Dictionary<string, ToolData> LoadAllProfiles()
    {
        Dictionary<string, ToolData> profileDictionary = new Dictionary<string, ToolData>();

        IEnumerable<DirectoryInfo> directoryInfos =  new DirectoryInfo(dataDirectoryPath).EnumerateDirectories();

        foreach(DirectoryInfo directoryInfo in directoryInfos)
        {
            string profileID = directoryInfo.Name;

            string fullPath = Path.Combine(dataDirectoryPath, profileID, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skip directory, does not contain data: " + profileID);
                continue;
            }
            
            ToolData profileData = Load(profileID);

            if(profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile, FAILED. ID:" + profileID);
            }
        }

        return profileDictionary;
    }


    //XOR encryption (simple version)
    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}
