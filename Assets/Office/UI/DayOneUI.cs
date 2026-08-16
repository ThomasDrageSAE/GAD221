using UnityEngine;
using TMPro;

public class DayOneUI : MonoBehaviour
{
    public GameObject dayOnePanel;

    public TMP_Text messageText;
    [Header("HUD")]
  //  [SerializeField] private GameObject officeHUD;
    [SerializeField] private DayHUD dayHUD;

    public void ShowDayOne()
    {
        dayOnePanel.SetActive(true);

        messageText.text =
            "Congratulations!\n\n" +
            "Your studio has been founded.\n\n" +
            "Your first game is now in development.\n\n" +
            "Click the PC to start!";
    }


    public void Continue()
    {
        Debug.Log("Continue button pressed");

        dayOnePanel.SetActive(false);

        if (dayHUD != null)
        {
            dayHUD.ShowHUD();
        }

      /*  if (officeHUD != null)
        {
            officeHUD.SetActive(true);
        }
       else
        {
           Debug.LogError("OfficeHUD has not been assigned.");
        }

      */  DayManager.Instance.StartDay();
    }
}