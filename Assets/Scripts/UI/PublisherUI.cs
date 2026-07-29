using UnityEngine;
using TMPro;


public class PublisherUI : MonoBehaviour
{
    public GameObject publisherPanel;

    public TMP_Text titleText;
    public TMP_Text demandText;
    public TMP_Text effectsText;


    public void ShowDemand()
    {
        Debug.Log("ShowDemand called");
        publisherPanel.SetActive(true);


        DarkPatternData demand =
            PublisherManager.Instance.currentDemandData;


        titleText.text =
            "PUBLISHER REQUEST";


        demandText.text =
            demand.name +
            "\n\n" +
            demand.description;


        effectsText.text =
            "AUDIENCE: " + FormatNumber(demand.audienceModifier) +
            "\nPROFIT: " + FormatNumber(demand.profitModifier) +
            "\nETHICS: " + FormatNumber(demand.ethicsModifier);
    }



    public void Accept()
    {
        publisherPanel.SetActive(false);

        PublisherManager.Instance.AcceptDemand();
    }



    public void Reject()
    {
        publisherPanel.SetActive(false);

        PublisherManager.Instance.RejectDemand();
    }



    string FormatNumber(int number)
    {
        if(number > 0)
            return "+" + number;

        return number.ToString();
    }
}