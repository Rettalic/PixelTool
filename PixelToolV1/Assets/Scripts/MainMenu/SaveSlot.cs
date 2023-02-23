using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SaveSlot : MonoBehaviour
{

    [Header("Profile")]
    [SerializeField] private string profileID = "";

    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    [SerializeField] private Button clearButton;
    [SerializeField] private TextMeshProUGUI projectName;

    public bool hasData { get; private set; } = false;
    private Button saveSlotButton;

    private void Awake()
    {
        
    }

    public void SetData(ToolData _data)
    {
        if(_data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            projectName.text = _data.GetProjectName();
        }
    }



    public string GetProfileID()
    {
        return this.profileID;
    }
}
