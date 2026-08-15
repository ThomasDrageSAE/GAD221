using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text reasonText;

    private void Awake()
    {
        Instance = this;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void ShowPublisherFailure()
    {
        ShowGameOver(
            "CONTRACT TERMINATED",
            "AAA Publishing has withdrawn funding from your studio.\n\nPublisher Trust reached 0%."
        );
    }

    public void ShowBankruptcy()
    {
        ShowGameOver(
            "STUDIO CLOSED",
            "Your studio has run out of money and can no longer continue development."
        );
    }

    private void ShowGameOver(string title, string reason)
    {
        if (DayManager.Instance != null)
        {
            DayManager.Instance.StopTimer();
        }

        titleText.text = title;
        reasonText.text = reason;

        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        if (GameSceneLoader.Instance != null)
        {
            GameSceneLoader.Instance.LoadScene("Office");
        }
    }
}