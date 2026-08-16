using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameWindow : DesktopWindow
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private TMP_Text gameInfoText;

    [SerializeField] private TMP_Text gameplayValueText;
    [SerializeField] private TMP_Text storyValueText;
    [SerializeField] private TMP_Text styleValueText;
    [SerializeField] private TMP_Text audienceValueText;
    [SerializeField] private TMP_Text profitValueText;

    [SerializeField] private Image gameplayBar;
    [SerializeField] private Image storyBar;
    [SerializeField] private Image styleBar;
    [SerializeField] private Image audienceBar;
    [SerializeField] private Image profitBar;

    [SerializeField] private int maxStat = 20;

    private void Start()
    {
        base.Start();
        Refresh();
    }

    public void Refresh()
    {
        if (StudioManager.Instance == null)
            return;

        GameProject project =
            StudioManager.Instance.currentProject;

        if (project == null || project.stats == null)
            return;

        gameNameText.text =
            project.gameName;

        gameInfoText.text =
            project.genre.ToString().ToUpper() +
            " • " +
            project.theme.ToString().ToUpper() +
            " • " +
            project.platform.ToString().ToUpper();

        gameplayValueText.text =
            project.stats.gameplay.ToString();

        storyValueText.text =
            project.stats.story.ToString();

        styleValueText.text =
            project.stats.style.ToString();

        audienceValueText.text =
            project.stats.audience.ToString();

        profitValueText.text =
            project.stats.profit.ToString();

        gameplayBar.fillAmount =
            Mathf.Clamp01(
                (float)project.stats.gameplay / maxStat);

        storyBar.fillAmount =
            Mathf.Clamp01(
                (float)project.stats.story / maxStat);

        styleBar.fillAmount =
            Mathf.Clamp01(
                (float)project.stats.style / maxStat);

        audienceBar.fillAmount =
            Mathf.Clamp01(
                (float)project.stats.audience / maxStat);

        profitBar.fillAmount =
            Mathf.Clamp01(
                (float)project.stats.profit / maxStat);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}