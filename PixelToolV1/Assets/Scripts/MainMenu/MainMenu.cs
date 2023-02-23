using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newProjectButton;
    [SerializeField] private Button loadProjectButton;

    public SaveSlotMenu saveslotMenu;


    public int sceneIndex;

    private void Start()
    {
        if(!DataPersistanceManager.Instance.HasToolData())
        {
            loadProjectButton.interactable = false;
        }    
    }

    public void OnNewProjectClicked()
    {
        saveslotMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnContinueProjectClicked()
    {
        DisableMenuButtons();
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    private void DisableMenuButtons()
    {
        newProjectButton.interactable = false;
        loadProjectButton.interactable= false;
    }


    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        Dictionary<string, ToolData> profilesToolData = DataPersistanceManager.Instance.GetAllProfilesToolData();
        
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
