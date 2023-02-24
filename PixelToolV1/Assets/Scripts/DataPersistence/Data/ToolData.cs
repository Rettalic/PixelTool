using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolData
{
    public long lastUpdated;
    public int deathCount;
    public Vector3 playerPosition;
       
    public string projectName;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public ToolData() 
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        projectName = "";
    }

    public string GetProjectName() 
    {
        return projectName;
    }
}
