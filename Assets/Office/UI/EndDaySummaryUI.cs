using System;
using TMPro;
using UnityEngine;

public class EndDaySummaryUI : MonoBehaviour
{
    [SerializeField] private GameObject summaryPanel;

    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text studioChangesText;
    [SerializeField] private TMP_Text projectChangesText;
    [SerializeField] private TMP_Text decisionText;

    public static Action OnContinuePress;
    
    private void Awake()
    {
        if (summaryPanel!= null)
        {
            summaryPanel.SetActive(false);
        }

        DayManager.OnDayEnded += ShowSummary;
    }

    private void OnDestroy()
    {
        DayManager.OnDayEnded -= ShowSummary;
    }

    private void Start()
    {
        Debug.Log("EndDaySummaryUI - Start");   
        
        if (DayManager.Instance.HasPendingDaySummary)
        {
            ShowSummary();
        }
    }

    public void ShowSummary(int day)
    {
        ShowSummary();
    }
    
    public void ShowSummary()
    {
        Debug.Log("EndDaySummaryUI - ShowSummary");
        
        if (DayManager.Instance == null)
        {
            Debug.LogError(
                "EndDaySummaryUI could not find DayManager.");

            return;
        }

        DayManager dayManager =
            DayManager.Instance;

        dayText.text =
            "END OF DAY " +
            dayManager.SummaryDay;

        studioChangesText.text =
            "Budget          " +
            FormatMoney(
                dayManager.SummaryBudgetChange) +

            "\nPublisher Trust " +
            FormatNumber(
                dayManager.SummaryTrustChange) +

            "\nEthics          " +
            FormatNumber(
                dayManager.SummaryEthicsChange);

        projectChangesText.text =
            "Gameplay   " +
            FormatNumber(
                dayManager.SummaryGameplayChange) +

            "\nStory      " +
            FormatNumber(
                dayManager.SummaryStoryChange) +

            "\nStyle      " +
            FormatNumber(
                dayManager.SummaryStyleChange) +

            "\nAudience   " +
            FormatNumber(
                dayManager.SummaryAudienceChange) +

            "\nProfit     " +
            FormatNumber(
                dayManager.SummaryProfitChange);

        UpdateDecisionText();

        if (summaryPanel != null)
        {
            summaryPanel.SetActive(true);
        }
    }

    private void UpdateDecisionText()
    {
        if (DayManager.Instance.SummaryDay == 1)
        {
            decisionText.text =
                "DECISION\n" +
                "No publisher request today.";

            return;
        }

        if (PublisherManager.Instance == null ||
            PublisherManager.Instance.currentDemandData == null)
        {
            decisionText.text =
                "DECISION\n" +
                "No publisher request.";

            return;
        }

        decisionText.text =
            "DECISION\n" +
            PublisherManager.Instance.currentDemandData.name +
            "\n" +
            PublisherManager.Instance.LastDecisionText;
    }

    public void Continue()
    {
        Debug.Log("SUMMARY CONTINUE CLICKED");

        if (summaryPanel != null)
        {
            summaryPanel.SetActive(false);
        }
        
        DayManager.Instance.NextDay();
    }

    private string FormatNumber(int value)
    {
        if (value > 0)
        {
            return "+" + value;
        }

        return value.ToString();
    }

    private string FormatMoney(int value)
    {
        if (value > 0)
        {
            return "+$" +
                   value.ToString("N0");
        }

        if (value < 0)
        {
            return "-$" +
                   Mathf.Abs(value).ToString("N0");
        }

        return "$0";
    }
    
    
}