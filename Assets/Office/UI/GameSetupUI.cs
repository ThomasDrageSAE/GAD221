using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameSetupUI : MonoBehaviour{
    [Header("Inputs")]
    public TMP_InputField studioNameInput;
    public TMP_InputField gameNameInput;


    [Header("Dropdowns")]
    public TMP_Dropdown genreDropdown;
    public TMP_Dropdown themeDropdown;
    public TMP_Dropdown engineDropdown;
    public TMP_Dropdown mechanicDropdown;
    public TMP_Dropdown platformDropdown;


    [Header("Panels")]
    public GameObject setupPanel;
    public DayOneUI dayOneUI;
    [Header("UI")]
    public GameObject mainCanvas;

    [Header("Office HUD")]
    [SerializeField] private GameObject officeHUDObject;
    [SerializeField] private OfficeHUD officeHUD;
    

    private void Start()
    {
        LoadDropdowns();

        RestoreUIState();
    }


    void LoadDropdowns()
    {
        genreDropdown.ClearOptions();

        genreDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Genre))
            ));


        themeDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Theme))
            ));


        engineDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Engine))
            ));


        mechanicDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Mechanic))
            ));


        platformDropdown.AddOptions(
            new System.Collections.Generic.List<string>(
                System.Enum.GetNames(typeof(Platform))
            ));
    }
    
    public void CreateGame()
    {
        StudioManager studio = StudioManager.Instance;


        studio.studioName =
            studioNameInput.text;


        studio.CreateNewProject();


        GameProject project = studio.currentProject;
        project.gameName = gameNameInput.text;
        project.genre = (Genre)genreDropdown.value;
        project.theme = (Theme)themeDropdown.value;
        project.engine = (Engine)engineDropdown.value;
        project.mechanic = (Mechanic)mechanicDropdown.value;
        project.platform = (Platform)platformDropdown.value;
        
        GameCalculator.Calculate(project);
        
        studio.setupComplete = true;

        Debug.Log("Created Game: " + project.gameName
        );


        if (officeHUDObject != null)
        {
            officeHUDObject.SetActive(true);
        }

        if (officeHUD != null)
        {
            officeHUD.Refresh();
        }
        
        setupPanel.SetActive(false);

        StartDayOne();

        dayOneUI.ShowDayOne();
        
        
    }



    void StartDayOne()
    {
        PublisherManager.Instance.StudioFounded();
    }
    
    private void RestoreUIState()
    {
        if (StudioManager.Instance == null)
        {
            Debug.LogError("GameSetupUI could not find StudioManager.");
            return;
        }

        // First time entering the Office.
        if (!StudioManager.Instance.setupComplete)
        {
            // New game.
            // The Title Screen decides when the setup panel becomes visible.
            setupPanel.SetActive(false);

            if (officeHUDObject != null)
            {
                officeHUDObject.SetActive(false);
            }

            return;
        }

        // Returning to the Office after setup has already been completed.
        setupPanel.SetActive(false);

        if (officeHUDObject != null)
        {
            officeHUDObject.SetActive(true);
        }

        if (officeHUD != null)
        {
            officeHUD.Refresh();
        }

        Debug.Log("Office UI restored.");
    }
}