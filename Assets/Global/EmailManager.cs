using System.Collections.Generic;
using UnityEngine;
using System;

public class EmailManager : MonoBehaviour
{
    public static EmailManager Instance { get; private set; }

    private List<EmailData> emails =
        new List<EmailData>();


    public event Action EmailsChanged;
    public IReadOnlyList<EmailData> Emails
    {
        get { return emails; }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    public void AddEmail(EmailData email)
    {
        if (email == null)
            return;

        if (HasEmail(email.id))
            return;

        emails.Add(email);

        Debug.Log(
            "New email received: " +
            email.subject);

        EmailsChanged?.Invoke();
    }


    public bool HasEmail(string id)
    {
        foreach (EmailData email in emails)
        {
            if (email.id == id)
                return true;
        }

        return false;
    }


    public void MarkAsRead(EmailData email)
    {
        if (email == null)
            return;

        if (email.isRead)
            return;

        email.isRead = true;

        EmailsChanged?.Invoke();
    }


    public int GetUnreadCount()
    {
        int count = 0;

        foreach (EmailData email in emails)
        {
            if (!email.isRead)
                count++;
        }

        return count;
    }
    
    public EmailData GetEmail(string id)
    {
        foreach (EmailData email in emails)
        {
            if (email.id == id)
                return email;
        }

        return null;
    }
    
   
}