using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject titleScreenPanel;
    [SerializeField] private GameObject setupPanel;

    public void Play()
    {
        if (titleScreenPanel != null)
        {
            titleScreenPanel.SetActive(false);
        }

        if (setupPanel != null)
        {
            setupPanel.SetActive(true);
        }
    }

    public void Quit()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}