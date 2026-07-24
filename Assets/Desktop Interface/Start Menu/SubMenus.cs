using System;
using UnityEngine;
using UnityEngine.Events;

public class SubMenus : MonoBehaviour
{
    #region --- Inspector References ---
    
    // -- In Scene --
    
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    
    
    #endregion
    
    #region --- Events ---

    public static event Action subMenusOpened;
    public static event Action subMenusClosed;
    
    private void EventSubscription()
    {
        
    }
    
    private void EventUnsubscription()
    {
        
    }

    private void SubMenusOpened()
    {
        subMenusOpened?.Invoke();
    }

    private void SubMenusClosed()
    {
        subMenusClosed?.Invoke();
    }
    
    #endregion
    
    #region --- Initialization & Termination ---
    
    private void Start()
    {
        EventSubscription();
        
        
    }
    
    private void OnDestroy()
    {
        EventUnsubscription();
        
        
    }
    
    #endregion
    
    
}
