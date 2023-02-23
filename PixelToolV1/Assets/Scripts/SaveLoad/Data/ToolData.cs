using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToolData
{
    public int pixelateAmount;
    public int test;
    public string projectName;

    public ToolData()
    {
        this.pixelateAmount = 0;
    }

    public string GetProjectName()
    {
        

        return projectName;
    }
}
