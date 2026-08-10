using System.Collections.Generic;
using UnityEngine;
using System;

public class EmailManager : Singleton<EmailManager>
{
    private List<EmailData> emails = new List<EmailData>();
    public event Action EmailsChanged;
    
    public IReadOnlyList<EmailData> Emails
    {
        get { return emails; }
    }


    protected override void Awake()
    {
       base.Awake();
       
       
    }


    public void AddEmail(EmailData email)
    {
        Debug.Log("EmailManager - AddEmail");
        
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
        Debug.Log("EmailManager - HasEmail");
        
        foreach (EmailData email in emails)
        {
            if (email.id == id)
                return true;
        }

        return false;
    }


    public void MarkAsRead(EmailData email)
    {
        Debug.Log("EmailManager - MarkAsRead");
        
        if (email == null)
            return;

        if (email.isRead)
            return;

        email.isRead = true;

        EmailsChanged?.Invoke();
    }


    public int GetUnreadCount()
    {
        Debug.Log("EmailManager - GetUnreadCount");
        
        // int count = 0;
        //
        // foreach (EmailData email in emails)
        // {
        //     if (!email.isRead)
        //         count++;
        // }

        return emails.Count;
    }
    
    public EmailData GetEmail(string id)
    {
        Debug.Log("EmailManager - GetEmail");
        
        foreach (EmailData email in emails)
        {
            if (email.id == id)
                return email;
        }

        return null;
    }
    
   
}