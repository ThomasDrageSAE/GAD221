using UnityEngine;
using TMPro;

public class DayOneUI : MonoBehaviour
{
    public GameObject dayOnePanel;

    public TMP_Text messageText;
    [Header("HUD")]
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

        if (DayHUD.Instance != null)
        {
            DayHUD.Instance.ShowHUD();
        }
        else
        {
            Debug.LogError("DayHUD.Instance is missing.");
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
            if (!DayManager.Instance.IsDayRunning)
            {
                DayManager.Instance.StartCurrentDay();
            }
        }
        else
        {
            Debug.LogError("DayManager.Instance is missing.");
        }
    }
}