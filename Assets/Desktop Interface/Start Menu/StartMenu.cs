using System;
using UnityEngine;

public class StartMenu : MonoBehaviour
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

    private void EventSubscription()
    {
        
    }

    private void EventUnsubscription()
    {
        
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