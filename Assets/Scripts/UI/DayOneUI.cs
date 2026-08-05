using UnityEngine;
using TMPro;

public class DayOneUI : MonoBehaviour
{
    public GameObject dayOnePanel;

    public TMP_Text messageText;
    [Header("HUD")]
    [SerializeField] private GameObject dayHUD;
    [SerializeField] private GameObject officeHUD;


    public void ShowDayOne()
    {
        dayOnePanel.SetActive(true);

        messageText.text =
            "Congratulations!\n\n" +
            "Your studio has been founded.\n\n" +
            "Your first game is now in development.";
    }


    public void Continue()
    {
        Debug.Log("Continue button pressed");

        dayOnePanel.SetActive(false);

        if (dayHUD != null)
        {
            dayHUD.SetActive(true);
        }
        else
        {
            Debug.LogError("DayHUD has not been assigned.");
        }

        if (officeHUD != null)
        {
            officeHUD.SetActive(true);
        }
        else
        {
            Debug.LogError("OfficeHUD has not been assigned.");
        }

        if (DayManager.Instance != null)
        {
            DayManager.Instance.StartCurrentDay();
        }
        else
        {
            Debug.LogError("DayManager.Instance is missing.");
        }
    }
}