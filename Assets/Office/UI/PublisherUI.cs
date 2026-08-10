using UnityEngine;
using TMPro;

public class PublisherUI : MonoBehaviour
{
    [Header("Old Publisher Popup")]
    [SerializeField] private GameObject publisherPanel;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text demandText;
    [SerializeField] private TMP_Text effectsText;


    public void ShowDemand()
    {
        if (publisherPanel == null)
            return;

        if (PublisherManager.Instance == null)
            return;

        DarkPatternData demand =
            PublisherManager.Instance.currentDemandData;

        if (demand == null)
            return;

        publisherPanel.SetActive(true);

        titleText.text =
            "PUBLISHER REQUEST";

        demandText.text =
            demand.name +
            "\n\n" +
            demand.description;

        effectsText.text =
            "GAMEPLAY: " +
            FormatNumber(demand.gameplayModifier) +

            "\nSTORY: " +
            FormatNumber(demand.storyModifier) +

            "\nSTYLE: " +
            FormatNumber(demand.styleModifier) +

            "\nAUDIENCE: " +
            FormatNumber(demand.audienceModifier) +

            "\nPROFIT: " +
            FormatNumber(demand.profitModifier) +

            "\nETHICS: " +
            FormatNumber(demand.ethicsModifier);
    }


    public void HideDemand()
    {
        if (publisherPanel != null)
        {
            publisherPanel.SetActive(false);
        }
    }


    public void Accept()
    {
        if (PublisherManager.Instance == null)
            return;

        PublisherManager.Instance.AcceptDemand();

        HideDemand();
    }


    public void Reject()
    {
        if (PublisherManager.Instance == null)
            return;

        PublisherManager.Instance.RejectDemand();

        HideDemand();
    }


    private string FormatNumber(int number)
    {
        if (number > 0)
            return "+" + number;

        return number.ToString();
    }
}