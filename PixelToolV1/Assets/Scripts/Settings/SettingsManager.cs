using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour, IDataPersistence
{

    public int pixelateAmount;
    public int test;

    public void LoadData(ToolData data)
    {
        this.pixelateAmount = data.pixelateAmount;
        this.test = data.test;
    }

    public void SaveData(ref ToolData data)
    {
        data.pixelateAmount = this.pixelateAmount;
        data.test = this.test;
    }
}
