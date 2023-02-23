using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotMenu : MonoBehaviour
{

    [SerializeField] private MainMenu menu;


    private SaveSlot[] saveSlots;
    public int sceneIndex;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

 

    public void OnBackClicked()
    {
        menu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnSaveSlotClicked(SaveSlot _saveSlot)
    {
        DataPersistanceManager.Instance.ChangeSelectedProfileID(_saveSlot.GetProfileID());

        DataPersistanceManager.Instance.NewProject();

        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        Dictionary<string, ToolData> profilesToolData = DataPersistanceManager.Instance.GetAllProfilesToolData();

        foreach(SaveSlot saveSlot in saveSlots)
        {
            ToolData profileData = null;
            profilesToolData.TryGetValue(saveSlot.GetProfileID(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
