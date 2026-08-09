using TMPro;
using UnityEngine;

public class OfficeHUD : MonoBehaviour
{
    [Header("HUD Text")]
    [SerializeField] private TMP_Text budgetText;
    [SerializeField] private TMP_Text publisherText;
    [SerializeField] private TMP_Text projectText;

    private void OnEnable()
    {
        if (StudioManager.Instance != null)
        {
            StudioManager.Instance.StudioDataChanged += Refresh;
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (StudioManager.Instance != null)
        {
            StudioManager.Instance.StudioDataChanged -= Refresh;
        }
    }

    public void Refresh()
    {
        StudioManager studio = StudioManager.Instance;

        if (studio == null)
        {
            Debug.LogWarning("OfficeHUD could not find StudioManager.");
            return;
        }

        budgetText.text =
            "BUDGET: $" + studio.money.ToString("N0");

        publisherText.text =
            "PUBLISHER TRUST: " + studio.publisherSatisfaction + "%";

        if (studio.currentProject == null)
        {
            projectText.text = "PROJECT: NONE";
        }
        else
        {
            projectText.text =
                "PROJECT: " + studio.currentProject.gameName;
        }
    }
}