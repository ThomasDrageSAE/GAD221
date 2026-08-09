using System;

[Serializable]
public class EmailData
{
    public string id;

    public string sender;
    public string subject;
    public string body;

    public bool isRead;

    public bool isPublisherDemand;

    public DarkPatternData publisherDemand;

    public bool hasBeenAnswered;
    public bool wasAccepted;


    public EmailData(
        string id,
        string sender,
        string subject,
        string body,
        bool isPublisherDemand = false,
        DarkPatternData publisherDemand = null)
    {
        this.id = id;
        this.sender = sender;
        this.subject = subject;
        this.body = body;

        this.isPublisherDemand =
            isPublisherDemand;

        this.publisherDemand =
            publisherDemand;

        isRead = false;

        hasBeenAnswered = false;
        wasAccepted = false;
    }
}