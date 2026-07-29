using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameSetupUI : MonoBehaviour
{
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


    void Start()
    {
        LoadDropdowns();
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


        GameProject project =
            studio.currentProject;


        project.gameName =
            gameNameInput.text;


        project.genre =
            (Genre)genreDropdown.value;


        project.theme =
            (Theme)themeDropdown.value;


        project.engine =
            (Engine)engineDropdown.value;


        project.mechanic =
            (Mechanic)mechanicDropdown.value;


        project.platform =
            (Platform)platformDropdown.value;



        Debug.Log(
            "Created Game: " +
            project.gameName
        );


        setupPanel.SetActive(false);

        StartDayOne();

        dayOneUI.ShowDayOne();
    }



    void StartDayOne()
    {
        PublisherManager.Instance.StartDayOne();
    }
}