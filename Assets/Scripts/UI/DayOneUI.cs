using UnityEngine;
using TMPro;

public class DayOneUI : MonoBehaviour
{
    public GameObject dayOnePanel;

    public TMP_Text messageText;


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

        PublisherManager.Instance.StartPublisherDay();
    }
}