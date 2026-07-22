using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartMenu : UIComponent
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] public GameObject primaryMenu;
    [SerializeField] public GameObject subMenuBackground;
    
    [SerializeField] public GameObject shutdownMenu;
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

    private enum SubMenu
    {
        None, Shutdown, Game, Display, Audio, Credits
    }

    private SubMenu currentSubMenu;
    
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
        subMenuBackground.gameObject.SetActive(false);
        primaryMenuOpen = false;
        currentSubMenu = SubMenu.None;
    }

    private void OnDestroy()
    {
        EventUnsubscription();
        
        
    }

    #endregion
    
    #region --- Primary Menu ---

    public void MenuButtonClicked()
    {
        Debug.Log("StartMenu - MenuButtonClicked");
        
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
        Debug.Log("StartMenu - OpenStartMenu");
        PrimaryOpenAnim();
        primaryMenu.SetActive(true);
        primaryMenuOpen = true;
    }

    private void PrimaryMenuOpened() // once anim finished
    {
        
    }

    public void CloseStartMenu()
    {
        Debug.Log("StartMenu - CloseStartMenu");
        PrimaryCloseAnim();
        primaryMenu.SetActive(false);
        primaryMenuOpen = false;
        CloseSubMenu();
    }

    private void PrimaryMenuClosed() // once anim finished
    {
        
    }
    
    #endregion
    
    #region --- Sub Menu ---

    private void OpenSubMenu(SubMenu subMenu)
    {
        Debug.Log("StartMenu - OpenSubMenu");
        
        if (currentSubMenu != SubMenu.None)
        {
            CloseSubMenu();
        }
        
        currentSubMenu = subMenu;
        
        SubOpenAnim();
        
        switch (currentSubMenu)
        {
            case SubMenu.None:
                gameMenu.SetActive(true);
                break;
            case SubMenu.Shutdown:
                shutdownMenu.SetActive(true);
                break;
            case SubMenu.Game:
                gameMenu.SetActive(true);
                break;
            case SubMenu.Display:
                displayMenu.SetActive(true);
                break;
            case SubMenu.Audio:
                audioMenu.SetActive(true);
                break;
            case SubMenu.Credits:
                creditsMenu.SetActive(true);
                break;
        }
        
        Debug.Log("StartMenu - Current Sub Menu: " + currentSubMenu);
    }

    private void SubMenuOpened() // once anim finished
    {
        
    }

    private void CloseSubMenu()
    {
        Debug.Log("StartMenu - CloseSubMenu");
        
        switch (currentSubMenu)
        {
            case SubMenu.None:
                break;
            case SubMenu.Shutdown:
                shutdownMenu.SetActive(false);
                SubCloseAnim();
                break;
            case SubMenu.Game:
                gameMenu.SetActive(false);
                SubCloseAnim();
                break;
            case SubMenu.Display:
                displayMenu.SetActive(false);
                SubCloseAnim();
                break;
            case SubMenu.Audio:
                audioMenu.SetActive(false);
                SubCloseAnim();
                break;
            case SubMenu.Credits:
                creditsMenu.SetActive(false);
                SubCloseAnim();
                break;
        }
        
        currentSubMenu = SubMenu.None;
        subMenuBackground.SetActive(false);
        
        Debug.Log("StartMenu - Current Sub Menu: " + currentSubMenu);
    }

    private void SubMenuClosed() // once anim finished.
    {
        
    }
    
    public void ShutdownButton()
    {
        Debug.Log("StartMenu - ShutdownButton");
        
        OpenSubMenu(SubMenu.Shutdown);
    }
    
    public void GameButton()
    {
        Debug.Log("StartMenu - GameButton");
        
        OpenSubMenu(SubMenu.Game);
    }

    public void DisplayButton()
    {
        Debug.Log("StartMenu - DisplayButton");
        
        OpenSubMenu(SubMenu.Display);
    }

    public void AudioButton()
    {
        Debug.Log("StartMenu - AudioButton");
        
        OpenSubMenu(SubMenu.Audio);
    }
    
    public void CreditsButton()
    {
        Debug.Log("StartMenu - CreditsButton");
        
        OpenSubMenu(SubMenu.Credits);
    }
    
    #endregion
    
    #region --- Animation ---

    private void PrimaryOpenAnim()
    {
        primaryMenu.SetActive(true);
    }

    private void PrimaryCloseAnim()
    {
        
    }

    private void SubOpenAnim()
    {
        subMenuBackground.SetActive(true);
    }

    private void SubCloseAnim()
    {
        
    }
    
    #endregion
}