using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailWindow : MonoBehaviour
{
    [Header("Inbox")]
    [SerializeField] private Transform emailListContent;
    [SerializeField] private EmailListItem emailListItemPrefab;

    [Header("Message Display")]
    [SerializeField] private GameObject noSelectionText;
    [SerializeField] private GameObject messageHeader;
    [SerializeField] private GameObject bodyArea;
    [SerializeField] private GameObject actionBar;

    [Header("Message Text")]
    [SerializeField] private TMP_Text fromText;
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private TMP_Text effectsText;
    [SerializeField] private TMP_Text statusText;

    [Header("Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    private EmailData selectedEmail;


    private void Start()
    {
        BuildInbox();
        ClearSelection();

        acceptButton.onClick.AddListener(AcceptPublisherDemand);
        rejectButton.onClick.AddListener(RejectPublisherDemand);
    }


    // --------------------------------------------------
    // INBOX
    // --------------------------------------------------

    private void BuildInbox()
    {
        if (EmailManager.Instance == null)
        {
            Debug.LogError(
                "EmailWindow could not find EmailManager.Instance.");

            return;
        }

        if (emailListItemPrefab == null)
        {
            Debug.LogError(
                "EmailListItem prefab has not been assigned.");

            return;
        }

        if (emailListContent == null)
        {
            Debug.LogError(
                "Email List Content has not been assigned.");

            return;
        }


        // Clear old list items.
        foreach (Transform child in emailListContent)
        {
            Destroy(child.gameObject);
        }


        // Create one list item for every email.
        foreach (EmailData email in EmailManager.Instance.Emails)
        {
            EmailListItem item =
                Instantiate(
                    emailListItemPrefab,
                    emailListContent);

            item.Setup(
                email,
                this);
        }
    }


    // --------------------------------------------------
    // SELECT EMAIL
    // --------------------------------------------------

    public void SelectEmail(EmailData email)
    {
        if (email == null)
            return;

        selectedEmail = email;

        if (noSelectionText != null)
            noSelectionText.SetActive(false);

        if (messageHeader != null)
            messageHeader.SetActive(true);

        if (bodyArea != null)
            bodyArea.SetActive(true);

        if (actionBar != null)
            actionBar.SetActive(true);


        fromText.text =
            "FROM: " + email.sender;

        subjectText.text =
            "SUBJECT: " + email.subject;

        bodyText.text =
            email.body;


        if (email.isPublisherDemand)
        {
            ShowPublisherEmail(email);
        }
        else
        {
            ShowNormalEmail();
        }
    }


    // --------------------------------------------------
    // NO EMAIL SELECTED
    // --------------------------------------------------

    private void ClearSelection()
    {
        selectedEmail = null;

        if (noSelectionText != null)
            noSelectionText.SetActive(true);

        if (messageHeader != null)
            messageHeader.SetActive(false);

        if (bodyArea != null)
            bodyArea.SetActive(false);

        if (actionBar != null)
            actionBar.SetActive(false);
    }


    // --------------------------------------------------
    // NORMAL EMAIL
    // --------------------------------------------------

    private void ShowNormalEmail()
    {
        effectsText.text = "";

        acceptButton.gameObject.SetActive(false);
        rejectButton.gameObject.SetActive(false);

        statusText.gameObject.SetActive(false);
    }


    // --------------------------------------------------
    // PUBLISHER EMAIL
    // --------------------------------------------------

    private void ShowPublisherEmail(EmailData email)
    {
        DarkPatternData demand =
            email.publisherDemand;

        if (demand == null)
        {
            effectsText.text = "";

            acceptButton.gameObject.SetActive(false);
            rejectButton.gameObject.SetActive(false);
            statusText.gameObject.SetActive(false);

            return;
        }


        effectsText.text =
            "EFFECTS\n\n" +

            "Gameplay  " +
            FormatNumber(demand.gameplayModifier) +

            "\nStory     " +
            FormatNumber(demand.storyModifier) +

            "\nStyle     " +
            FormatNumber(demand.styleModifier) +

            "\nAudience  " +
            FormatNumber(demand.audienceModifier) +

            "\nProfit    " +
            FormatNumber(demand.profitModifier) +

            "\nEthics    " +
            FormatNumber(demand.ethicsModifier);


        // This particular email has already been answered.
        if (email.hasBeenAnswered)
        {
            acceptButton.gameObject.SetActive(false);
            rejectButton.gameObject.SetActive(false);

            statusText.gameObject.SetActive(true);

            if (email.wasAccepted)
            {
                statusText.text = "ACCEPTED";
            }
            else
            {
                statusText.text = "REJECTED";
            }

            return;
        }


        // Only allow the CURRENT publisher demand to be answered.
        if (PublisherManager.Instance != null &&
            demand == PublisherManager.Instance.currentDemandData &&
            !PublisherManager.Instance.HasAnsweredCurrentDemand)
        {
            acceptButton.gameObject.SetActive(true);
            rejectButton.gameObject.SetActive(true);

            statusText.gameObject.SetActive(false);
        }
        else
        {
            // Old/unavailable demand.
            acceptButton.gameObject.SetActive(false);
            rejectButton.gameObject.SetActive(false);

            statusText.gameObject.SetActive(true);
            statusText.text = "EXPIRED";
        }
    }


    // --------------------------------------------------
    // ACCEPT
    // --------------------------------------------------

    public void AcceptPublisherDemand()
    {
        if (selectedEmail == null)
            return;

        if (!selectedEmail.isPublisherDemand)
            return;

        if (selectedEmail.hasBeenAnswered)
            return;

        if (PublisherManager.Instance == null)
            return;


        // Make sure this is actually today's demand.
        if (selectedEmail.publisherDemand !=
            PublisherManager.Instance.currentDemandData)
        {
            return;
        }


        PublisherManager.Instance.AcceptDemand();

        selectedEmail.hasBeenAnswered = true;
        selectedEmail.wasAccepted = true;

        ShowPublisherEmail(selectedEmail);
    }


    // --------------------------------------------------
    // REJECT
    // --------------------------------------------------

    public void RejectPublisherDemand()
    {
        if (selectedEmail == null)
            return;

        if (!selectedEmail.isPublisherDemand)
            return;

        if (selectedEmail.hasBeenAnswered)
            return;

        if (PublisherManager.Instance == null)
            return;


        if (selectedEmail.publisherDemand !=
            PublisherManager.Instance.currentDemandData)
        {
            return;
        }


        PublisherManager.Instance.RejectDemand();

        selectedEmail.hasBeenAnswered = true;
        selectedEmail.wasAccepted = false;

        ShowPublisherEmail(selectedEmail);
    }


    // --------------------------------------------------
    // CLOSE WINDOW
    // --------------------------------------------------

    public void CloseWindow()
    {
        Destroy(gameObject);
    }


    // --------------------------------------------------
    // HELPERS
    // --------------------------------------------------

    private string FormatNumber(int number)
    {
        if (number > 0)
            return "+" + number;

        return number.ToString();
    }
}