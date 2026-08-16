using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudioWindow : DesktopWindow
{
    [SerializeField] private TMP_Text studioNameText;
    [SerializeField] private TMP_Text fundsValueText;
    [SerializeField] private TMP_Text trustValueText;
    [SerializeField] private TMP_Text ethicsValueText;
    [SerializeField] private Image fundsBar;
    [SerializeField] private Image trustBar;
    [SerializeField] private Image ethicsBar;

    [SerializeField] private int maxFunds = 100000;

    protected override void Start()
    {
        base.Start();
        Refresh();
    }

    public void Refresh()
    {
        if (StudioManager.Instance == null)
            return;

        StudioManager studio = StudioManager.Instance;

        studioNameText.text = studio.studioName;

        fundsValueText.text = studio.money.ToString("N0");

        trustValueText.text = studio.publisherSatisfaction + "%";

        ethicsValueText.text = studio.ethics + "%";
        
        if (fundsBar != null)
        {
            fundsBar.fillAmount = Mathf.Clamp01((float)studio.money / maxFunds);
        }

        if (trustBar != null)
        {
            trustBar.fillAmount = Mathf.Clamp01(studio.publisherSatisfaction / 100f);
        }

        if (ethicsBar != null)
        {
            ethicsBar.fillAmount = Mathf.Clamp01(studio.ethics / 100f);
        }
    }
}