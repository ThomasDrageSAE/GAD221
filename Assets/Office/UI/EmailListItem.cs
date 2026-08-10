using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailListItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private GameObject unreadMarker;
    [SerializeField] private Button button;

    private EmailData emailData;
    private EmailWindow emailWindow;


    public void Setup(
        EmailData data,
        EmailWindow owner)
    {
        emailData = data;
        emailWindow = owner;

        senderText.text = data.sender;
        subjectText.text = data.subject;

        RefreshReadState();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);
    }


    private void OnClicked()
    {
        if (emailData == null)
            return;

        if (EmailManager.Instance != null)
        {
            EmailManager.Instance.MarkAsRead(emailData);
        }

        RefreshReadState();

        if (emailWindow != null)
        {
            emailWindow.SelectEmail(emailData);
        }
    }


    public void RefreshReadState()
    {
        if (emailData == null)
            return;

        if (emailData.isRead)
        {
            senderText.fontStyle =
                FontStyles.Normal;

            subjectText.fontStyle =
                FontStyles.Normal;

            if (unreadMarker != null)
                unreadMarker.SetActive(false);
        }
        else
        {
            senderText.fontStyle =
                FontStyles.Bold;

            subjectText.fontStyle =
                FontStyles.Bold;

            if (unreadMarker != null)
                unreadMarker.SetActive(true);
        }
    }
    
}