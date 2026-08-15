using System;
using UnityEngine;
using UnityEngine.UI;

public class EmailShortcut : Shortcut
{
    [SerializeField] private Image shortcutIcon;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite unreadSprite;
    private bool unreadEmail = false;
    
    protected override void EventSubscription()
    {
        EmailManager.Instance.EmailsChanged += UpdateNotification;
    }
    
    protected override void EventUnsubscription()
    {
        EmailManager.Instance.EmailsChanged -= UpdateNotification;
    }
    
    private void UpdateNotification()
    {
        unreadEmail = EmailManager.Instance.GetUnreadCount() > 0;

        if (unreadEmail)
        {
            shortcutIcon.sprite = unreadSprite;
        }

        else
        {
            shortcutIcon.sprite = defaultSprite;
        }
    }
}
