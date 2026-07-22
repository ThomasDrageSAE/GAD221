using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartMenu : UIComponent
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] public GameObject primaryMenu;
    [SerializeField] public GameObject gameMenu;
    [SerializeField] public GameObject displayMenu;
    [SerializeField] public GameObject audioMenu;
    [SerializeField] public GameObject creditsMenu;
    
    // -- Resources & Prefabs --
    
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    
    // -- Private --
    private bool primaryMenuOpen;
    private bool subMenuOpen;
    
    #endregion
    
    #region --- Events ---

    public UnityEvent onMenuOpen = new UnityEvent();
    public UnityEvent onMenuClose = new UnityEvent();
    public UnityEvent onSubMenuOpen = new UnityEvent();
    public UnityEvent onSubMenuClose = new UnityEvent();
    
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

        primaryMenu.gameObject.SetActive(false);
        primaryMenuOpen = false;
    }

    private void OnDestroy()
    {
        EventUnsubscription();
        
        
    }

    #endregion
    
    #region --- Menu Control ---

    public void MenuButtonClicked()
    {
        if (!primaryMenuOpen)
        {
            OpenStartMenu();
        }

        else
        {
            CloseStartMenu();
        }
    }
    
    public void OpenStartMenu()
    {
        primaryMenu.SetActive(true);
        primaryMenuOpen = true;
    }

    public void CloseStartMenu()
    {
        primaryMenu.SetActive(false);
        primaryMenuOpen = false;
    }

    public void OpenSubMenu()
    {
        
    }

    public void CloseSubMenu()
    {
        
    }

    public void Shutdown()
    {
        DesktopInterface.InterfaceClose();
    }
    
    #endregion
    
    #region --- SubMenu Control ---

    public void GameSubMenu()
    {
        
    }

    public void DisplaySubMenu()
    {
        
    }

    public void AudioSubMenu()
    {
        
    }
    
    public void CreditsSubMenu()
    {
        
    }
    
    #endregion
}