using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailListItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private GameObject unreadMarker;
    [SerializeField] private DesktopButton button;

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
    }


    public void OnClicked()
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
            if (unreadMarker != null)
                unreadMarker.SetActive(false);
        }
        
        else
        {
            if (unreadMarker != null)
                unreadMarker.SetActive(true);
        }
    }
}